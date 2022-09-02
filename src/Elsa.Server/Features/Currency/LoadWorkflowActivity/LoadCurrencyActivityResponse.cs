using Elsa.Server.Features.Shared.LoadWorkflowActivity;

namespace Elsa.Server.Features.Currency.LoadWorkflowActivity
{
    public class LoadCurrencyActivityResponse : LoadWorkflowActivityResponse
    {
        public CurrencyQuestionActivityData ActivityData { get; set; } = null!;
    }

    public class CurrencyQuestionActivityData : QuestionActivityData
    {
        public decimal? Answer { get; set; }
    }
}
