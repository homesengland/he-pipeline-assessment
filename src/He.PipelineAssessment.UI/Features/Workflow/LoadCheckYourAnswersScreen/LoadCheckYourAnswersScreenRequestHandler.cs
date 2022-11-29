using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using MediatR;
using System.Text.Json;

namespace He.PipelineAssessment.UI.Features.Workflow.LoadCheckYourAnswersScreen
{
    public class LoadCheckYourAnswersScreenRequestHandler : IRequestHandler<LoadCheckYourAnswersScreenRequest, QuestionScreenSaveAndContinueCommand?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        public LoadCheckYourAnswersScreenRequestHandler(IElsaServerHttpClient elsaServerHttpClient)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
        }

        public async Task<QuestionScreenSaveAndContinueCommand?> Handle(LoadCheckYourAnswersScreenRequest request, CancellationToken cancellationToken)
        {
<<<<<<< HEAD
            var response = await _elsaServerHttpClient.LoadCheckYourAnswersScreen(new LoadWorkflowActivityDto
=======
            var response = await _elsaServerHttpClient.LoadQuestionScreen(new LoadWorkflowActivityDto
>>>>>>> origin/feature/start-oppourtunity-assessment-prototype
            {
                WorkflowInstanceId = request.WorkflowInstanceId,
                ActivityId = request.ActivityId,
                ActivityType = ActivityTypeConstants.CheckYourAnswersScreen
            });

            if (response != null)
            {
                string jsonResponse = JsonSerializer.Serialize(response);
                QuestionScreenSaveAndContinueCommand? result = JsonSerializer.Deserialize<QuestionScreenSaveAndContinueCommand>(jsonResponse);
                return await Task.FromResult(result);
            }
            else
            {
                return null;
            }
        }
    }
}
