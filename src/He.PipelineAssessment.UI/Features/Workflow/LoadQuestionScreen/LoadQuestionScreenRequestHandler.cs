using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using MediatR;
using System.Text.Json;

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

                var isRoleExist = await _roleValidation.ValidateRole(assessmentWorkflowInstance!.AssessmentId);

                if (!isRoleExist && !request.IsReadOnly)
                {
                    return new QuestionScreenSaveAndContinueCommand()
                    {
                        IsCorrectBusinessArea = false
                    };
                }
                else
                {

                    var response = await _elsaServerHttpClient.LoadQuestionScreen(new LoadWorkflowActivityDto
                    {
                        WorkflowInstanceId = request.WorkflowInstanceId,
                        ActivityId = request.ActivityId,
                        ActivityType = ActivityTypeConstants.QuestionScreen
                    });

                    if (response != null)
                    {
                        string jsonResponse = JsonSerializer.Serialize(response);
                        QuestionScreenSaveAndContinueCommand? result = JsonSerializer.Deserialize<QuestionScreenSaveAndContinueCommand>(jsonResponse);
                        result!.IsCorrectBusinessArea = true;
                        result!.AssessmentId = assessmentWorkflowInstance.AssessmentId;
                        return await Task.FromResult(result);
                    }
                    else
                    {
                        return null;
                    }
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

                return null;
            }
        }

    }
}
