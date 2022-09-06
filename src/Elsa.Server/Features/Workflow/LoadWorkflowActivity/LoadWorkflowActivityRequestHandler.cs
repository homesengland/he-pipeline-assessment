using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Models;
using Elsa.Services.Models;
using MediatR;
using Constants = Elsa.CustomActivities.Activities.Constants;

namespace Elsa.Server.Features.Workflow.LoadWorkflowActivity
{
    public class LoadWorkflowActivityRequestHandler : IRequestHandler<LoadWorkflowActivityRequest, OperationResult<LoadWorkflowActivityResponse>>
    {
        private readonly IQuestionInvoker _QuestionInvoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IPipelineAssessmentRepository _pipelineAssessmentRepository;
        private readonly ILoadWorkflowActivityMapper _loadWorkflowActivityMapper;

        public LoadWorkflowActivityRequestHandler(IWorkflowInstanceStore workflowInstanceStore, IPipelineAssessmentRepository pipelineAssessmentRepository, ILoadWorkflowActivityMapper loadWorkflowActivityMapper, IQuestionInvoker questionInvoker)
        {
            _workflowInstanceStore = workflowInstanceStore;
            _pipelineAssessmentRepository = pipelineAssessmentRepository;
            _loadWorkflowActivityMapper = loadWorkflowActivityMapper;
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
                var dbMultipleChoiceQuestionModel =
                    await _pipelineAssessmentRepository.GetMultipleChoiceQuestions(activityRequest.ActivityId, activityRequest.WorkflowInstanceId, cancellationToken);

                if(dbMultipleChoiceQuestionModel != null)
                {
                    IEnumerable<CollectedWorkflow> workflows = await _QuestionInvoker.FindWorkflowsAsync(activityRequest.ActivityId, dbMultipleChoiceQuestionModel.ActivityType, activityRequest.WorkflowInstanceId, cancellationToken);

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

                                result.Data.ActivityType = dbMultipleChoiceQuestionModel.ActivityType;
                                result.Data.PreviousActivityId = dbMultipleChoiceQuestionModel.PreviousActivityId;

                                AssignActivityData(dbMultipleChoiceQuestionModel, activityDataDictionary, result);
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
                        $"Unable to find workflow instance with Id: {activityRequest.WorkflowInstanceId} and Activity Id: {activityRequest.ActivityId}");
                }
            }
            catch (Exception e)
            {
                result.ErrorMessages.Add(e.Message);
            }

            return await Task.FromResult(result);
        }

        private void AssignActivityData(AssessmentQuestion dbAssessmentQuestion, IDictionary<string, object?> activityDataDictionary,
            OperationResult<LoadWorkflowActivityResponse> result)
        {
            if (dbAssessmentQuestion.ActivityType == Constants.MultipleChoiceQuestion)
            {
                var activityData =
                    _loadWorkflowActivityMapper.ActivityDataDictionaryToMultipleChoiceActivityData(
                        activityDataDictionary);
                if (activityData != null)
                {
                    result.Data!.MultipleChoiceQuestionActivityData = activityData;
                }
            }

            if (dbAssessmentQuestion.ActivityType == Constants.CurrencyQuestion)
            {
                var activityData =
                    _loadWorkflowActivityMapper.ActivityDataDictionaryToCurrencyActivityData(activityDataDictionary);
                if (activityData != null)
                {
                    result.Data!.CurrencyQuestionActivityData = activityData;
                }
            }

            if (dbAssessmentQuestion.ActivityType == Constants.DateQuestion)
            {
                var activityData = _loadWorkflowActivityMapper.ActivityDataDictionaryToDateActivityData(activityDataDictionary);
                if (activityData != null)
                {
                    result.Data!.DateQuestionActivityData = activityData;
                }
            }

            if (dbAssessmentQuestion.ActivityType == Constants.TextQuestion)
            {
                var activityData = _loadWorkflowActivityMapper.ActivityDataDictionaryToTextActivityData(activityDataDictionary);
                if (activityData != null)
                {
                    result.Data!.TextQuestionActivityData = activityData;
                }
            }
        }
    }
}
