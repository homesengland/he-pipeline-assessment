using Elsa.CustomActivities.Activities.MultipleChoice;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Persistence;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Workflow.LoadWorkflowActivity
{
    public class LoadWorkflowActivityRequestHandler : IRequestHandler<LoadWorkflowActivityRequest, OperationResult<LoadWorkflowActivityResponse>>
    {
        private readonly IMultipleChoiceQuestionInvoker _invoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IPipelineAssessmentRepository _pipelineAssessmentRepository;
        private readonly ILoadWorkflowActivityMapper _loadWorkflowActivityMapper;

        public LoadWorkflowActivityRequestHandler(IMultipleChoiceQuestionInvoker invoker, IWorkflowInstanceStore workflowInstanceStore, IPipelineAssessmentRepository pipelineAssessmentRepository, ILoadWorkflowActivityMapper loadWorkflowActivityMapper)
        {
            _invoker = invoker;
            _workflowInstanceStore = workflowInstanceStore;
            _pipelineAssessmentRepository = pipelineAssessmentRepository;
            _loadWorkflowActivityMapper = loadWorkflowActivityMapper;
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
                var workflows = await _invoker.FindWorkflowsAsync(activityRequest.ActivityId, activityRequest.WorkflowInstanceId, cancellationToken);
                var collectedWorkflow = workflows.FirstOrDefault();
                if (collectedWorkflow != null)
                {
                    var workflowInstance = await _workflowInstanceStore.FindByIdAsync(collectedWorkflow.WorkflowInstanceId, cancellationToken: cancellationToken);
                    if (workflowInstance != null)
                    {
                        var dbMultipleChoiceQuestionModel =
                            await _pipelineAssessmentRepository.GetMultipleChoiceQuestions(activityRequest.ActivityId, activityRequest.WorkflowInstanceId, cancellationToken);
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

                                var activityData = _loadWorkflowActivityMapper.ActivityDataDictionaryToActivityData(activityDataDictionary);
                                if (activityData != null)
                                {
                                    result.Data.ActivityData = activityData;
                                    result.Data.PreviousActivityId = dbMultipleChoiceQuestionModel.PreviousActivityId;
                                }
                                else
                                {
                                    result.ErrorMessages.Add("Failed to map activity data");
                                }
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
