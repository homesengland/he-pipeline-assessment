using Elsa.CustomModels;
using Elsa.Server.Providers;

namespace Elsa.Server.Features.Workflow.SaveAndContinue
{
    public interface ISaveAndContinueMapper
    {
        AssessmentQuestion SaveAndContinueCommandToNextAssessmentQuestion(string workflowInstanceId,
            string previousActivityId, string previousActivityInstanceId, string nextActivityId,
            string nextActivityType);
    }

    public class SaveAndContinueMapper : ISaveAndContinueMapper
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public SaveAndContinueMapper(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public AssessmentQuestion SaveAndContinueCommandToNextAssessmentQuestion(string workflowInstanceId,
            string previousActivityId, string previousActivityInstanceId, string nextActivityId,
            string nextActivityType)
        {
            return new AssessmentQuestion
            {
                Id = $"{workflowInstanceId}-{nextActivityId}",
                ActivityId = nextActivityId,
                ActivityType = nextActivityType,
                FinishWorkflow = false,
                NavigateBack = false,
                Answer = null,
                WorkflowInstanceId = workflowInstanceId,
                PreviousActivityId = previousActivityId,
                PreviousActivityInstanceId = previousActivityInstanceId,
                CreatedDateTime = _dateTimeProvider.UtcNow()
            };
        }
    }
}
