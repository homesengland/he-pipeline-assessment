using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Models;
using Elsa.Services.Models;
using MediatR;
using System.Text.Json;
using Constants = Elsa.CustomActivities.Activities.Constants;

namespace Elsa.Server.Features.Workflow.LoadWorkflowActivity
{
    public class LoadWorkflowActivityRequestHandler : IRequestHandler<LoadWorkflowActivityRequest, OperationResult<LoadWorkflowActivityResponse>>
    {
        private readonly IQuestionInvoker _QuestionInvoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IPipelineAssessmentRepository _pipelineAssessmentRepository;
        private readonly ILoadWorkflowActivityJsonHelper _loadWorkflowActivityJsonHelper;

        public LoadWorkflowActivityRequestHandler(IWorkflowInstanceStore workflowInstanceStore, IPipelineAssessmentRepository pipelineAssessmentRepository, IQuestionInvoker questionInvoker, ILoadWorkflowActivityJsonHelper loadWorkflowActivityJsonHelper)
        {
            _workflowInstanceStore = workflowInstanceStore;
            _pipelineAssessmentRepository = pipelineAssessmentRepository;
            _loadWorkflowActivityJsonHelper = loadWorkflowActivityJsonHelper;
            _QuestionInvoker = questionInvoker;
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
                    await _pipelineAssessmentRepository.GetAssessmentQuestion(activityRequest.ActivityId, activityRequest.WorkflowInstanceId, cancellationToken);

                if (dbAssessmentQuestion != null)
                {
                    IEnumerable<CollectedWorkflow> workflows = await _QuestionInvoker.FindWorkflowsAsync(activityRequest.ActivityId, dbAssessmentQuestion.ActivityType, activityRequest.WorkflowInstanceId, cancellationToken);

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
                                    workflowInstance.ActivityData.FirstOrDefault(a => a.Key == activityRequest.ActivityId).Value;

                                result.Data.ActivityType = dbAssessmentQuestion.ActivityType;
                                result.Data.PreviousActivityId = dbAssessmentQuestion.PreviousActivityId;

                                var activityData = _loadWorkflowActivityJsonHelper.ActivityDataDictionaryToQuestionActivityData<QuestionActivityData>(activityDataDictionary);
                                if (activityData != null)
                                {
                                    result.Data!.QuestionActivityData = activityData;
                                    result.Data.QuestionActivityData.ActivityType = dbAssessmentQuestion.ActivityType;
                                    result.Data.QuestionActivityData.Answer = dbAssessmentQuestion.Answer;

                                    // Restore preserved checkboxes from previous page load
                                    if (result.Data.ActivityType == Constants.MultipleChoiceQuestion && !string.IsNullOrEmpty(result.Data.QuestionActivityData.Answer))
                                    {
                                        var answerList = JsonSerializer.Deserialize<List<string>>(result.Data.QuestionActivityData.Answer);

                                        foreach (var choice in result.Data.QuestionActivityData.Choices)
                                        {
                                            choice.IsSelected = answerList!.Contains(choice.Answer);
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
