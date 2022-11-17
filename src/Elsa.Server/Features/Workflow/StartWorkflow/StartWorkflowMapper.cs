using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomModels;
using Elsa.Server.Providers;
using Elsa.Services.Models;

namespace Elsa.Server.Features.Workflow.StartWorkflow
{
    public interface IStartWorkflowMapper
    {
        CustomActivityNavigation? RunWorkflowResultToCustomNavigationActivity(RunWorkflowResult result, string activityType);
        QuestionScreenQuestion? RunWorkflowResultToQuestionScreenQuestion(RunWorkflowResult result, string activityType, Question question);
        StartWorkflowResponse? RunWorkflowResultToStartWorkflowResponse(RunWorkflowResult result);
    }
    public class StartWorkflowMapper : IStartWorkflowMapper
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        public StartWorkflowMapper(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public CustomActivityNavigation? RunWorkflowResultToCustomNavigationActivity(RunWorkflowResult result, string activityType)
        {
            if (result.WorkflowInstance != null && result.WorkflowInstance
                    .LastExecutedActivityId != null)
                return new CustomActivityNavigation
                {
                    ActivityId = result.WorkflowInstance.LastExecutedActivityId,
                    ActivityType = activityType,
                    WorkflowInstanceId = result.WorkflowInstance.Id,
                    PreviousActivityId = result.WorkflowInstance.LastExecutedActivityId,
                    CreatedDateTime = _dateTimeProvider.UtcNow()
                };
            return null;
        }

        public QuestionScreenQuestion? RunWorkflowResultToQuestionScreenQuestion(RunWorkflowResult result, string activityType,
            Question question)
        {
            if (result.WorkflowInstance != null && result.WorkflowInstance
                    .LastExecutedActivityId != null)
                return new QuestionScreenQuestion
                {
                    ActivityId = result.WorkflowInstance.LastExecutedActivityId,
                    WorkflowInstanceId = result.WorkflowInstance.Id,
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
