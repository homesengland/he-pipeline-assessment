using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Elsa.Server.Features.Workflow.LoadQuestionScreen
{
    public interface ILoadQuestionScreenJsonHelper
    {
        T? ActivityDataDictionaryToQuestionActivityData<T>(IDictionary<string, object?>? activityDataDictionary);
    }

    [ExcludeFromCodeCoverage]
    public class LoadQuestionScreenJsonHelper : ILoadQuestionScreenJsonHelper
    {
        public T? ActivityDataDictionaryToQuestionActivityData<T>(IDictionary<string, object?>? activityDataDictionary)
        {
            var json = JsonSerializer.Serialize(activityDataDictionary);
            var activityData = JsonSerializer.Deserialize<T>(json);
            return activityData;
        }
    }
}
