using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Workflow.LoadWorkflow
{
    public class LoadWorkflowRequest : IRequest<OperationResult<LoadWorkflowResponse>>
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
    }
}
