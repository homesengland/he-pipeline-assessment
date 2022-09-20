using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomModels;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;

namespace Elsa.CustomActivities.Activities.Currency
{
    public class GetCurrencyQuestionScriptHandler : GetScriptHandler
    {
        public override string JavascriptElementName { get; set; } = "currencyQuestionResponse";
    }
}
