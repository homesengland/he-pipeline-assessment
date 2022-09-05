using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.LoadWorkflowActivity;
using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.LoadWorkflowActivity
{
    public class LoadWorkflowActivityRequestHandler : IRequestHandler<LoadWorkflowActivityRequest, SaveAndContinueCommand?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        public LoadWorkflowActivityRequestHandler(IElsaServerHttpClient elsaServerHttpClient)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
        }
        public async Task<SaveAndContinueCommand?> Handle(LoadWorkflowActivityRequest request, CancellationToken cancellationToken)
        {
            var response = await _elsaServerHttpClient.LoadWorkflowActivity(new LoadWorkflowActivityDto
            {
                WorkflowInstanceId = request.WorkflowInstanceId,
                ActivityId = request.ActivityId,
                ActivityType = request.ActivityType

            });

            if (response != null)
            {

                return await Task.FromResult((SaveAndContinueCommand)response);
            }
            else
            {
                return null;
            }
        }

    }
}
