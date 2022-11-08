using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomModels;
using Elsa.Server.Providers;
using Elsa.Services.Models;

namespace Elsa.Server.Features.Workflow.StartWorkflow
{
    public interface IStartWorkflowMapper
    {
        AssessmentQuestion? RunWorkflowResultToAssessmentQuestion(RunWorkflowResult result, string activityType);
        AssessmentQuestion? RunWorkflowResultToAssessmentQuestion(RunWorkflowResult result, string activityType, Question question);
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

        public AssessmentQuestion? RunWorkflowResultToAssessmentQuestion(RunWorkflowResult result, string activityType,
            Question question)
        {
            if (result.WorkflowInstance != null && result.WorkflowInstance
                    .LastExecutedActivityId != null)
                return new AssessmentQuestion
                {
                    ActivityId = result.WorkflowInstance.LastExecutedActivityId,
                    ActivityType = activityType,
                    WorkflowInstanceId = result.WorkflowInstance.Id,
                    PreviousActivityId = result.WorkflowInstance.LastExecutedActivityId,
                    CreatedDateTime = _dateTimeProvider.UtcNow(),
                    QuestionId = question.Id,
                    QuestionType = question.QuestionType
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
