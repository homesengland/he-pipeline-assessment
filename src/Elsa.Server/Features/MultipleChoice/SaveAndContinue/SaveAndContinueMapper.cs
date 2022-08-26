using Elsa.CustomModels;
using Elsa.Server.Providers;

namespace Elsa.Server.Features.MultipleChoice.SaveAndContinue
{
    public interface ISaveAndContinueMapper
    {
        MultipleChoiceQuestionModel SaveAndContinueCommandToNextMultipleChoiceQuestionModel(SaveAndContinueCommand command, string nextActivityId);
    }

    public class SaveAndContinueMapper : ISaveAndContinueMapper
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public SaveAndContinueMapper(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public MultipleChoiceQuestionModel SaveAndContinueCommandToNextMultipleChoiceQuestionModel(SaveAndContinueCommand command, string nextActivityId)
        {
            return new MultipleChoiceQuestionModel
            {
                Id = $"{command.WorkflowInstanceId}-{nextActivityId}",
                ActivityId = nextActivityId,
                FinishWorkflow = false,
                NavigateBack = false,
                Answer = null,
                WorkflowInstanceId = command.WorkflowInstanceId,
                PreviousActivityId = command.ActivityId,
                CreatedDateTime = _dateTimeProvider.UtcNow()
            };
        }
    }
}
