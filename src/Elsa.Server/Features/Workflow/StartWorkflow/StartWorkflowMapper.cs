using Elsa.CustomModels;
using Elsa.Server.Providers;
using Elsa.Services.Models;

namespace Elsa.Server.Features.Workflow.StartWorkflow
{
    public interface IStartWorkflowMapper
    {
        AssessmentQuestion? RunWorkflowResultToAssessmentQuestion(RunWorkflowResult result, string activityType);
        StartWorkflowResponse? RunWorkflowResultToStartWorkflowResponse(RunWorkflowResult result);
    }
    public class StartWorkflowMapper : IStartWorkflowMapper
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        public StartWorkflowMapper(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public AssessmentQuestion? RunWorkflowResultToAssessmentQuestion(RunWorkflowResult result, string activityType)
        {
            if (result.WorkflowInstance != null && result.WorkflowInstance
                    .LastExecutedActivityId != null)
                return new AssessmentQuestion
                {
                    ActivityId = result.WorkflowInstance.LastExecutedActivityId,
                    ActivityType = activityType,
                    WorkflowInstanceId = result.WorkflowInstance.Id,
                    PreviousActivityId = result.WorkflowInstance.LastExecutedActivityId,
                    CreatedDateTime = _dateTimeProvider.UtcNow()
                };
            return null;
        }

        public StartWorkflowResponse? RunWorkflowResultToStartWorkflowResponse(RunWorkflowResult result)
        {
            if (result.WorkflowInstance != null && result.WorkflowInstance
                    .LastExecutedActivityId != null)
            {
                return new StartWorkflowResponse
                {
                    WorkflowInstanceId = result.WorkflowInstance.Id,
                    NextActivityId = result.WorkflowInstance.LastExecutedActivityId                
                };
            }

            return null;
        }
    }
}
