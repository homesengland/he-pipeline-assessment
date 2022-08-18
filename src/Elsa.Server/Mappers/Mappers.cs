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
                return new WorkflowExecutionResultDto
                {
                    ActivityData =
                        result.WorkflowInstance.ActivityData.ElsaActivityDataToActivityData(result.WorkflowInstance
                            .LastExecutedActivityId),
                    ActivityId = result.WorkflowInstance.LastExecutedActivityId,
                    WorkflowInstanceId = result.WorkflowInstance.Id
                };
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
                    ActivityID = result.WorkflowInstance.LastExecutedActivityId,
                    WorkflowInstanceID = result.WorkflowInstance.Id,
                    PreviousActivityId = result.WorkflowInstance.LastExecutedActivityId //this will need to change when progressing workflow
                };
            return null;
        }

        public static ActivityData? ElsaActivityDataToActivityData(this IDictionary<string, IDictionary<string, object?>> activityDataDictionary, string activityId)
        {
            var activityDataObject = activityDataDictionary[activityId];
            var jsonActivityData = JsonSerializer.Serialize(activityDataObject);

            return JsonSerializer.Deserialize<ActivityData>(jsonActivityData);
        }

        public static MultipleChoiceQuestionModel ToMultipleChoiceQuestionModel(this MultipleChoiceQuestionResponseDto multipleChoiceQuestionResponseDto, string nextActivityId)
        {
            return new MultipleChoiceQuestionModel
            {
                Id = $"{multipleChoiceQuestionResponseDto.WorkflowInstanceID}-{nextActivityId}",
                ActivityID = nextActivityId,
                Answer = multipleChoiceQuestionResponseDto.Answer,
                FinishWorkflow = false,
                NavigateBack = multipleChoiceQuestionResponseDto.NavigateBack,
                WorkflowInstanceID = multipleChoiceQuestionResponseDto.WorkflowInstanceID,
                PreviousActivityId = multipleChoiceQuestionResponseDto.ActivityID
            };
        }

        public static ActivityData ToActivityData(this IDictionary<string, object?>? nextActivityData)
        {
            var json = JsonSerializer.Serialize(nextActivityData);
            var activityData = JsonSerializer.Deserialize<ActivityData>(json);

            if (activityData.Output != null)
            {
                var output = JsonSerializer.Deserialize<MultipleChoiceQuestionModel>(activityData.Output.ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                foreach (var activityDataChoice in activityData.Choices)
                {
                    activityDataChoice.IsSelected = output.Answer.Contains(activityDataChoice.Answer);
                }
            }

            return activityData;
        }
    }
}
