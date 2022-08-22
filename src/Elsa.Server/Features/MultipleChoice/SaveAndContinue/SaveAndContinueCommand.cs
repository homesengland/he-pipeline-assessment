using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.MultipleChoice.SaveAndContinue
{
    public class SaveAndContinueCommand : IRequest<OperationResult<SaveAndContinueResponse>>
    {
        public string Id { get; set; } = null!;
        public string ActivityId { get; set; } = null!;

        public string WorkflowInstanceId { get; set; } = null!;

        public string Answer { get; set; } = null!;

        public string PreviousActivityId { get; set; } = null!;
    }
}
