using Elsa.CustomModels;
using System.Globalization;
using Elsa.Server.Features.Shared.LoadWorkflowActivity;

namespace Elsa.Server.Features.Currency.LoadWorkflowActivity
{
    public interface ILoadCurrencyActivityMapper
    {

        CurrencyQuestionActivityData? ActivityDataDictionaryToCurrencyActivityData(
            IDictionary<string, object?>? activityDataDictionary);

    }

    public class LoadCurrencyActivityMapper : ILoadCurrencyActivityMapper
    {
        private readonly ILoadWorkflowActivityJsonHelper _loadWorkflowActivityJsonHelper;


        public LoadCurrencyActivityMapper(ILoadWorkflowActivityJsonHelper loadWorkflowActivityJsonHelper)
        {
            _loadWorkflowActivityJsonHelper = loadWorkflowActivityJsonHelper;
        }

        public CurrencyQuestionActivityData? ActivityDataDictionaryToCurrencyActivityData(IDictionary<string, object?>? activityDataDictionary)
        {
            var activityData = _loadWorkflowActivityJsonHelper.ActivityDataDictionaryToActivityData<CurrencyQuestionActivityData>(activityDataDictionary);

            if (activityData != null && activityData.Output != null)
            {
                var activityJson = activityData.Output.ToString();

                var output = _loadWorkflowActivityJsonHelper.ActivityOutputJsonToMultipleChoiceQuestionModel(activityJson!);
                if (output != null && output.Answer != null)
                {
                    bool isValid = Decimal.TryParse(output.Answer, out decimal result);
                    activityData.Answer = isValid ? result : null;
                }
            }
            return activityData;
        }
    }
}
