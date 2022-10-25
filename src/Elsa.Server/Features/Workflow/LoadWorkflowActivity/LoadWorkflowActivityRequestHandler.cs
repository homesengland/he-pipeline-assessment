﻿using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Models;
using Elsa.Services.Models;
using MediatR;
using System.Text.Json;
using Open.Linq.AsyncExtensions;
using Constants = Elsa.CustomActivities.Activities.Constants;
using Elsa.CustomModels;
using Elsa.Models;

namespace Elsa.Server.Features.Workflow.LoadWorkflowActivity
{
    public class LoadWorkflowActivityRequestHandler : IRequestHandler<LoadWorkflowActivityRequest, OperationResult<LoadWorkflowActivityResponse>>
    {
        private readonly IQuestionInvoker _questionInvoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly ILoadWorkflowActivityJsonHelper _loadWorkflowActivityJsonHelper;

        public LoadWorkflowActivityRequestHandler(IWorkflowInstanceStore workflowInstanceStore, IElsaCustomRepository elsaCustomRepository, IQuestionInvoker questionInvoker, ILoadWorkflowActivityJsonHelper loadWorkflowActivityJsonHelper)
        {
            _workflowInstanceStore = workflowInstanceStore;
            _elsaCustomRepository = elsaCustomRepository;
            _loadWorkflowActivityJsonHelper = loadWorkflowActivityJsonHelper;
            _questionInvoker = questionInvoker;
        }

        public async Task<OperationResult<LoadWorkflowActivityResponse>> Handle(LoadWorkflowActivityRequest activityRequest, CancellationToken cancellationToken)
        {
            var result = new OperationResult<LoadWorkflowActivityResponse>
            {
                Data = new LoadWorkflowActivityResponse
                {
                    WorkflowInstanceId = activityRequest.WorkflowInstanceId,
                    ActivityId = activityRequest.ActivityId
                }
            };
            try
            {
                var dbAssessmentQuestion =
                    await _elsaCustomRepository.GetAssessmentQuestion(activityRequest.ActivityId, activityRequest.WorkflowInstanceId, cancellationToken);

                if (dbAssessmentQuestion != null)
                {
                    IEnumerable<CollectedWorkflow> workflows = await _questionInvoker.FindWorkflowsAsync(activityRequest.ActivityId, dbAssessmentQuestion.ActivityType, activityRequest.WorkflowInstanceId, cancellationToken);

                    var collectedWorkflow = workflows.FirstOrDefault();
                    if (collectedWorkflow != null)
                    {
                        var workflowSpecification =
                            new WorkflowInstanceIdSpecification(collectedWorkflow.WorkflowInstanceId);
                        var workflowInstance = await _workflowInstanceStore.FindAsync(workflowSpecification, cancellationToken: cancellationToken);

                        if (workflowInstance != null)
                        {
                            var activityId = activityRequest.ActivityId;
                            if (workflowInstance.WorkflowStatus == WorkflowStatus.Finished)
                            {
                                var summaryScreenActivity =
                                    await _elsaCustomRepository.GetLatestAssessmentQuestionByType(workflowInstance.DefinitionId,
                                        workflowInstance.CorrelationId, "SummaryScreen", cancellationToken);
                                dbAssessmentQuestion = summaryScreenActivity;
                                activityId = summaryScreenActivity!.ActivityId;
                            }

                            if (!workflowInstance.ActivityData.ContainsKey(activityId))
                            {
                                result.ErrorMessages.Add(
                                    $"Cannot find activity Id {activityId} in the workflow activity data dictionary");
                            }
                            else
                            {
                                var activityDataDictionary =
                                    workflowInstance.ActivityData.FirstOrDefault(a => a.Key == activityId).Value;

                                result.Data.WorkflowStatus = workflowInstance.WorkflowStatus.ToString();
                                result.Data.ActivityType = dbAssessmentQuestion.ActivityType;
                                result.Data.PreviousActivityId = dbAssessmentQuestion.PreviousActivityId;

                                var activityData = _loadWorkflowActivityJsonHelper.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(activityDataDictionary);
                                if (activityData != null)
                                {
                                    result.Data!.QuestionActivityData = activityData;
                                    result.Data.QuestionActivityData.ActivityType = dbAssessmentQuestion.ActivityType;
                                    result.Data.QuestionActivityData.Answer = dbAssessmentQuestion.Answer;
                                    result.Data.QuestionActivityData.Comments = dbAssessmentQuestion.Comments;

                                    if (result.Data.ActivityType == Constants.SummaryScreen)
                                    {
                                        var assessmentQuestions = await _elsaCustomRepository
                                            .GetAssessmentQuestions(workflowInstance.DefinitionId,
                                                workflowInstance.CorrelationId)!
                                            .Where(x => x.ActivityType != "SummaryScreen");

                                        var model = new SummaryScreenModel
                                        {
                                            AssessmentQuestions = assessmentQuestions.ToList(),
                                            FooterText = activityDataDictionary["FooterText"]?.ToString(),
                                            FooterTitle = activityDataDictionary["FooterTitle"]?.ToString()
                                        };
                                        result.Data.QuestionActivityData.SummaryScreen = model;
                                    }
                                    else
                                    {
                                        // Restore preserved checkboxes from previous page load
                                        if (result.Data.ActivityType == Constants.MultipleChoiceQuestion && !string.IsNullOrEmpty(result.Data.QuestionActivityData.Answer))
                                        {
                                            var answerList = JsonSerializer.Deserialize<List<string>>(result.Data.QuestionActivityData.Answer);
                                            result.Data.QuestionActivityData.MultipleChoice.SelectedChoices = answerList!;
                                        }

                                        // Restore preserved selected answer from previous page load
                                        if (result.Data.ActivityType == Constants.SingleChoiceQuestion && !string.IsNullOrEmpty(result.Data.QuestionActivityData.Answer))
                                        {
                                            result.Data.QuestionActivityData.SingleChoice.SelectedAnswer = result.Data.QuestionActivityData.Answer;
                                        }
                                    }
                                }
                                else
                                {
                                    result.ErrorMessages.Add(
                                        $"Failed to map activity data");
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
    }
}
