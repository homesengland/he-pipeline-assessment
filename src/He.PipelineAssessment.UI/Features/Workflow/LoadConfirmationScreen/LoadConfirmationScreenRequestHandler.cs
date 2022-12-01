using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using MediatR;
using System.Text.Json;

namespace He.PipelineAssessment.UI.Features.Workflow.LoadConfirmationScreen
{
    public class LoadConfirmationScreenRequestHandler : IRequestHandler<LoadConfirmationScreenRequest, LoadConfirmationScreenResponse?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;

        public LoadConfirmationScreenRequestHandler(IElsaServerHttpClient elsaServerHttpClient)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
        }

        public async Task<LoadConfirmationScreenResponse?> Handle(LoadConfirmationScreenRequest request, CancellationToken cancellationToken)
        {
            var response = await _elsaServerHttpClient.LoadConfirmationScreen(new LoadWorkflowActivityDto
            {
                WorkflowInstanceId = request.WorkflowInstanceId,
                ActivityId = request.ActivityId,
                ActivityType = ActivityTypeConstants.ConfirmationScreen
            });

            if (response != null)
            {
                string jsonResponse = JsonSerializer.Serialize(response);
                LoadConfirmationScreenResponse? result = JsonSerializer.Deserialize<LoadConfirmationScreenResponse>(jsonResponse);
                return await Task.FromResult(result);
            }
            else
            {
                return null;
            }
        }
    }
}
