using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Models;
using Elsa.Server.Providers;

namespace Elsa.Server.Helpers
{
    public interface IElsaCustomModelHelper
    {
        CustomActivityNavigation CreateNextCustomActivityNavigation(string previousActivityId, string previousActivityType, string nextActivityId, string nextActivityType, WorkflowInstance workflowInstance);

        List<QuestionScreenAnswer> CreateQuestionScreenAnswers(string activityId, WorkflowInstance workflowInstance);
    }

    public class ElsaCustomModelHelper : IElsaCustomModelHelper
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public ElsaCustomModelHelper(IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public CustomActivityNavigation CreateNextCustomActivityNavigation(string previousActivityId, string previousActivityType, string nextActivityId, string nextActivityType, WorkflowInstance workflowInstance)
        {
            var now = _dateTimeProvider.UtcNow();
            return new CustomActivityNavigation
            {
                ActivityId = nextActivityId,
                ActivityType = nextActivityType,
                CorrelationId = workflowInstance.CorrelationId,
                WorkflowInstanceId = workflowInstance.Id,
                PreviousActivityId = previousActivityId,
                PreviousActivityType = previousActivityType,
                CreatedDateTime = now,
                LastModifiedDateTime = now
            };
        }

        public QuestionScreenAnswer CreateQuestionScreenAnswer(string nextActivityId, string nextActivityType, Question question, WorkflowInstance workflowInstance)
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

        public List<QuestionScreenAnswer> CreateQuestionScreenAnswers(string activityId, WorkflowInstance workflowInstance)
        {
            var assessments = new List<QuestionScreenAnswer>();
            //create one for each question
            var dictionList = workflowInstance.ActivityData
                .FirstOrDefault(x => x.Key == activityId).Value;

            if (dictionList != null)
            {
                var dictionaryQuestions = (AssessmentQuestions?)dictionList.FirstOrDefault(x => x.Key == "Questions").Value;

                if (dictionaryQuestions != null)
                {
                    var questionList = (List<Question>)dictionaryQuestions.Questions;
                    if (questionList!.Any())
                    {
                        foreach (var item in questionList!)
                        {
                            assessments.Add(this.CreateQuestionScreenAnswer(activityId, ActivityTypeConstants.QuestionScreen, item, workflowInstance));
                        }
                        return assessments;
                    }
                }
            }

            return assessments;
        }
    }
}
