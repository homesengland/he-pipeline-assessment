using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Workflow.LoadConfirmationScreen
{
    public class LoadConfirmationScreenRequest : IRequest<OperationResult<LoadConfirmationScreenResponse>>
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
    }
}
