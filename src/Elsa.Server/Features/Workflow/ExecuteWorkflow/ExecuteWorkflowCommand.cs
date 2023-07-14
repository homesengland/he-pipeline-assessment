using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Workflow.ExecuteWorkflow
{
    public class ExecuteWorkflowCommand : IRequest<OperationResult<ExecuteWorkflowResponse>>
    {
        public string ActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
        public string WorkflowInstanceId{ get; set; } = null!;

    }
}