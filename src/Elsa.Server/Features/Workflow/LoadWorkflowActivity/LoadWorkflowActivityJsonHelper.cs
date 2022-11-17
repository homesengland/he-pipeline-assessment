using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace Elsa.Server.Features.Workflow.LoadWorkflowActivity
{
    public interface ILoadWorkflowActivityJsonHelper
    {
        T? ActivityDataDictionaryToQuestionActivityData<T>(IDictionary<string, object?>? activityDataDictionary);
    }

    [ExcludeFromCodeCoverage]
    public class LoadWorkflowActivityJsonHelper : ILoadWorkflowActivityJsonHelper
    {
        public T? ActivityDataDictionaryToQuestionActivityData<T>(IDictionary<string, object?>? activityDataDictionary)
        {
            var json = JsonSerializer.Serialize(activityDataDictionary);
            var activityData = JsonSerializer.Deserialize<T>(json);
            return activityData;
        }
    }
}
