using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomModels;
using Elsa.Server.Providers;

namespace Elsa.Server.Features.Workflow.MultiSaveAndContinue
{
    public interface IMultiSaveAndContinueMapper
    {
        CustomActivityNavigation saveAndContinueCommandToNextCustomActivityNavigation(MultiSaveAndContinueCommand command, string nextActivityId, string nextActivityType);
        QuestionScreenQuestion SaveAndContinueCommandToQuestionScreenQuestion(MultiSaveAndContinueCommand command, string nextActivityId, string type, Question item);
    }

    public class MultiSaveAndContinueMapper : IMultiSaveAndContinueMapper
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public MultiSaveAndContinueMapper(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public CustomActivityNavigation saveAndContinueCommandToNextCustomActivityNavigation(MultiSaveAndContinueCommand command, string nextActivityId, string nextActivityType)
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

        public QuestionScreenQuestion SaveAndContinueCommandToQuestionScreenQuestion(MultiSaveAndContinueCommand command, string nextActivityId, string nextActivityType, Question question)
        {
            return new QuestionScreenQuestion
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
