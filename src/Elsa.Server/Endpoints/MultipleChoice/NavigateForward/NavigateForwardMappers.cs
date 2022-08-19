using Elsa.CustomModels;
using System.Text.Json;

namespace Elsa.Server.Endpoints.MultipleChoice.NavigateForward
{
    public static class NavigateForwardMappers
    {
        public static MultipleChoiceQuestionModel ToMultipleChoiceQuestionModel(this NavigateForwardCommand command, string nextActivityId)
        {
            return new MultipleChoiceQuestionModel
            {
                Id = $"{command.WorkflowInstanceId}-{nextActivityId}",
                ActivityId = nextActivityId,
                Answer = command.Answer,
                FinishWorkflow = false,
                NavigateBack = command.NavigateBack,
                WorkflowInstanceId = command.WorkflowInstanceId,
                PreviousActivityId = command.ActivityId
            };
        }

        public static ActivityData? ToActivityData2(this IDictionary<string, object?>? nextActivityData)
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
