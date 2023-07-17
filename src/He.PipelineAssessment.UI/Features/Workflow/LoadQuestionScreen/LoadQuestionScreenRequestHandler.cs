using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using MediatR;
using Newtonsoft.Json;

namespace He.PipelineAssessment.UI.Features.Workflow.LoadQuestionScreen
{
    public class LoadQuestionScreenRequestHandler : IRequestHandler<LoadQuestionScreenRequest, QuestionScreenSaveAndContinueCommand?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IRoleValidation _roleValidation;
        private readonly ILogger<LoadQuestionScreenRequestHandler> _logger;

        public LoadQuestionScreenRequestHandler(
            IElsaServerHttpClient elsaServerHttpClient,
            IAssessmentRepository assessmentRepository,
            IRoleValidation roleValidation,
            ILogger<LoadQuestionScreenRequestHandler> logger
            )
        {
            _elsaServerHttpClient = elsaServerHttpClient;
            _assessmentRepository = assessmentRepository;
            _roleValidation = roleValidation;
            _logger = logger;
        }
        public async Task<QuestionScreenSaveAndContinueCommand?> Handle(LoadQuestionScreenRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var assessmentWorkflowInstance = await _assessmentRepository.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId);

                var isRoleExist = await _roleValidation.ValidateRole(assessmentWorkflowInstance!.AssessmentId, assessmentWorkflowInstance!.WorkflowDefinitionId);

                if (!isRoleExist && !request.IsReadOnly)
                {
                    throw new UnauthorizedAccessException($"You do not have permission to access this resource.");
                }

                if (assessmentWorkflowInstance.Status != AssessmentToolWorkflowInstanceConstants.Draft || request.IsReadOnly)
                {
                    return new QuestionScreenSaveAndContinueCommand()
                    {
                        IsReadOnly = true
                    };
                }

                var response = await _elsaServerHttpClient.LoadQuestionScreen(new LoadWorkflowActivityDto
                {
                    WorkflowInstanceId = request.WorkflowInstanceId,
                    ActivityId = request.ActivityId,
                    ActivityType = ActivityTypeConstants.QuestionScreen
                });

                if (response != null)
                {
                    var jsonSerializerSettings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
                    string jsonResponse = JsonConvert.SerializeObject(response, jsonSerializerSettings);
                    QuestionScreenSaveAndContinueCommand? result = JsonConvert.DeserializeObject<QuestionScreenSaveAndContinueCommand>(jsonResponse, jsonSerializerSettings);
                    result!.IsAuthorised = true;
                    result!.AssessmentId = assessmentWorkflowInstance.AssessmentId;
                    result!.WorkflowDefinitionId = assessmentWorkflowInstance.WorkflowDefinitionId;
                    return await Task.FromResult(result);
                }
                else
                {
                    _logger.LogError($"Failed to load Question Screen activity, response from elsa server client is null. ActivityId: {request.ActivityId} WorkflowInstanceId: {request.WorkflowInstanceId}");
                    throw new ApplicationException("Failed to load Question Screen activity, response from elsa server client is null. ActivityId: {request.ActivityId} WorkflowInstanceId: {request.WorkflowInstanceId}");
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException("Failed to load Question Screen activity. ActivityId: {request.ActivityId} WorkflowInstanceId: {request.WorkflowInstanceId}");
            }
        }

    }
}
