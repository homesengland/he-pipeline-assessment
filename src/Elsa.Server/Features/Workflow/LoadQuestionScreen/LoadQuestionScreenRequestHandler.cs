using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Models;
using MediatR;
using Question = Elsa.CustomModels.Question;

namespace Elsa.Server.Features.Workflow.LoadQuestionScreen
{
    public class LoadQuestionScreenRequestHandler : IRequestHandler<LoadQuestionScreenRequest, OperationResult<LoadQuestionScreenResponse>>
    {
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IElsaCustomRepository _elsaCustomRepository;

        public LoadQuestionScreenRequestHandler(IWorkflowInstanceStore workflowInstanceStore, IElsaCustomRepository elsaCustomRepository)
        {
            _workflowInstanceStore = workflowInstanceStore;
            _elsaCustomRepository = elsaCustomRepository;
        }

        public async Task<OperationResult<LoadQuestionScreenResponse>> Handle(LoadQuestionScreenRequest activityRequest, CancellationToken cancellationToken)
        {
            var result = new OperationResult<LoadQuestionScreenResponse>
            {
                Data = new LoadQuestionScreenResponse
                {
                    WorkflowInstanceId = activityRequest.WorkflowInstanceId,
                    ActivityId = activityRequest.ActivityId
                }
            };
            try
            {
                var customActivityNavigation =
                    await _elsaCustomRepository.GetCustomActivityNavigation(activityRequest.ActivityId, activityRequest.WorkflowInstanceId, cancellationToken);

                if (customActivityNavigation != null)
                {
                    if (customActivityNavigation.ActivityType != ActivityTypeConstants.QuestionScreen)
                    {
                        throw new ApplicationException(
                            $"Attempted to load question screen with {customActivityNavigation.ActivityType} activity type");
                    }

                    var workflowSpecification =
                        new WorkflowInstanceIdSpecification(activityRequest.WorkflowInstanceId);
                    var workflowInstance = await _workflowInstanceStore.FindAsync(workflowSpecification, cancellationToken: cancellationToken);
                    if (workflowInstance != null)
                    {

                        if (!workflowInstance.ActivityData.ContainsKey(activityRequest.ActivityId))
                        {
                            result.ErrorMessages.Add(
                                $"Cannot find activity Id {activityRequest.ActivityId} in the workflow activity data dictionary");
                        }
                        else
                        {
                            var activityDataDictionary =
                                workflowInstance.ActivityData
                                    .FirstOrDefault(a => a.Key == activityRequest.ActivityId).Value;

                            result.Data.ActivityType = customActivityNavigation.ActivityType;
                            result.Data.PreviousActivityId = customActivityNavigation.PreviousActivityId;
                            result.Data.PreviousActivityType = customActivityNavigation.PreviousActivityType;

                            var title = (string?)activityDataDictionary.FirstOrDefault(x => x.Key == "PageTitle").Value;
                            result.Data.PageTitle = title;

                            var dbQuestions = await _elsaCustomRepository.GetQuestionScreenQuestions(
                                activityRequest.ActivityId, activityRequest.WorkflowInstanceId,
                                cancellationToken);

                            AssessmentQuestions? questions = (AssessmentQuestions?)activityDataDictionary.FirstOrDefault(x => x.Key == "Questions").Value;

                            var elsaActivityAssessmentQuestions =
                                (AssessmentQuestions?)activityDataDictionary
                                    .FirstOrDefault(x => x.Key == "Questions").Value;

                            if (elsaActivityAssessmentQuestions != null)
                            {
                                result.Data.QuestionScreenAnswers = new List<QuestionActivityData>();
                                result.Data.ActivityType = customActivityNavigation.ActivityType;

                                foreach (var item in elsaActivityAssessmentQuestions.Questions)
                                {
                                    //get me the item
                                    var dbQuestion =
                                        dbQuestions.FirstOrDefault(x => x.QuestionId == item.Id);
                                    if (dbQuestion != null)
                                    {
                                        var questionActivityData = CreateQuestionActivityData(dbQuestion, item);

                                        result.Data.QuestionScreenAnswers.Add(questionActivityData);
                                    }
                                }
                            }
                            else
                            {
                                result.ErrorMessages.Add(
                                    $"Failed to map activity data to QuestionScreenAnswers");
                            }
                        }
                    }
                    else
                    {
                        result.ErrorMessages.Add(
                            $"Unable to find workflow instance with Id: {activityRequest.WorkflowInstanceId} in Elsa database");
                    }
                }
                else
                {
                    result.ErrorMessages.Add(
                        $"Unable to find activity navigation with Workflow Id: {activityRequest.WorkflowInstanceId} and Activity Id: {activityRequest.ActivityId} in Elsa Custom database");
                }
            }
            catch (Exception e)
            {
                result.ErrorMessages.Add(e.Message);
            }

            return await Task.FromResult(result);
        }

