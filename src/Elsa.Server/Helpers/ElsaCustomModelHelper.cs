using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.Providers;
using Elsa.Models;
using Question = Elsa.CustomModels.Question;

namespace Elsa.Server.Helpers
{
    public interface IElsaCustomModelHelper
    {
        CustomActivityNavigation CreateNextCustomActivityNavigation(string previousActivityId, string previousActivityType, string nextActivityId, string nextActivityType, WorkflowInstance workflowInstance);

        List<Question> CreateQuestions(string activityId, WorkflowInstance workflowInstance, string activityName, string workflowName);
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

        public Question CreateQuestion(string nextActivityId, string nextActivityType, CustomActivities.Activities.QuestionScreen.Question question, WorkflowInstance workflowInstance, string activityName, string workflowName)
        {
            
            return new Question
            {
                ActivityId = nextActivityId,
                CorrelationId = workflowInstance.CorrelationId,
                WorkflowInstanceId = workflowInstance.Id,
                CreatedDateTime = _dateTimeProvider.UtcNow(),
                QuestionId = question.Id,
                QuestionType = question.QuestionType,
                QuestionText = question.QuestionText,
                Weighting = question.QuestionWeighting,
                QuestionDataDictionaryId = question.DataDictionary == 0 ? null : question.DataDictionary,
                Choices = MapChoices(question),
                IsReadOnly = question.IsReadOnly,
                WorkflowName = workflowName,
                ActivityName = activityName
            };
        }

        private List<QuestionChoice>? MapChoices(CustomActivities.Activities.QuestionScreen.Question question)
        {
            List<QuestionChoice>? choices = new List<QuestionChoice>();
            switch (question.QuestionType)
            {
                case QuestionTypeConstants.CheckboxQuestion:
                    choices = question.Checkbox.Choices.Select(x => new QuestionChoice()
                    {
                        Identifier = x.Identifier,
                        Answer = x.Answer,
                        IsSingle = x.IsSingle,
                        IsPrePopulated = x.IsPrePopulated
                    })
                        .ToList();
                    break;
                case QuestionTypeConstants.RadioQuestion:
                    choices = question.Radio.Choices.Select(x => new QuestionChoice()
                    {
                        Identifier = x.Identifier,
                        Answer = x.Answer,
                        IsSingle = false,
                        IsPrePopulated = x.IsPrePopulated,
                        IsExclusiveToQuestion = false
                    })
                        .ToList();
                    break;
                case QuestionTypeConstants.WeightedRadioQuestion:
                    choices = question.WeightedRadio.Choices.Select(x => new QuestionChoice()
                    {
                        Identifier = x.Identifier,
                        Answer = x.Answer,
                        IsSingle = false,
                        IsPrePopulated = x.IsPrePopulated,
                        NumericScore = x.Score,
                        IsExclusiveToQuestion = false
                    })
                        .ToList();
                    break;
                case QuestionTypeConstants.PotScoreRadioQuestion:
                    choices = question.PotScoreRadio.Choices.Select(x => new QuestionChoice()
                    {
                        Identifier = x.Identifier,
                        Answer = x.Answer,
                        IsSingle = false,
                        IsPrePopulated = x.IsPrePopulated,
                        PotScoreCategory = x.PotScore,
                        IsExclusiveToQuestion = false
                    })
                        .ToList();
                    break;
                case QuestionTypeConstants.WeightedCheckboxQuestion:
                    {
                        var groups = question.WeightedCheckbox.Groups.Select(x => new QuestionChoiceGroup()
                        { GroupIdentifier = x.Key, CreatedDateTime = _dateTimeProvider.UtcNow() });
                        foreach (var weightedCheckboxGroup in question.WeightedCheckbox.Groups)
                        {
                            var group = groups.FirstOrDefault(y =>
                                y.GroupIdentifier == weightedCheckboxGroup.Value.GroupIdentifier);
                            choices.AddRange(weightedCheckboxGroup.Value.Choices.Select(x => new QuestionChoice()
                            {
                                Identifier = x.Identifier,
                                Answer = x.Answer,
                                IsSingle = x.IsSingle,
                                IsPrePopulated = x.IsPrePopulated,
                                QuestionChoiceGroup = group,
                                NumericScore = x.Score,
                                IsExclusiveToQuestion = x.IsExclusiveToQuestion
                            }).ToList());

                        }
                        break;
                    }
                default:
                    choices = null;
                    break;
            }

            return choices;
        }

        public List<Question> CreateQuestions(string activityId, WorkflowInstance workflowInstance, string activityName, string workflowName)
        {
            var questions = new List<Question>();
            //create one for each question
            var dictionList = workflowInstance.ActivityData
                .FirstOrDefault(x => x.Key == activityId).Value;

            if (dictionList != null)
            {
                var dictionaryQuestions = (AssessmentQuestions?)dictionList.FirstOrDefault(x => x.Key == "Questions").Value;

                if (dictionaryQuestions != null)
                {
                    var questionList = (List<CustomActivities.Activities.QuestionScreen.Question>)dictionaryQuestions.Questions;
                    if (questionList!.Any())
                    {
                        foreach (var item in questionList!)
                        {
                            questions.Add(this.CreateQuestion(activityId, ActivityTypeConstants.QuestionScreen, item, workflowInstance, activityName, workflowName));
                        }
                        return questions;
                    }
                }
            }

            return questions;
        }

    }
}
