using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Server.Providers;
using Elsa.Services.Models;

namespace Elsa.Server.Features.Workflow.StartWorkflow
{
    public interface IStartWorkflowMapper
    {
        CustomActivityNavigation? RunWorkflowResultToCustomNavigationActivity(RunWorkflowResult result, string activityType);
        QuestionScreenAnswer? RunWorkflowResultToQuestionScreenAnswer(RunWorkflowResult result, string activityType, Question question);
        StartWorkflowResponse? RunWorkflowResultToStartWorkflowResponse(RunWorkflowResult result, string activityType);
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
                    CorrelationId = result.WorkflowInstance.CorrelationId,
                    WorkflowInstanceId = result.WorkflowInstance.Id,
                    PreviousActivityId = result.WorkflowInstance.LastExecutedActivityId,
                    PreviousActivityType = activityType,
                    CreatedDateTime = _dateTimeProvider.UtcNow()
                };
            return null;
        }

        public QuestionScreenAnswer? RunWorkflowResultToQuestionScreenAnswer(RunWorkflowResult result, string activityType,
            Question question)
        {
            if (result.WorkflowInstance != null && result.WorkflowInstance
                    .LastExecutedActivityId != null)
                return new QuestionScreenAnswer
                {
                    ActivityId = result.WorkflowInstance.LastExecutedActivityId,
                    CorrelationId = result.WorkflowInstance.CorrelationId,
                    WorkflowInstanceId = result.WorkflowInstance.Id,
                    CreatedDateTime = _dateTimeProvider.UtcNow(),
                    QuestionId = question.Id,
                    QuestionType = question.QuestionType,
                    Question = question.QuestionText,
                    Choices = MapChoices(question)
                };
            return null;
        }

        public StartWorkflowResponse? RunWorkflowResultToStartWorkflowResponse(RunWorkflowResult result,
            string activityType)
        {
            if (result.WorkflowInstance != null && result.WorkflowInstance
                    .LastExecutedActivityId != null)
            {
                return new StartWorkflowResponse
                {
                    WorkflowInstanceId = result.WorkflowInstance.Id,
                    NextActivityId = result.WorkflowInstance.LastExecutedActivityId,
                    ActivityType = activityType
                };
            }

            return null;
        }

        private List<QuestionScreenAnswer.Choice>? MapChoices(Question question)
        {
            var choices = question.QuestionType switch
            {
                QuestionTypeConstants.CheckboxQuestion => question.Checkbox.Choices.Select(x =>
                        new QuestionScreenAnswer.Choice() { Answer = x.Answer, IsSingle = x.IsSingle })
                    .ToList(),
                QuestionTypeConstants.RadioQuestion => question.Radio.Choices
                    .Select(x => new QuestionScreenAnswer.Choice() { Answer = x.Answer, IsSingle = false })
                    .ToList(),
                _ => null
            };

            return choices;
        }
    }
}
