using System.Text.Json;
using Elsa.CustomModels;
using Elsa.Server.Models;
using Elsa.Services.Models;
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
    }
}
