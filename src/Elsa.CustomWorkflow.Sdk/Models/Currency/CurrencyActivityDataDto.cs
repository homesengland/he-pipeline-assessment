using Elsa.CustomWorkflow.Sdk.Models.Workflow;

namespace Elsa.CustomWorkflow.Sdk.Models.Currency
{
    public  class CurrencyActivityDataDto : WorkflowActivityDataDto<CurrencyQuestionDataDto>
    {

    }

    public class CurrencyQuestionDataDto : QuestionActivityDataDto
    {
        public decimal? Answer { get; set; }
    }
}
