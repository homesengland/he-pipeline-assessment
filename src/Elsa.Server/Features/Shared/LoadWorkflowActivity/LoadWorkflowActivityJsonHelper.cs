using Elsa.CustomModels;
using System.Text.Json;

namespace Elsa.Server.Features.Shared.LoadWorkflowActivity
{
    public interface ILoadWorkflowActivityJsonHelper
    {
        MultipleChoiceQuestionModel? ActivityOutputJsonToMultipleChoiceQuestionModel(string activityJson);

        T? ActivityDataDictionaryToActivityData<T>(IDictionary<string, object?>? activityDataDictionary);


    }

    public class LoadWorkflowActivityJsonHelper : ILoadWorkflowActivityJsonHelper
    {
        public T? ActivityDataDictionaryToActivityData<T>(IDictionary<string, object?>? activityDataDictionary)
        {
            var json = JsonSerializer.Serialize(activityDataDictionary);
            var activityData = JsonSerializer.Deserialize<T>(json);
            return activityData;
        }

        public MultipleChoiceQuestionModel? ActivityOutputJsonToMultipleChoiceQuestionModel(string activityJson)
        {
            var output = JsonSerializer.Deserialize<MultipleChoiceQuestionModel>(activityJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return output;
        }
    }
}
