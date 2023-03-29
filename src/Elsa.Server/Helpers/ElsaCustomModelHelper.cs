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

        List<QuestionScreenQuestion> CreateQuestionScreenQuestions(string activityId, WorkflowInstance workflowInstance);
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

        public QuestionScreenQuestion CreateQuestionScreenQuestion(string nextActivityId, string nextActivityType, Question question, WorkflowInstance workflowInstance)
        {
            return new QuestionScreenQuestion
            {
                ActivityId = nextActivityId,
                CorrelationId = workflowInstance.CorrelationId,
                WorkflowInstanceId = workflowInstance.Id,
                CreatedDateTime = _dateTimeProvider.UtcNow(),
                QuestionId = question.Id,
                QuestionType = question.QuestionType,
                Question = question.QuestionText,
                Choices = MapChoices(question)
            };
        }

        private List<QuestionChoice>? MapChoices(Question question)
        {
            var choices = question.QuestionType switch
            {
                QuestionTypeConstants.CheckboxQuestion => question.Checkbox.Choices
                    .Select(x => new QuestionChoice() { Identifier = x.Identifier, Answer = x.Answer, IsSingle = x.IsSingle, IsPrePopulated = x.IsPrePopulated })
                    .ToList(),
                QuestionTypeConstants.RadioQuestion => question.Radio.Choices
                    .Select(x => new QuestionChoice() { Identifier = x.Identifier, Answer = x.Answer, IsSingle = false, IsPrePopulated = x.IsPrePopulated })
                    .ToList(),
                QuestionTypeConstants.PotScoreRadioQuestion => question.PotScoreRadio.Choices
                    .Select(x => new QuestionChoice() { Identifier = x.Identifier, Answer = x.Answer, IsSingle = false, IsPrePopulated = x.IsPrePopulated, PotScoreCategory = x.PotScore })
                    .ToList(),
                _ => null
            };

            return choices;
        }

        public List<QuestionScreenQuestion> CreateQuestionScreenQuestions(string activityId, WorkflowInstance workflowInstance)
        {
            var questionScreenQuestions = new List<QuestionScreenQuestion>();
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
                            questionScreenQuestions.Add(this.CreateQuestionScreenQuestion(activityId, ActivityTypeConstants.QuestionScreen, item, workflowInstance));
                        }
                        return questionScreenQuestions;
                    }
                }
            }

            return questionScreenQuestions;
        }
    }
}
