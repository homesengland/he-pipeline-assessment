using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Workflow.ReturnToActivity
{
    public class ReturnToActivityCommand : IRequest<OperationResult<ReturnToActivityResponse>>
    {
        public string? WorkflowInstanceId { get; set; }
        public string? ActivityId { get; set; }
    }
}
