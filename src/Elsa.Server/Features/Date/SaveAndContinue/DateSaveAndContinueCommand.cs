using Elsa.Server.Features.Shared;
using Elsa.Server.Features.Shared.SaveAndContinue;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Date.SaveAndContinue
{


    public class DateSaveAndContinueCommand : SaveAndContinueCommand, IRequest<OperationResult<SaveAndContinueResponse>>
    {
        public DateTime? Answer { get; set; }
    }
}
