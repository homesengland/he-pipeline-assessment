using System.Text.Json;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.SetResult
{
    public class SetResultRequestHandler : IRequestHandler<SetResultRequest, QuestionScreenSaveAndContinueCommandResponse?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;

        public SetResultRequestHandler(IElsaServerHttpClient elsaServerHttpClient)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
        }

        public async Task<QuestionScreenSaveAndContinueCommandResponse?> Handle(SetResultRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _elsaServerHttpClient.SetResult(new LoadWorkflowActivityDto
            {
                WorkflowInstanceId = request.WorkflowInstanceId,
                ActivityId = request.ActivityId,
            });

            if (response != null)
            {
                string jsonResponse = JsonSerializer.Serialize(response);
                SetResultResponse? result = JsonSerializer.Deserialize<SetResultResponse>(jsonResponse);
                if (result != null)
                {
                    QuestionScreenSaveAndContinueCommandResponse nextActivityResponse =
                        new QuestionScreenSaveAndContinueCommandResponse
                        {
                            ActivityId = result.Data.ActivityId,
                            ActivityType = result.Data.ActivityType,
                            WorkflowInstanceId = result.Data.WorkflowInstanceId
                        };
                    return await Task.FromResult(nextActivityResponse);
                }
            }

            return null;
        }
    }
}
