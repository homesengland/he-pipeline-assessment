using Elsa.CustomActivities.Activities.MultipleChoice;
using Elsa.Persistence;
using Elsa.Server.Data;
using Elsa.Server.Models;
using MediatR;

namespace Elsa.Server.Features.Workflow.LoadWorkflow
{
    public class LoadWorkflowRequestHandler : IRequestHandler<LoadWorkflowRequest, OperationResult<LoadWorkflowResponse>>
    {
        private readonly IMultipleChoiceQuestionInvoker _invoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IPipelineAssessmentRepository _pipelineAssessmentRepository;

        public LoadWorkflowRequestHandler(IMultipleChoiceQuestionInvoker invoker, IWorkflowInstanceStore workflowInstanceStore, IPipelineAssessmentRepository pipelineAssessmentRepository)
        {
            _invoker = invoker;
            _workflowInstanceStore = workflowInstanceStore;
            _pipelineAssessmentRepository = pipelineAssessmentRepository;
        }

        public async Task<OperationResult<LoadWorkflowResponse>> Handle(LoadWorkflowRequest request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<LoadWorkflowResponse>
            {
                Data = new LoadWorkflowResponse
                {
                    WorkflowInstanceId = request.WorkflowInstanceId,
                    ActivityId = request.ActivityId
                }
            };
            try
            {
                string nextActivityId = "";

                var workflows = await _invoker.FindWorkflowsAsync(request.ActivityId, request.WorkflowInstanceId, cancellationToken);
                var collectedWorkflow = workflows.FirstOrDefault();
                if (collectedWorkflow != null)
                {
                    var workflowInstance = await _workflowInstanceStore.FindByIdAsync(collectedWorkflow.WorkflowInstanceId, cancellationToken: cancellationToken);
                    if (workflowInstance != null)
                    {
                        var dbMultipleChoiceQuestionModel =
                            await _pipelineAssessmentRepository.GetMultipleChoiceQuestions(request.ActivityId, request.WorkflowInstanceId, cancellationToken);
                        if (dbMultipleChoiceQuestionModel != null)
                        {
                            var previousBlockingActivity = workflowInstance.BlockingActivities
                                .FirstOrDefault(y =>
                                    y.ActivityId == dbMultipleChoiceQuestionModel.PreviousActivityId);
                            if (previousBlockingActivity != null)
                            {
                                nextActivityId = previousBlockingActivity.ActivityId;
                                if (!workflowInstance.ActivityData.ContainsKey(nextActivityId))
                                {
                                    result.ErrorMessages.Add(
                                        $"Cannot find activity Id {nextActivityId} in the workflow activity data dictionary");
                                }
                                else
                                {
                                    var nextActivity =
                                        workflowInstance.ActivityData.FirstOrDefault(a => a.Key == nextActivityId).Value;
                                    var activityData = nextActivity.ToActivityData();
                                    if (activityData != null)
                                    {
                                        result.Data.ActivityData = activityData;
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
                                    $"Unable to find blocking activity with Id: {dbMultipleChoiceQuestionModel.PreviousActivityId}");
                            }
                        }
                        else
                        {
                            result.ErrorMessages.Add(
                                $"Unable to find workflow instance with Id: {request.WorkflowInstanceId} and Activity Id: {request.ActivityId}");
                        }
                    }
                    else
                    {
                        result.ErrorMessages.Add(
                            $"Unable to find workflow instance with Id: {request.WorkflowInstanceId} in Elsa database");
                    }
                }
                else
                {
                    result.ErrorMessages.Add(
                        $"Unable to progress workflow instance Id {request.WorkflowInstanceId}. No collected workflows");
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
