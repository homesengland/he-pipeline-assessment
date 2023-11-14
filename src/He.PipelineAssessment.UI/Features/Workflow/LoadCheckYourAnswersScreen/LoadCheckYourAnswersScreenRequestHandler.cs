using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using He.PipelineAssessment.UI.Helper;
using MediatR;
using System.Text.Json;

namespace He.PipelineAssessment.UI.Features.Workflow.LoadCheckYourAnswersScreen
{
    public class LoadCheckYourAnswersScreenRequestHandler : IRequestHandler<LoadCheckYourAnswersScreenRequest, QuestionScreenSaveAndContinueCommand?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IRoleValidation _roleValidation;
        private readonly ILogger<LoadCheckYourAnswersScreenRequestHandler> _logger;

        public LoadCheckYourAnswersScreenRequestHandler(
        IElsaServerHttpClient elsaServerHttpClient,
        IAssessmentRepository assessmentRepository,
        IRoleValidation roleValidation,
        ILogger<LoadCheckYourAnswersScreenRequestHandler> logger)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
            _assessmentRepository = assessmentRepository;
            _roleValidation = roleValidation;
            _logger = logger;
        }

        public async Task<QuestionScreenSaveAndContinueCommand?> Handle(LoadCheckYourAnswersScreenRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var assessmentWorkflowInstance = await _assessmentRepository.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId);

                var isRoleExist = await _roleValidation.ValidateRole(assessmentWorkflowInstance!.AssessmentId, assessmentWorkflowInstance.WorkflowDefinitionId);

                if (!isRoleExist && !request.IsReadOnly)
                {
                    throw new UnauthorizedAccessException($"You do not have permission to access this resource.");
                }

                var response = await _elsaServerHttpClient.LoadCheckYourAnswersScreen(new LoadWorkflowActivityDto
                {
                    WorkflowInstanceId = request.WorkflowInstanceId,
                    ActivityId = request.ActivityId,
                    ActivityType = ActivityTypeConstants.CheckYourAnswersScreen
                });

                if (response != null)
                {
                    string jsonResponse = JsonSerializer.Serialize(response);
                    QuestionScreenSaveAndContinueCommand? result = JsonSerializer.Deserialize<QuestionScreenSaveAndContinueCommand>(jsonResponse);
                    var entity = await _assessmentRepository.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId);

                    if (entity != null && result != null)
                    {
                        result.AssessmentId = entity.AssessmentId;
                        result.CorrelationId = entity.Assessment.SpId;
                        result.IsAuthorised = true;
                        result.WorkflowDefinitionId= entity.WorkflowDefinitionId;
                        result.IsReadOnly = assessmentWorkflowInstance.Status !=
                                            AssessmentToolWorkflowInstanceConstants.Draft;

                        PageHeaderHelper.PopulatePageHeaderInformation(result, assessmentWorkflowInstance);
                    }

                    return await Task.FromResult(result);
                }
                else
                {
                    _logger.LogError($"Failed to load check your answers screen activity response from elsa server client is null. ActivityId: {request.ActivityId} WorkflowInstanceId: {request.WorkflowInstanceId}");
                    throw new ApplicationException("Failed to load check your answers screen activity");
                }
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException("Failed to load check your answers screen activity");
            }

        }
    }
}

