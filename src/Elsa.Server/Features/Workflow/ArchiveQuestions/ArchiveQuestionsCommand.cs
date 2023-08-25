using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Workflow.ArchiveQuestions
{
    public class ArchiveQuestionsCommand : IRequest<OperationResult<ArchiveQuestionsCommandResponse>>
    {
        public string[] WorkflowInstanceIds { get; set; } = null!;
    }
}
