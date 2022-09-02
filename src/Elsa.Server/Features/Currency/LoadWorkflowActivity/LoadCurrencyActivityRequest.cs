using Elsa.Server.Features.Shared.SaveAndContinue;
using Elsa.Server.Models;
using Elsa.Server.Features.Shared.LoadWorkflowActivity;
using MediatR;

namespace Elsa.Server.Features.Currency.LoadWorkflowActivity
{
    public class LoadCurrencyActivityRequest : LoadWorkflowActivityRequest, IRequest<OperationResult<LoadCurrencyActivityResponse>>
    {
    }
}
