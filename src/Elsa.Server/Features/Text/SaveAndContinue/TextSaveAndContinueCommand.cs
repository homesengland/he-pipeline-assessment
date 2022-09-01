using Elsa.Server.Features.Shared;
using Elsa.Server.Features.Shared.SaveAndContinue;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Text.SaveAndContinue
{


    public class TextSaveAndContinueCommand : SaveAndContinueCommand, IRequest<OperationResult<SaveAndContinueResponse>>
    {
        public string? Answer { get; set; }
    }
}
