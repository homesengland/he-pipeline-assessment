using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Workflow.MultiSaveAndContinue
{
    public class MultiSaveAndContinueCommand : IRequest<OperationResult<MultiSaveAndContinueResponse>>
    {
        public string Id { get; set; } = null!;
        public string ActivityId { get; set; } = null!;

        public string WorkflowInstanceId { get; set; } = null!;
        public List<Answer>? Answers { get; set; }
    }

    public record Answer(string Id, string? AnswerText, string? Comments);
}
