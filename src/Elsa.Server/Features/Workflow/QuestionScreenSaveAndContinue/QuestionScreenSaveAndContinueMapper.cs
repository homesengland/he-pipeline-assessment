using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Server.Providers;

namespace Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue
{
    public interface IQuestionScreenSaveAndContinueMapper
    {
        CustomActivityNavigation saveAndContinueCommandToNextCustomActivityNavigation(QuestionScreenSaveAndContinueCommand command, string nextActivityId, string nextActivityType, WorkflowInstance workflowInstance);
        QuestionScreenAnswer SaveAndContinueCommandToQuestionScreenAnswer(string nextActivityId, string type, Question item, WorkflowInstance workflowInstance);
    }

    public class QuestionScreenSaveAndContinueMapper : IQuestionScreenSaveAndContinueMapper
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public QuestionScreenSaveAndContinueMapper(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public CustomActivityNavigation saveAndContinueCommandToNextCustomActivityNavigation(QuestionScreenSaveAndContinueCommand command, string nextActivityId, string nextActivityType, WorkflowInstance workflowInstance)
        {
            return new CustomActivityNavigation
            {
                ActivityId = nextActivityId,
                ActivityType = nextActivityType,
                CorrelationId = workflowInstance.CorrelationId,
                WorkflowInstanceId = workflowInstance.Id,
                PreviousActivityId = command.ActivityId,
                PreviousActivityType = ActivityTypeConstants.QuestionScreen,
                CreatedDateTime = _dateTimeProvider.UtcNow()
            };
        }

        public QuestionScreenAnswer SaveAndContinueCommandToQuestionScreenAnswer(string nextActivityId, string nextActivityType, Question question, WorkflowInstance workflowInstance)
        {
            return new QuestionScreenAnswer
            {
                ActivityId = nextActivityId,
                Answer = null,
                Comments = null,
                CorrelationId = workflowInstance.CorrelationId,
                WorkflowInstanceId = workflowInstance.Id,
                CreatedDateTime = _dateTimeProvider.UtcNow(),
                QuestionId = question.Id,
                QuestionType = question.QuestionType,
                Question = question.QuestionText,
                Choices = MapChoices(question)
            };
        }

        private List<QuestionScreenAnswer.Choice>? MapChoices(Question question)
        {
            var choices = question.QuestionType switch
            {
                QuestionTypeConstants.CheckboxQuestion => question.Checkbox.Choices.Select(x =>
                        new QuestionScreenAnswer.Choice() { Identifier = x.Identifier, Answer = x.Answer, IsSingle = x.IsSingle })
                    .ToList(),
                QuestionTypeConstants.RadioQuestion => question.Radio.Choices
                    .Select(x => new QuestionScreenAnswer.Choice() { Identifier = x.Identifier, Answer = x.Answer, IsSingle = false })
                    .ToList(),
                _ => null
            };

            return choices;
        }
    }
}
