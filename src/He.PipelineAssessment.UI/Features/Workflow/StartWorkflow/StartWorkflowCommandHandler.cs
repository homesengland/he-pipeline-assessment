using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.UI.Features.Workflow.LoadQuestionScreen;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.StartWorkflow
{
    public class StartWorkflowCommandHandler : IRequestHandler<StartWorkflowCommand, LoadQuestionScreenRequest?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;

        public StartWorkflowCommandHandler(IElsaServerHttpClient elsaServerHttpClient)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
        }

        public async Task<LoadQuestionScreenRequest?> Handle(StartWorkflowCommand request, CancellationToken cancellationToken)
        {
            var dto = new StartWorkflowCommandDto()
            {
                WorkflowDefinitionId = request.WorkflowDefinitionId
            };
            var response = await _elsaServerHttpClient.PostStartWorkflow(dto);

            if (response != null)
            {
                var result = new LoadQuestionScreenRequest()
                {
                    ActivityId = response.Data.NextActivityId,
                    WorkflowInstanceId = response.Data.WorkflowInstanceId,
                    ActivityType = response.Data.ActivityType
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
