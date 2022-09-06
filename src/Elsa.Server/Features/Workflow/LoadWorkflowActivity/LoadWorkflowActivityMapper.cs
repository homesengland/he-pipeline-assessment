using Elsa.CustomModels;
using System.Globalization;

namespace Elsa.Server.Features.Workflow.LoadWorkflowActivity
{
    public interface ILoadWorkflowActivityMapper
    {
        T ActivityDataDictionaryToActivityData<T>(string activityType, IDictionary<string, object?>? activityDataDictionary);

        MultipleChoiceQuestionActivityData? ActivityDataDictionaryToMultipleChoiceActivityData(IDictionary<string, object?>? activityDataDictionary);

        CurrencyQuestionActivityData? ActivityDataDictionaryToCurrencyActivityData(
            IDictionary<string, object?>? activityDataDictionary);

        DateQuestionActivityData? ActivityDataDictionaryToDateActivityData(
            IDictionary<string, object?>? activityDataDictionary);

        TextQuestionActivityData? ActivityDataDictionaryToTextActivityData(
            IDictionary<string, object?>? activityDataDictionary);
    }

    public class LoadWorkflowActivityMapper : ILoadWorkflowActivityMapper
    {
        private readonly ILoadWorkflowActivityJsonHelper _loadWorkflowActivityJsonHelper;


        public T ActivityDataDictionaryToActivityData<T>(string activityType, IDictionary<string, object?>? activityDataDictionary)
        {
            throw new NotImplementedException();
        }

        public LoadWorkflowActivityMapper(ILoadWorkflowActivityJsonHelper loadWorkflowActivityJsonHelper)
        {
            _loadWorkflowActivityJsonHelper = loadWorkflowActivityJsonHelper;
        }

        public MultipleChoiceQuestionActivityData? ActivityDataDictionaryToMultipleChoiceActivityData(IDictionary<string, object?>? activityDataDictionary)
        {
            var activityData = _loadWorkflowActivityJsonHelper.ActivityDataDictionaryToQuestionActivityData<MultipleChoiceQuestionActivityData>(activityDataDictionary);

            if (activityData != null && activityData.Output != null)
            {
                var activityJson = activityData.Output.ToString();

                var output = _loadWorkflowActivityJsonHelper.ActivityOutputJsonToMultipleChoiceQuestionModel(activityJson!);
                if (output != null && output.Answer != null)
                {
                    foreach (var activityDataChoice in activityData.Choices)
                    {
                        var answerList = output.Answer.Split(Constants.StringSeparator).ToList();
                        activityDataChoice.IsSelected = answerList.Contains(activityDataChoice.Answer);
                    }
                }
            }

            return activityData;
        }

        public CurrencyQuestionActivityData? ActivityDataDictionaryToCurrencyActivityData(IDictionary<string, object?>? activityDataDictionary)
        {
            var activityData = _loadWorkflowActivityJsonHelper.ActivityDataDictionaryToQuestionActivityData<CurrencyQuestionActivityData>(activityDataDictionary);

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

        public DateQuestionActivityData? ActivityDataDictionaryToDateActivityData(IDictionary<string, object?>? activityDataDictionary)
        {
            var activityData = _loadWorkflowActivityJsonHelper.ActivityDataDictionaryToQuestionActivityData<DateQuestionActivityData>(activityDataDictionary);

            if (activityData != null && activityData.Output != null)
            {
                var activityJson = activityData.Output.ToString();

                var output = _loadWorkflowActivityJsonHelper.ActivityOutputJsonToMultipleChoiceQuestionModel(activityJson!);
                if (output != null && output.Answer != null)
                {
                    string dateString = output.Answer;
                    bool isValidDate = DateTime.TryParseExact(output.Answer, Constants.DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out DateTime date);
                    activityData.Answer = isValidDate ? date : null;
                    activityData.Day = isValidDate ? date.Day : null;
                    activityData.Month = isValidDate ? date.Month : null;
                    activityData.Year = isValidDate ? date.Year : null;
                }
            }

            return activityData;
        }

        public TextQuestionActivityData? ActivityDataDictionaryToTextActivityData(IDictionary<string, object?>? activityDataDictionary)
        {
            var activityData = _loadWorkflowActivityJsonHelper.ActivityDataDictionaryToQuestionActivityData<TextQuestionActivityData>(activityDataDictionary);

            if (activityData != null && activityData.Output != null)
            {
                var activityJson = activityData.Output.ToString();

                var output = _loadWorkflowActivityJsonHelper.ActivityOutputJsonToMultipleChoiceQuestionModel(activityJson!);
                if (output != null && output.Answer != null)
                {
                    activityData.Answer = output.Answer;
                }
            }

            return activityData;
        }
    }
}
