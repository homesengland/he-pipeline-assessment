using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomModels;
using Elsa.Server.Providers;

namespace Elsa.Server.Features.Workflow.QuestionScreenSaveAndContinue
{
    public interface IQuestionScreenSaveAndContinueMapper
    {
        CustomActivityNavigation saveAndContinueCommandToNextCustomActivityNavigation(QuestionScreenSaveAndContinueCommand command, string nextActivityId, string nextActivityType);
        QuestionScreenAnswer SaveAndContinueCommandToQuestionScreenAnswer(QuestionScreenSaveAndContinueCommand command, string nextActivityId, string type, Question item);
    }

    public class QuestionScreenSaveAndContinueMapper : IQuestionScreenSaveAndContinueMapper
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public QuestionScreenSaveAndContinueMapper(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public CustomActivityNavigation saveAndContinueCommandToNextCustomActivityNavigation(QuestionScreenSaveAndContinueCommand command, string nextActivityId, string nextActivityType)
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
                QuestionType = question.QuestionType
            };
        }
    }
}
