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
                    PreviousActivityId = result.WorkflowInstance.LastExecutedActivityId,
                    CreatedDateTime = DateTime.UtcNow
                };
            return null;
        }

        public static StartWorkflowResponse? ToStartWorkflowResponse(
            this RunWorkflowResult result)
        {
            if (result.WorkflowInstance != null && result.WorkflowInstance
                    .LastExecutedActivityId != null)
            {
                return new StartWorkflowResponse
                {
                    WorkflowInstanceId = result.WorkflowInstance.Id,
                    NextActivityId = result.WorkflowInstance.LastExecutedActivityId,
                };
            }

            return null;
        }
    }
}
