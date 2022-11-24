using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;
using MediatR;
using System.Text.Json;

namespace He.PipelineAssessment.UI.Features.Workflow.LoadQuestionScreen
{
    public class LoadQuestionScreenRequestHandler : IRequestHandler<LoadQuestionScreenRequest, SaveAndContinueCommand?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        public LoadQuestionScreenRequestHandler(IElsaServerHttpClient elsaServerHttpClient)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
        }
        public async Task<SaveAndContinueCommand?> Handle(LoadQuestionScreenRequest request, CancellationToken cancellationToken)
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
                SaveAndContinueCommand? result = JsonSerializer.Deserialize<SaveAndContinueCommand>(jsonResponse);
                return await Task.FromResult(result);
            }
            else
            {
                return null;
            }
        }

    }
}
