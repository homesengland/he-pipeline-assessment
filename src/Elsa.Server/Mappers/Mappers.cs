using Elsa.CustomModels;
using Elsa.Server.Models;
using Elsa.Services.Models;
using System.Text.Json;
using ActivityData = Elsa.Server.Models.ActivityData;

namespace Elsa.Server.Mappers
{
    public static class Mappers
    {
        public static WorkflowExecutionResultDto? ToWorkflowExecutionResultDto(
            this RunWorkflowResult result)
        {
            if (result.WorkflowInstance != null && result.WorkflowInstance
                    .LastExecutedActivityId != null)
            {
                var activityData = result.WorkflowInstance.ActivityData.ToActivityData(result.WorkflowInstance
                    .LastExecutedActivityId);
                if (activityData != null)
                {
                    return new WorkflowExecutionResultDto
                    {
                        ActivityData = activityData,
                        ActivityId = result.WorkflowInstance.LastExecutedActivityId,
                        WorkflowInstanceId = result.WorkflowInstance.Id
                    };
                }
            }

            return null;
        }

        public static MultipleChoiceQuestionModel? ToMultipleChoiceQuestionModel(
            this RunWorkflowResult result)
        {
            if (result.WorkflowInstance != null && result.WorkflowInstance
                    .LastExecutedActivityId != null)
                return new MultipleChoiceQuestionModel
                {
                    Id = $"{result.WorkflowInstance.Id}-{result.WorkflowInstance.LastExecutedActivityId}",
                    ActivityId = result.WorkflowInstance.LastExecutedActivityId,
                    WorkflowInstanceId = result.WorkflowInstance.Id,
                    PreviousActivityId = result.WorkflowInstance.LastExecutedActivityId
                };
            return null;
        }

        public static ActivityData? ToActivityData(this IDictionary<string, IDictionary<string, object?>> activityDataDictionary, string activityId)
        {
            var activityDataObject = activityDataDictionary[activityId];
            var jsonActivityData = JsonSerializer.Serialize(activityDataObject);

            return JsonSerializer.Deserialize<ActivityData>(jsonActivityData);
        }

        public static MultipleChoiceQuestionModel ToMultipleChoiceQuestionModel(this MultipleChoiceQuestionResponseDto multipleChoiceQuestionResponseDto, string nextActivityId)
        {
            return new MultipleChoiceQuestionModel
            {
                Id = $"{multipleChoiceQuestionResponseDto.WorkflowInstanceId}-{nextActivityId}",
                ActivityId = nextActivityId,
                Answer = multipleChoiceQuestionResponseDto.Answer,
                FinishWorkflow = false,
                NavigateBack = multipleChoiceQuestionResponseDto.NavigateBack,
                WorkflowInstanceId = multipleChoiceQuestionResponseDto.WorkflowInstanceId,
                PreviousActivityId = multipleChoiceQuestionResponseDto.ActivityId
            };
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
