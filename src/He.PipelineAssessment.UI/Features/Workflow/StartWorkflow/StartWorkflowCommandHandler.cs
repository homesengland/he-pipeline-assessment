using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.StartWorkflow;
using He.PipelineAssessment.UI.Features.Workflow.LoadWorkflowActivity;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.StartWorkflow
{
    public class StartWorkflowCommandHandler : IRequestHandler<StartWorkflowCommand, LoadWorkflowActivityRequest?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;

        public StartWorkflowCommandHandler(IElsaServerHttpClient elsaServerHttpClient)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
        }

        public async Task<LoadWorkflowActivityRequest?> Handle(StartWorkflowCommand request, CancellationToken cancellationToken)
        {
            var dto = new StartWorkflowCommandDto()
            {
                WorkflowDefinitionId = request.WorkflowDefinitionId
            };
            var response = await _elsaServerHttpClient.PostStartWorkflow(dto);

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
                return null;
            }

        }
    }
}
