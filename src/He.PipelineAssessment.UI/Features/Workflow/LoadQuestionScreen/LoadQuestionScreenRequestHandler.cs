using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using MediatR;
using System.Text.Json;

namespace He.PipelineAssessment.UI.Features.Workflow.LoadQuestionScreen
{
    public class LoadQuestionScreenRequestHandler : IRequestHandler<LoadQuestionScreenRequest, QuestionScreenSaveAndContinueCommand?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        public LoadQuestionScreenRequestHandler(IElsaServerHttpClient elsaServerHttpClient)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
        }
        public async Task<QuestionScreenSaveAndContinueCommand?> Handle(LoadQuestionScreenRequest request, CancellationToken cancellationToken)
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
                return await Task.FromResult(result);
            }
            else
            {
                return null;
            }
        }

    }
}
