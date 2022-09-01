using Elsa.Server.Features.Shared;
using Elsa.Server.Features.Shared.SaveAndContinue;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Currency.SaveAndContinue
{


    public class SaveAndContinueCommand : SaveAndContinueCommandBase, IRequest<OperationResult<SaveAndContinueResponse>>
    {
        public string? Answer { get; set; }
    }
}
