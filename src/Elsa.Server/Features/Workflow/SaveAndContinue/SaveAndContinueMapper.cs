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
                ActivityId = nextActivityId,
                ActivityType = nextActivityType,
                Answer = null,
                Comments = null,
                WorkflowInstanceId = workflowInstanceId,
                PreviousActivityId = previousActivityId,
                CreatedDateTime = _dateTimeProvider.UtcNow()
            };
        }
    }
}
