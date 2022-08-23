using Elsa.CustomWorkflow.Sdk.HttpClients;
using He.PipelineAssessment.UI.Features.Workflow.LoadWorkflowActivity;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue
{
    public class SaveAndContinueCommandHandler : IRequestHandler<SaveAndContinueCommand, LoadWorkflowActivityRequest>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        public SaveAndContinueCommandHandler(IElsaServerHttpClient elsaServerHttpClient)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
        }

        public async Task<LoadWorkflowActivityRequest> Handle(SaveAndContinueCommand request, CancellationToken cancellationToken)
        {
            var response = await _elsaServerHttpClient.SaveAndContinue(request.ToSaveAndContinueCommandDto());

            if (response != null)
            {
                var result = new LoadWorkflowActivityRequest()
                {
                    ActivityId = response.Data.NextActivityId,
                    WorkflowInstanceId = response.Data.WorkflowInstanceId
                };

                return await Task.FromResult(result);
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
