using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Workflow.LoadQuestionScreen;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.StartWorkflow
{
    public class StartWorkflowCommandHandler : IRequestHandler<StartWorkflowCommand, LoadQuestionScreenRequest?>
    {

        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<StartWorkflowCommandHandler> _logger;
        public StartWorkflowCommandHandler(IElsaServerHttpClient elsaServerHttpClient, IAssessmentRepository assessmentRepository, ILogger<StartWorkflowCommandHandler> logger)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
            _assessmentRepository = assessmentRepository;
            _logger = logger;
        }

        public async Task<LoadQuestionScreenRequest?> Handle(StartWorkflowCommand request, CancellationToken cancellationToken)
        {
            try
            {
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
                        ActivityType = response.Data.ActivityType
                    };

                    var assessmentStage = AssessmentStage(request, response);

                    await _assessmentRepository.CreateAssessmentToolWorkflowInstance(assessmentStage);

                    return await Task.FromResult(result);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return null;
            }

        }

        private static AssessmentToolWorkflowInstance AssessmentStage(StartWorkflowCommand request, WorkflowNextActivityDataDto response)
        {
            var assessmentStage = new AssessmentToolWorkflowInstance();
            assessmentStage.WorkflowInstanceId = response.Data.WorkflowInstanceId;
            assessmentStage.AssessmentId = request.AssessmentId;
            assessmentStage.Status = AssessmentStageConstants.Draft;
            assessmentStage.WorkflowName = response.Data.WorkflowName;
            assessmentStage.WorkflowDefinitionId = request.WorkflowDefinitionId;
            assessmentStage.CurrentActivityId = response.Data.NextActivityId;
            assessmentStage.CurrentActivityType = response.Data.ActivityType;
            return assessmentStage;
        }
    }
}
