using System.Text.Json;
using Elsa.CustomModels;

namespace Elsa.Server.Features.MultipleChoice.SaveAndContinue
{
    public static class SaveAndContinueMappers
    {
        public static MultipleChoiceQuestionModel ToMultipleChoiceQuestionModel(this SaveAndContinueCommand command, string nextActivityId)
        {
            return new MultipleChoiceQuestionModel
            {
                Id = $"{command.WorkflowInstanceId}-{nextActivityId}",
                ActivityId = nextActivityId,
                Answer = command.Answer,
                FinishWorkflow = false,
                NavigateBack = false,
                WorkflowInstanceId = command.WorkflowInstanceId,
                PreviousActivityId = command.ActivityId
            };
        }

        public static MultipleChoiceQuestionModel ToMultipleChoiceQuestionModel(this SaveAndContinueCommand command)
        {
            return command.ToMultipleChoiceQuestionModel("");
        }

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
