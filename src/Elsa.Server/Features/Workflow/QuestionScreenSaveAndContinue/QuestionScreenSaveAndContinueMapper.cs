using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Server.Providers;

namespace Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue
{
    public interface IQuestionScreenSaveAndContinueMapper
    {
        CustomActivityNavigation SaveAndContinueCommandToNextCustomActivityNavigation(QuestionScreenSaveAndContinueCommand command, string nextActivityId, string nextActivityType);
        QuestionScreenAnswer SaveAndContinueCommandToQuestionScreenAnswer(QuestionScreenSaveAndContinueCommand command, string nextActivityId, string type, Question item);
    }

    public class QuestionScreenSaveAndContinueMapper : IQuestionScreenSaveAndContinueMapper
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public QuestionScreenSaveAndContinueMapper(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public CustomActivityNavigation SaveAndContinueCommandToNextCustomActivityNavigation(QuestionScreenSaveAndContinueCommand command, string nextActivityId, string nextActivityType)
        {
            return new CustomActivityNavigation
            {
                ActivityId = nextActivityId,
                ActivityType = nextActivityType,
                WorkflowInstanceId = command.WorkflowInstanceId,
                PreviousActivityId = command.ActivityId,
                CreatedDateTime = _dateTimeProvider.UtcNow()
            };
        }

        public QuestionScreenAnswer SaveAndContinueCommandToQuestionScreenAnswer(QuestionScreenSaveAndContinueCommand command, string nextActivityId, string nextActivityType, Question question)
        {
            return new QuestionScreenAnswer
            {
                ActivityId = nextActivityId,
                Answer = null,
                Comments = null,
                WorkflowInstanceId = command.WorkflowInstanceId,
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
