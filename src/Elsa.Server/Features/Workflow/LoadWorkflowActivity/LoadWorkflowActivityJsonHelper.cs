using Elsa.CustomModels;
using System.Text.Json;

namespace Elsa.Server.Features.Workflow.LoadWorkflowActivity
{
    public interface ILoadWorkflowActivityJsonHelper
    {
        MultipleChoiceQuestionModel? ActivityOutputJsonToMultipleChoiceQuestionModel(string activityJson);
        MultipleChoiceQuestionActivityData? ActivityDataDictionaryToActivityData(IDictionary<string, object?>? activityDataDictionary);

        CurrencyQuestionActivityData? ActivityDataDictionaryToCurrencyQuestionActivityData(
            IDictionary<string, object?>? activityDataDictionary);

        TextQuestionActivityData? ActivityDataDictionaryToTextQuestionActivityData(
            IDictionary<string, object?>? activityDataDictionary);

        DateQuestionActivityData? ActivityDataDictionaryToDateQuestionActivityData(
            IDictionary<string, object?>? activityDataDictionary);
    }

    public class LoadWorkflowActivityJsonHelper : ILoadWorkflowActivityJsonHelper
    {
        public MultipleChoiceQuestionModel? ActivityOutputJsonToMultipleChoiceQuestionModel(string activityJson)
        {
            var output = JsonSerializer.Deserialize<MultipleChoiceQuestionModel>(activityJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return output;
        }

        public MultipleChoiceQuestionActivityData? ActivityDataDictionaryToActivityData(IDictionary<string, object?>? activityDataDictionary)
        {
            var json = JsonSerializer.Serialize(activityDataDictionary);
            var activityData = JsonSerializer.Deserialize<MultipleChoiceQuestionActivityData>(json);
            return activityData;
        }

        public CurrencyQuestionActivityData? ActivityDataDictionaryToCurrencyQuestionActivityData(IDictionary<string, object?>? activityDataDictionary)
        {
            var json = JsonSerializer.Serialize(activityDataDictionary);
            var activityData = JsonSerializer.Deserialize<CurrencyQuestionActivityData>(json);
            return activityData;
        }

        public TextQuestionActivityData? ActivityDataDictionaryToTextQuestionActivityData(IDictionary<string, object?>? activityDataDictionary)
        {
            var json = JsonSerializer.Serialize(activityDataDictionary);
            var activityData = JsonSerializer.Deserialize<TextQuestionActivityData>(json);
            return activityData;
        }

        public DateQuestionActivityData? ActivityDataDictionaryToDateQuestionActivityData(IDictionary<string, object?>? activityDataDictionary)
        {
            var json = JsonSerializer.Serialize(activityDataDictionary);
            var activityData = JsonSerializer.Deserialize<DateQuestionActivityData>(json);
            return activityData;
        }
    }
}
