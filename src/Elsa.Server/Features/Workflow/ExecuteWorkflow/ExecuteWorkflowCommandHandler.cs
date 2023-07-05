using Elsa.Server.Features.Workflow.StartWorkflow;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using MediatR;

namespace Elsa.Server.Features.Workflow.ExecuteWorkflow
{
    public class ExecuteWorkflowCommandHandler : IRequestHandler<ExecuteWorkflowCommand, OperationResult<ExecuteWorkflowResponse>>
    {
        private readonly IWorkflowNextActivityProvider _nextActivityProvider;

        public ExecuteWorkflowCommandHandler(IWorkflowNextActivityProvider nextActivityProvider)
        {
            _nextActivityProvider = nextActivityProvider;
        }

        public async Task<OperationResult<ExecuteWorkflowResponse>> Handle(ExecuteWorkflowCommand command, CancellationToken cancellationToken)
        {
            var nextActivity = await _nextActivityProvider.GetNextActivity(command.ActivityId, command.WorkflowInstanceId, null,
                command.ActivityType,cancellationToken);

            return new OperationResult<ExecuteWorkflowResponse>()
            {
                Data = new ExecuteWorkflowResponse()
                {
                    WorkflowInstanceId = command.WorkflowInstanceId,
                    ActivityType = nextActivity.NextActivity.Type,
                    NextActivityId = nextActivity.NextActivity.Id
                }
            };

        }
    }
}
