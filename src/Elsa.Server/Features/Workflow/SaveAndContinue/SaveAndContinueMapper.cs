//using Elsa.CustomActivities.Activities.QuestionScreen;
//using Elsa.CustomModels;
//using Elsa.Server.Providers;

//namespace Elsa.Server.Features.Workflow.SaveAndContinue
//{
//    public interface ISaveAndContinueMapper
//    {
//        AssessmentQuestion SaveAndContinueCommandToNextAssessmentQuestion(SaveAndContinueCommand command, string nextActivityId, string nextActivityType);
//        AssessmentQuestion SaveAndContinueCommandToNextAssessmentQuestion(SaveAndContinueCommand command, string nextActivityId, string type, Question item);
//    }

//    public class SaveAndContinueMapper : ISaveAndContinueMapper
//    {
//        private readonly IDateTimeProvider _dateTimeProvider;

//        public SaveAndContinueMapper(IDateTimeProvider dateTimeProvider)
//        {
//            _dateTimeProvider = dateTimeProvider;
//        }

//        public AssessmentQuestion SaveAndContinueCommandToNextAssessmentQuestion(SaveAndContinueCommand command, string nextActivityId, string nextActivityType)
//        {
//            return new AssessmentQuestion
//            {
//                ActivityId = nextActivityId,
//                ActivityType = nextActivityType,
//                FinishWorkflow = false,
//                NavigateBack = false,
//                Answer = null,
//                Comments = null,
//                WorkflowInstanceId = command.WorkflowInstanceId,
//                PreviousActivityId = command.ActivityId,
//                CreatedDateTime = _dateTimeProvider.UtcNow()
//            };
//        }

//        public AssessmentQuestion SaveAndContinueCommandToNextAssessmentQuestion(SaveAndContinueCommand command, string nextActivityId, string nextActivityType, Question question)
//        {
//            return new AssessmentQuestion
//            {
//                ActivityId = nextActivityId,
//                ActivityType = nextActivityType,
//                FinishWorkflow = false,
//                NavigateBack = false,
//                Answer = null,
//                Comments = null,
//                WorkflowInstanceId = command.WorkflowInstanceId,
//                PreviousActivityId = command.ActivityId,
//                CreatedDateTime = _dateTimeProvider.UtcNow(),
//                QuestionId = question.Id,
//                QuestionType = question.QuestionType
//            };
//        }
//    }
//}
