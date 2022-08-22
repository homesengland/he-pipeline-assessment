using System.Text.Json;
using Elsa.CustomModels;

namespace Elsa.Server.Features.Workflow.LoadWorkflow
{
    public static class LoadWorkflowMappers
    {
        public static ActivityData? ToActivityData(this IDictionary<string, object?>? nextActivityData)
        {
            var json = JsonSerializer.Serialize(nextActivityData);
            var activityData = JsonSerializer.Deserialize<ActivityData>(json);

            if (activityData != null && activityData.Output != null)
            {
                var activityJson = activityData.Output.ToString();
                if (activityJson != null)
                {
                    var output = JsonSerializer.Deserialize<MultipleChoiceQuestionModel>(activityJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    if (output != null && output.Answer != null)
                    {
                        foreach (var activityDataChoice in activityData.Choices)
                        {
                            activityDataChoice.IsSelected = output.Answer.Contains(activityDataChoice.Answer);
                        }
                    }
                }
            }

            return activityData;
        }
    }
}
