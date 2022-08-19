using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.MultipleChoice.NavigateForward
{
    public class NavigateForwardCommand : IRequest<OperationResult<NavigateForwardResponse>>
    {
        public string Id { get; set; } = null!;
        public string ActivityId { get; set; } = null!;

        public string WorkflowInstanceId { get; set; } = null!;

        public string Answer { get; set; } = null!;

        public bool FinishWorkflow { get; set; }
        public bool NavigateBack { get; set; }
        public string PreviousActivityId { get; set; } = null!;
    }
}
