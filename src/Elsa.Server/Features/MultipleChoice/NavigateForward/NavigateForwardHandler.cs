using Elsa.CustomActivities.Activities.MultipleChoice;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Server.Data;
using MediatR;
using Open.Linq.AsyncExtensions;

namespace Elsa.Server.Features.MultipleChoice.NavigateForward
{
    public class NavigateForwardHandler : IRequestHandler<NavigateForwardCommand, NavigateForwardResponse>
    {

        private readonly IMultipleChoiceQuestionInvoker _invoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IPipelineAssessmentRepository _pipelineAssessmentRepository;

        public NavigateForwardHandler(IMultipleChoiceQuestionInvoker invoker, IWorkflowInstanceStore workflowInstanceStore, IPipelineAssessmentRepository pipelineAssessmentRepository)
        {
            _invoker = invoker;
            _workflowInstanceStore = workflowInstanceStore;
            _pipelineAssessmentRepository = pipelineAssessmentRepository;
        }

        public async Task<NavigateForwardResponse> Handle(NavigateForwardCommand command, CancellationToken cancellationToken)
        {
            try
            {
                string nextActivityId = "";

                WorkflowInstance? workflowInstance = null;

                var multipleChoiceQuestionModel = command.ToMultipleChoiceQuestionModel(nextActivityId);

                //TODO: compare the model from the db with the dto, if no change, do not execute workflow

                var collectedWorkflows = await _invoker.ExecuteWorkflowsAsync(command.ActivityId,
                    command.WorkflowInstanceId, multipleChoiceQuestionModel).ToList();
                var collectedWorkflow = collectedWorkflows.FirstOrDefault();
                if (collectedWorkflow != null)
                {
                    workflowInstance =
                        await _workflowInstanceStore.FindByIdAsync(collectedWorkflow.WorkflowInstanceId);
                    if (workflowInstance != null)
                    {
                        if (workflowInstance.Output != null)
                        {
                            nextActivityId = workflowInstance.Output.ActivityId;
                        }
                        else
                        {
                            return await Task.FromResult(new NavigateForwardResponse
                            {
                                Result = new Result { IsSuccess = false, ErrorMessage = $"Unable to find workflow instance with Id: {command.WorkflowInstanceId} in Elsa database" },
                            });
                        }
                    }
                    else
                    {
                        return await Task.FromResult(new NavigateForwardResponse
                        {
                            Result = new Result { IsSuccess = false, ErrorMessage = $"Unable to find workflow instance with Id: {command.WorkflowInstanceId} in Elsa database" },
                        });
                    }
                }
                else
                {
                    return await Task.FromResult(new NavigateForwardResponse
                    {
                        Result = new Result { IsSuccess = false, ErrorMessage = $"Unable to progress workflow. Elsa Execute failed" },
                    });
                }

                if (workflowInstance != null)
                {
                    if (!workflowInstance.ActivityData.ContainsKey(nextActivityId))
                    {
                        return await Task.FromResult(new NavigateForwardResponse
                        {
                            Result = new Result { IsSuccess = false, ErrorMessage = $"Cannot find activity Id {nextActivityId} in the workflow activity data dictionary" },
                        });
                    }

                    var nextActivity =
                        workflowInstance.ActivityData.FirstOrDefault(a => a.Key == nextActivityId).Value;

                    await SaveMultipleChoiceResponse(command, nextActivityId);

                    var activityData = nextActivity.ToActivityData2();

                    return await Task.FromResult(new NavigateForwardResponse
                    {
                        Result = new Result { IsSuccess = true },
                        WorkflowInstanceId = command.WorkflowInstanceId,
                        ActivityData = activityData,
                        ActivityId = nextActivityId
                    });
                }

                return await Task.FromResult(new NavigateForwardResponse
                {
                    Result = new Result { IsSuccess = false, ErrorMessage = $"Unable to find workflow instance with Id: {command.WorkflowInstanceId} in Elsa database" },
                });

            }
            catch (Exception e)
            {
                return await Task.FromResult(new NavigateForwardResponse
                {
                    Result = new Result { IsSuccess = false, ErrorMessage = e.Message },
                });
            }
        }

        private async Task SaveMultipleChoiceResponse(NavigateForwardCommand command, string nextActivityId)
        {
            var multipleChoiceQuestion = command.ToMultipleChoiceQuestionModel(nextActivityId);
            await _pipelineAssessmentRepository.SaveMultipleChoiceQuestionAsync(multipleChoiceQuestion);
        }
    }
}
