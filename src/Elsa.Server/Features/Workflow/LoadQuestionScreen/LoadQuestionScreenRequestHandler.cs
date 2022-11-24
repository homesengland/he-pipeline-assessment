using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Models;
using Elsa.Services.Models;
using MediatR;
using System.Text.Json;

namespace Elsa.Server.Features.Workflow.LoadQuestionScreen
{
    public class LoadQuestionScreenRequestHandler : IRequestHandler<LoadQuestionScreenRequest, OperationResult<LoadQuestionScreenResponse>>
    {
        private readonly IQuestionInvoker _questionInvoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IElsaCustomRepository _elsaCustomRepository;

        public LoadQuestionScreenRequestHandler(IWorkflowInstanceStore workflowInstanceStore, IElsaCustomRepository elsaCustomRepository, IQuestionInvoker questionInvoker)
        {
            _workflowInstanceStore = workflowInstanceStore;
            _elsaCustomRepository = elsaCustomRepository;
            _questionInvoker = questionInvoker;
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
                var dbActivity =
                    await _elsaCustomRepository.GetCustomActivityNavigation(activityRequest.ActivityId, activityRequest.WorkflowInstanceId, cancellationToken);

                if (dbActivity != null)
                {
                    IEnumerable<CollectedWorkflow> workflows = await _questionInvoker.FindWorkflowsAsync(activityRequest.ActivityId, dbActivity.ActivityType, activityRequest.WorkflowInstanceId, cancellationToken);

                    var collectedWorkflow = workflows.FirstOrDefault();
                    if (collectedWorkflow != null)
                    {
                        var workflowSpecification =
                            new WorkflowInstanceIdSpecification(collectedWorkflow.WorkflowInstanceId);
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

                                if (activityDataDictionary != null)
                                {
                                    result.Data.ActivityType = dbActivity.ActivityType;
                                    result.Data.PreviousActivityId = dbActivity.PreviousActivityId;

                                    if (dbActivity.ActivityType == ActivityTypeConstants.QuestionScreen)
                                    {
                                        var title = (string?)activityDataDictionary.FirstOrDefault(x => x.Key == "PageTitle").Value;
                                        result.Data.PageTitle = title;

                                        var dbQuestions = await _elsaCustomRepository.GetQuestionScreenAnswers(
                                            activityRequest.ActivityId, activityRequest.WorkflowInstanceId,
                                            cancellationToken);

                                        var elsaActivityAssessmentQuestions =
                                            (AssessmentQuestions?)activityDataDictionary
                                                .FirstOrDefault(x => x.Key == "Questions").Value;

                                        if (elsaActivityAssessmentQuestions != null)
                                        {
                                            result.Data.MultiQuestionActivityData = new List<QuestionActivityData>();
                                            result.Data.ActivityType = dbActivity.ActivityType;

                                            foreach (var item in elsaActivityAssessmentQuestions.Questions)
                                            {
                                                //get me the item
                                                var dbQuestion =
                                                    dbQuestions.FirstOrDefault(x => x.QuestionId == item.Id);
                                                if (dbQuestion != null)
                                                {
                                                    var questionActivityData = CreateQuestionActivityData(dbQuestion, item);

                                                    result.Data.MultiQuestionActivityData.Add(questionActivityData);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            result.ErrorMessages.Add(
                                                $"Failed to map activity data to MultiQuestionActivityData");
                                        }
                                    }

                                }
                                else
                                {
                                    result.ErrorMessages.Add(
                                        $"Activity data is null for Activity Id: {activityRequest.ActivityId}");
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
                            $"Unable to progress workflow instance Id {activityRequest.WorkflowInstanceId}. No collected workflows");
                    }
                }
                else
                {
                    result.ErrorMessages.Add(
                        $"Unable to find workflow instance with Id: {activityRequest.WorkflowInstanceId} and Activity Id: {activityRequest.ActivityId} in Pipeline Assessment database");
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
            questionActivityData.Answer = dbQuestion.Answer;
            questionActivityData.Comments = dbQuestion.Comments;
            questionActivityData.QuestionId = dbQuestion.QuestionId;
            questionActivityData.QuestionType = dbQuestion.QuestionType;

            questionActivityData.Question = item.QuestionText;
            questionActivityData.DisplayComments = item.DisplayComments;
            questionActivityData.QuestionGuidance = item.QuestionGuidance;
            questionActivityData.QuestionHint = item.QuestionHint;

            if (item.QuestionType == QuestionTypeConstants.CheckboxQuestion)
            {
                questionActivityData.Checkbox = new Checkbox();
                questionActivityData.Checkbox.Choices =
                    item.Checkbox.Choices.Select(x => new Choice()
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
                    .Select(x => new Choice() { Answer = x.Answer })
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
