using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Server.Providers;
using Elsa.Services.Models;

namespace Elsa.Server.Features.Workflow.StartWorkflow
{
    public interface IStartWorkflowMapper
    {
        AssessmentQuestion? RunWorkflowResultToAssessmentQuestion(RunWorkflowResult result,
            IActivityBlueprint activity, string? workflowName);
        StartWorkflowResponse? RunWorkflowResultToStartWorkflowResponse(RunWorkflowResult result);
        AssessmentQuestion? RunWorkflowResultToAssessmentQuestion(WorkflowInstance workflowInstance, IActivityBlueprint nextActivity, string? workflowName);
        StartWorkflowResponse RunWorkflowResultToStartWorkflowResponse(string workflowInstanceId, string? nextActivityId);
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

        public AssessmentQuestion? RunWorkflowResultToAssessmentQuestion(WorkflowInstance workflowInstance,
            IActivityBlueprint nextActivity, string? workflowName)
        {
            return new AssessmentQuestion
            {
                ActivityId = workflowInstance.LastExecutedActivityId,
                ActivityType = nextActivity.Type,
                ActivityName = nextActivity.Name,
                WorkflowInstanceId = workflowInstance.Id,
                WorkflowDefinitionId = workflowInstance.DefinitionId,
                WorkflowName = workflowName,
                PreviousActivityId = workflowInstance.LastExecutedActivityId,
                CreatedDateTime = _dateTimeProvider.UtcNow(),
                CorrelationId = workflowInstance.CorrelationId,
                Version = workflowInstance.Version
            };
        }

        public StartWorkflowResponse RunWorkflowResultToStartWorkflowResponse(string workflowInstanceId, string? nextActivityId)
        {
            return new StartWorkflowResponse
            {
                WorkflowInstanceId = workflowInstanceId,
                NextActivityId = nextActivityId
            };
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
