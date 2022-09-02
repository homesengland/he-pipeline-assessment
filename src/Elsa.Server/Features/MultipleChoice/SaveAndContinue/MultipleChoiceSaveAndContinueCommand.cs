using Elsa.Server.Features.Shared.SaveAndContinue;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.MultipleChoice.SaveAndContinue
{
    public class MultipleChoiceSaveAndContinueCommand : SaveAndContinueCommand, IRequest<OperationResult<SaveAndContinueResponse>>
    {
        public List<string> Answers { get; set; } = null!;
    }
}
