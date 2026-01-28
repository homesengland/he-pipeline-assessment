using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Workflow.LoadQuestionScreen;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using He.PipelineAssessment.UI.Integration.ServiceBusSend;
using MediatR;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Workflow.StartWorkflow
{
    public class StartWorkflowCommandHandler : IRequestHandler<StartWorkflowCommand, LoadQuestionScreenRequest?>
    {

        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IRoleValidation _roleValidation;
        private readonly IServiceBusMessageSender _serviceBusMessageSender;
        private readonly ILogger<StartWorkflowCommandHandler> _logger;

        public StartWorkflowCommandHandler(
            IElsaServerHttpClient elsaServerHttpClient, 
            IAssessmentRepository assessmentRepository, 
            IRoleValidation roleValidation, 
            IServiceBusMessageSender serviceBusMessageSender,
            ILogger<StartWorkflowCommandHandler> logger)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
            _assessmentRepository = assessmentRepository;
            _roleValidation = roleValidation;
            _serviceBusMessageSender = serviceBusMessageSender;
            _logger = logger;
        }

        public async Task<LoadQuestionScreenRequest?> Handle(StartWorkflowCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!await _roleValidation.ValidateRole(request.AssessmentId, request.WorkflowDefinitionId))
                {
                    if(_roleValidation.IsAdmin())
                        throw new UnauthorizedAccessException($"You do not have permission to access this resource. This is a SR and you must request access from the assigned Project Manager or an Administrator.");
                    
                    throw new UnauthorizedAccessException($"You do not have permission to access this resource.");
                }

                var dto = new StartWorkflowCommandDto()
                {
                    WorkflowDefinitionId = request.WorkflowDefinitionId,
                    CorrelationId = request.CorrelationId
                };

                var response = await _elsaServerHttpClient.PostStartWorkflow(dto);

                if (response != null)
                {
                    var result = new LoadQuestionScreenRequest()
                    {
                        ActivityId = response.Data.NextActivityId,
                        WorkflowInstanceId = response.Data.WorkflowInstanceId,
                        ActivityType = response.Data.ActivityType,
                    };

                    var assessmentToolWorkflowInstance = AssessmentToolWorkflowInstance(request, response);

                    //if there is a next workflow record for the current set it to started
                    var nextWorkflow =
                        await _assessmentRepository.GetAssessmentToolInstanceNextWorkflowByAssessmentId(request.AssessmentId,
                            request.WorkflowDefinitionId);

                    if (nextWorkflow != null)
                    {
                        assessmentToolWorkflowInstance.IsVariation = nextWorkflow.IsVariation;
                        await _assessmentRepository.DeleteNextWorkflow(nextWorkflow);
                    }

                    await _assessmentRepository.CreateAssessmentToolWorkflowInstance(assessmentToolWorkflowInstance);

                    this._serviceBusMessageSender.SendMessage(assessmentToolWorkflowInstance); 
                    return await Task.FromResult(result);
                }
                else
                {
                    _logger.LogError($"Failed to start workflow, response from elsa server client is null. AssessmentId: {request.AssessmentId} WorkflowDefinitionId: {request.WorkflowDefinitionId}");
                    throw new ApplicationException("Failed to start workflow");
                }
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                throw new ApplicationException("Failed to start workflow");
            }
        }

        private static AssessmentToolWorkflowInstance AssessmentToolWorkflowInstance(StartWorkflowCommand request, WorkflowNextActivityDataDto response)
        {
            var assessmentToolWorkflowInstance = new AssessmentToolWorkflowInstance();
            assessmentToolWorkflowInstance.WorkflowInstanceId = response.Data.WorkflowInstanceId;
            assessmentToolWorkflowInstance.AssessmentId = request.AssessmentId;
            assessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Draft;
            assessmentToolWorkflowInstance.WorkflowName = response.Data.WorkflowName;
            assessmentToolWorkflowInstance.WorkflowDefinitionId = request.WorkflowDefinitionId;
            assessmentToolWorkflowInstance.CurrentActivityId = response.Data.NextActivityId;
            assessmentToolWorkflowInstance.CurrentActivityType = response.Data.ActivityType;
            assessmentToolWorkflowInstance.FirstActivityId = response.Data.FirstActivityId;
            assessmentToolWorkflowInstance.FirstActivityType = response.Data.FirstActivityType;
            assessmentToolWorkflowInstance.AssessmentToolWorkflowId = request.AssessmentToolWorkflowId;
            return assessmentToolWorkflowInstance;
        }
    }
}
