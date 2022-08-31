using Elsa.CustomActivities.Activities;
using Elsa.CustomActivities.Activities.Currency;
using Elsa.CustomActivities.Activities.MultipleChoice;
using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Models;
using Elsa.Services.Models;
using MediatR;

namespace Elsa.Server.Features.Workflow.LoadWorkflowActivity
{
    public class LoadWorkflowActivityRequestHandler : IRequestHandler<LoadWorkflowActivityRequest, OperationResult<LoadWorkflowActivityResponse>>
    {
        //private readonly IMultipleChoiceQuestionInvoker _multipleChoiceQuestionInvoker;
        //private readonly ICurrencyQuestionInvoker _currencyQuestionInvoker;
        private readonly IQuestionInvoker _QuestionInvoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IPipelineAssessmentRepository _pipelineAssessmentRepository;
        private readonly ILoadWorkflowActivityMapper _loadWorkflowActivityMapper;

        public LoadWorkflowActivityRequestHandler(IWorkflowInstanceStore workflowInstanceStore, IPipelineAssessmentRepository pipelineAssessmentRepository, ILoadWorkflowActivityMapper loadWorkflowActivityMapper, IQuestionInvoker questionInvoker)
        {
            //_multipleChoiceQuestionInvoker = multipleChoiceQuestionInvoker;
            _workflowInstanceStore = workflowInstanceStore;
            _pipelineAssessmentRepository = pipelineAssessmentRepository;
            _loadWorkflowActivityMapper = loadWorkflowActivityMapper;
            _QuestionInvoker = questionInvoker;
            //_currencyQuestionInvoker = currencyQuestionInvoker;
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

                IEnumerable<CollectedWorkflow> workflows = null;

                if (dbMultipleChoiceQuestionModel.ActivityType == Constants.MultipleChoiceQuestion)
                {
                    workflows = await _QuestionInvoker.FindWorkflowsAsync<MultipleChoiceQuestion>(activityRequest.ActivityId, activityRequest.WorkflowInstanceId, cancellationToken);
                }

                if (dbMultipleChoiceQuestionModel.ActivityType == Constants.CurrencyQuestion)
                {
                    workflows = await _QuestionInvoker.FindWorkflowsAsync<CurrencyQuestion>(activityRequest.ActivityId, activityRequest.WorkflowInstanceId, cancellationToken);
                }

                var collectedWorkflow = workflows.FirstOrDefault();
                if (collectedWorkflow != null)
                {
                    var workflowSpecification =
                        new WorkflowInstanceIdSpecification(collectedWorkflow.WorkflowInstanceId);
                    var workflowInstance = await _workflowInstanceStore.FindAsync(workflowSpecification, cancellationToken: cancellationToken);
                    if (workflowInstance != null)
                    {

                        if (dbMultipleChoiceQuestionModel != null)
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

                                if (dbMultipleChoiceQuestionModel.ActivityType == Constants.MultipleChoiceQuestion)
                                {
                                    var activityData =
                                        _loadWorkflowActivityMapper.ActivityDataDictionaryToActivityData(
                                            activityDataDictionary);
                                    if (activityData != null)
                                    {
                                        result.Data.MultipleChoiceQuestionActivityData = activityData;
                                    }
                                }

                                if (dbMultipleChoiceQuestionModel.ActivityType == Constants.CurrencyQuestion)
                                {
                                    var activityData = _loadWorkflowActivityMapper.ActivityDataDictionaryToCurrencyActivityData(activityDataDictionary);
                                    if (activityData != null)
                                    {
                                        result.Data.CurrencyQuestionActivityData = activityData;
                                    }
                                }

                                result.Data.ActivityType = dbMultipleChoiceQuestionModel.ActivityType;
                                result.Data.PreviousActivityId = dbMultipleChoiceQuestionModel.PreviousActivityId;

                            }
                        }
                        else
                        {
                            result.ErrorMessages.Add(
                                $"Unable to find workflow instance with Id: {activityRequest.WorkflowInstanceId} and Activity Id: {activityRequest.ActivityId}");
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
            catch (Exception e)
            {
                result.ErrorMessages.Add(e.Message);
            }

            return await Task.FromResult(result);
        }
    }
}
