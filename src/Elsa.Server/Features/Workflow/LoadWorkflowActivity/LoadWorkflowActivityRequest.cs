using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Workflow.LoadWorkflowActivity
{
    public class LoadWorkflowActivityRequest : IRequest<OperationResult<LoadWorkflowActivityResponse>>
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
    }
}
