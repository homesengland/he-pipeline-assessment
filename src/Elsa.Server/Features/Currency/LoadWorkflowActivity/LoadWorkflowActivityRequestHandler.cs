using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Models;
using Elsa.Services.Models;
using MediatR;
using Constants = Elsa.CustomActivities.Activities.Constants;

namespace Elsa.Server.Features.Currency.LoadWorkflowActivity
{
    public class LoadWorkflowActivityRequestHandler : IRequestHandler<LoadCurrencyActivityRequest, OperationResult<LoadCurrencyActivityResponse>>
    {
        private readonly IQuestionInvoker _QuestionInvoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IPipelineAssessmentRepository _pipelineAssessmentRepository;
        private readonly ILoadCurrencyActivityMapper _loadWorkflowActivityMapper;

        public LoadWorkflowActivityRequestHandler(IWorkflowInstanceStore workflowInstanceStore, IPipelineAssessmentRepository pipelineAssessmentRepository, ILoadCurrencyActivityMapper loadWorkflowActivityMapper, IQuestionInvoker questionInvoker)
        {
            _workflowInstanceStore = workflowInstanceStore;
            _pipelineAssessmentRepository = pipelineAssessmentRepository;
            _loadWorkflowActivityMapper = loadWorkflowActivityMapper;
            _QuestionInvoker = questionInvoker;
        }

        public async Task<OperationResult<LoadCurrencyActivityResponse>> Handle(LoadCurrencyActivityRequest activityRequest, CancellationToken cancellationToken)
        {
            var result = new OperationResult<LoadCurrencyActivityResponse>
            {
                Data = new LoadCurrencyActivityResponse
                {
                    WorkflowInstanceId = activityRequest.WorkflowInstanceId,
                    ActivityId = activityRequest.ActivityId
                }
            };
            try
            {
                var dbMultipleChoiceQuestionModel =
                    await _pipelineAssessmentRepository.GetMultipleChoiceQuestions(activityRequest.ActivityId, activityRequest.WorkflowInstanceId, cancellationToken);
                if (dbMultipleChoiceQuestionModel != null)
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
                                var activityData = _loadWorkflowActivityMapper.ActivityDataDictionaryToCurrencyActivityData(activityDataDictionary);
                                if (activityData != null)
                                {
                                    result.Data.ActivityData = activityData;
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
                        $"Unable to find workflow instance with Id: {activityRequest.WorkflowInstanceId} and Activity Id: {activityRequest.ActivityId}");
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