        private static QuestionActivityData CreateQuestionActivityData(Question dbQuestion, CustomActivities.Activities.QuestionScreen.Question item)
        {
            //assign the values
            var questionActivityData = new QuestionActivityData();
            questionActivityData.ActivityId = dbQuestion.ActivityId;
            questionActivityData.Answer = dbQuestion.Answers != null && dbQuestion.Answers.Count() == 1 ? dbQuestion.Answers.First().AnswerText : item.Answer;
            questionActivityData.Comments = dbQuestion.Comments;
            questionActivityData.QuestionId = dbQuestion.QuestionId;
            questionActivityData.QuestionType = dbQuestion.QuestionType;

            questionActivityData.Question = item.QuestionText;
            questionActivityData.DisplayComments = item.DisplayComments;
            questionActivityData.QuestionGuidance = item.QuestionGuidance;
            questionActivityData.QuestionHint = item.QuestionHint;
            questionActivityData.CharacterLimit = item.CharacterLimit;
            questionActivityData.IsReadOnly = item.IsReadOnly;

            if (item.QuestionType == QuestionTypeConstants.CheckboxQuestion)
            {
                questionActivityData.Checkbox = new Checkbox();
                questionActivityData.Checkbox.Choices =
                    item.Checkbox.Choices.Select(x => new QuestionChoice()
                    { Answer = x.Answer, IsSingle = x.IsSingle }).ToArray();
            }

            if (item.QuestionType == QuestionTypeConstants.CheckboxQuestion &&
                          string.IsNullOrEmpty(questionActivityData.Answer) && item.Checkbox.Choices.Any(x => x.IsPrePopulated))
            {
                var answerList = item.Checkbox.Choices.Where(x => x.IsPrePopulated).Select(x => x.Answer).ToList();

                questionActivityData.Checkbox.SelectedChoices = answerList;
            }

            if (item.QuestionType == QuestionTypeConstants.CheckboxQuestion &&
                dbQuestion.Answers != null && dbQuestion.Answers.Any())
            {
                var answerList = dbQuestion.Answers.Select(x => x.AnswerText).ToList();

                questionActivityData.Checkbox.SelectedChoices =
                    answerList!;
            }

            if (item.QuestionType == QuestionTypeConstants.RadioQuestion)
            {
                questionActivityData.Radio = new Radio();
                questionActivityData.Radio.Choices = item.Radio.Choices
                    .Select(x => new QuestionChoice() { Answer = x.Answer })
                    .ToArray();
            }

            if (item.QuestionType == QuestionTypeConstants.PotScoreRadioQuestion)
            {
                questionActivityData.Radio = new Radio();
                questionActivityData.Radio.Choices = item.PotScoreRadio.Choices
                    .Select(x => new QuestionChoice() { Answer = x.Answer })
                    .ToArray();
            }

            if ((item.QuestionType == QuestionTypeConstants.RadioQuestion) &&
               string.IsNullOrEmpty(questionActivityData.Answer) && item.Radio.Choices.Any(x => x.IsPrePopulated))
            {
                questionActivityData.Radio.SelectedAnswer =
                    item.Radio.Choices.First(x => x.IsPrePopulated).Answer;
            }

            if ((item.QuestionType == QuestionTypeConstants.PotScoreRadioQuestion) &&
                string.IsNullOrEmpty(questionActivityData.Answer) && item.PotScoreRadio.Choices.Any(x => x.IsPrePopulated))
            {
                questionActivityData.Radio.SelectedAnswer =
                    item.PotScoreRadio.Choices.First(x => x.IsPrePopulated).Answer;
            }

            if ((item.QuestionType == QuestionTypeConstants.RadioQuestion || item.QuestionType == QuestionTypeConstants.PotScoreRadioQuestion) &&
                 dbQuestion.Answers != null && dbQuestion.Answers.Any())
            {
                questionActivityData.Radio.SelectedAnswer = dbQuestion.Answers.First().AnswerText;
            }

            if (item.QuestionType == QuestionTypeConstants.Information)
            {
                questionActivityData.Information = new Information();
                questionActivityData.Information.InformationTextList = item.Text.TextRecords
                    .Select(x => new InformationText() { Text = x.Text, IsGuidance = x.IsGuidance, IsParagraph = x.IsParagraph, IsHyperlink = x.IsHyperlink, Url = x.Url })
                    .ToArray();
            }

            return questionActivityData;
        }
    }
}
