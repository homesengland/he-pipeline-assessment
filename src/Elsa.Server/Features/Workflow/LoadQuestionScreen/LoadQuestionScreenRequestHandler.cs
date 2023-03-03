using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Models;
using MediatR;
using System.Text.Json;

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

                            var dbQuestions = await _elsaCustomRepository.GetQuestionScreenAnswers(
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

        private static QuestionActivityData CreateQuestionActivityData(QuestionScreenAnswer dbQuestion, Question item)
        {
            //assign the values
            var questionActivityData = new QuestionActivityData();
            questionActivityData.ActivityId = dbQuestion.ActivityId;
            questionActivityData.Answer = dbQuestion.Answer ?? item.Answer;
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
                    item.Checkbox.Choices.Select(x => new QuestionScreenAnswer.Choice()
                    { Answer = x.Answer, IsSingle = x.IsSingle }).ToArray();
            }

            if (item.QuestionType == QuestionTypeConstants.CheckboxQuestion &&
                !string.IsNullOrEmpty(questionActivityData.Answer))
            {
                var answerList =
                    JsonSerializer.Deserialize<List<string>>(
                        questionActivityData.Answer);
                questionActivityData.Checkbox.SelectedChoices =
                    answerList!;
            }

            if (item.QuestionType == QuestionTypeConstants.RadioQuestion)
            {
                questionActivityData.Radio = new Radio();
                questionActivityData.Radio.Choices = item.Radio.Choices
                    .Select(x => new QuestionScreenAnswer.Choice() { Answer = x.Answer })
                    .ToArray();
            }

            if (item.QuestionType == QuestionTypeConstants.RadioQuestion &&
                !string.IsNullOrEmpty(questionActivityData.Answer))
            {
                questionActivityData.Radio.SelectedAnswer =
                    questionActivityData.Answer;
            }

            return questionActivityData;
        }
    }
}
