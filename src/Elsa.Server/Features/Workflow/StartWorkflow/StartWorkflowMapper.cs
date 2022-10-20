using Elsa.CustomModels;
using Elsa.Server.Providers;
using Elsa.Services.Models;

namespace Elsa.Server.Features.Workflow.StartWorkflow
{
    public interface IStartWorkflowMapper
    {
        AssessmentQuestion? RunWorkflowResultToAssessmentQuestion(RunWorkflowResult result,
            IActivityBlueprint activity, string? workflowName);
        StartWorkflowResponse? RunWorkflowResultToStartWorkflowResponse(RunWorkflowResult result);
    }
    public class StartWorkflowMapper : IStartWorkflowMapper
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        public StartWorkflowMapper(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public AssessmentQuestion? RunWorkflowResultToAssessmentQuestion(RunWorkflowResult result,
            IActivityBlueprint activity, string? workflowName)
        {
            if (result.WorkflowInstance != null && result.WorkflowInstance
                    .LastExecutedActivityId != null)
                return new AssessmentQuestion
                {
                    ActivityId = result.WorkflowInstance.LastExecutedActivityId,
                    ActivityType = activity.Type,
                    ActivityName = activity.Name,
                    WorkflowInstanceId = result.WorkflowInstance.Id,
                    WorkflowDefinitionId = result.WorkflowInstance.DefinitionId,
                    WorkflowName = workflowName,
                    PreviousActivityId = result.WorkflowInstance.LastExecutedActivityId,
                    CreatedDateTime = _dateTimeProvider.UtcNow(),
                    CorrelationId = result.WorkflowInstance.CorrelationId,
                    Version = result.WorkflowInstance.Version
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
