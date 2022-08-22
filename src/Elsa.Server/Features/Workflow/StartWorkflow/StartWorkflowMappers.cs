using System.Text.Json;
using Elsa.CustomModels;
using Elsa.Services.Models;

namespace Elsa.Server.Features.Workflow.StartWorkflow
{
    public static class StartWorkflowMappers
    {
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

        public static StartWorkflowResponse? ToStartWorkflowResponse(
            this RunWorkflowResult result)
        {
            if (result.WorkflowInstance != null && result.WorkflowInstance
                    .LastExecutedActivityId != null)
            {
                var activityData = result.WorkflowInstance.ActivityData.ToActivityData(result.WorkflowInstance
                    .LastExecutedActivityId);
                if (activityData != null)
                {
                    return new StartWorkflowResponse
                    {
                        ActivityData = activityData,
                        ActivityId = result.WorkflowInstance.LastExecutedActivityId,
                        WorkflowInstanceId = result.WorkflowInstance.Id
                    };
                }
            }

            return null;
        }

        public static ActivityData? ToActivityData(this IDictionary<string, IDictionary<string, object?>> activityDataDictionary, string activityId)
        {
            var activityDataObject = activityDataDictionary[activityId];
            var jsonActivityData = JsonSerializer.Serialize(activityDataObject);

            return JsonSerializer.Deserialize<ActivityData>(jsonActivityData);
        }
    }
}
