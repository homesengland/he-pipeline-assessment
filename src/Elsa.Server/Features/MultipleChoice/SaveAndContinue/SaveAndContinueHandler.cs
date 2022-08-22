using Elsa.CustomActivities.Activities.MultipleChoice;
using Elsa.Persistence;
using Elsa.Server.Data;
using Elsa.Server.Models;
using MediatR;
using Open.Linq.AsyncExtensions;

namespace Elsa.Server.Features.MultipleChoice.SaveAndContinue
{
    public class SaveAndContinueHandler : IRequestHandler<SaveAndContinueCommand, OperationResult<SaveAndContinueResponse>>
    {

        private readonly IMultipleChoiceQuestionInvoker _invoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IPipelineAssessmentRepository _pipelineAssessmentRepository;

        public SaveAndContinueHandler(IMultipleChoiceQuestionInvoker invoker, IWorkflowInstanceStore workflowInstanceStore, IPipelineAssessmentRepository pipelineAssessmentRepository)
        {
            _invoker = invoker;
            _workflowInstanceStore = workflowInstanceStore;
            _pipelineAssessmentRepository = pipelineAssessmentRepository;
        }

        public async Task<OperationResult<SaveAndContinueResponse>> Handle(SaveAndContinueCommand command, CancellationToken cancellationToken)
        {
            var result = new OperationResult<SaveAndContinueResponse>();
            try
            {
                var multipleChoiceQuestionModel = command.ToMultipleChoiceQuestionModel();

                //TODO: compare the model from the db with the dto, if no change, do not execute workflow

                var collectedWorkflows = await _invoker.ExecuteWorkflowsAsync(command.ActivityId,
                    command.WorkflowInstanceId, multipleChoiceQuestionModel, cancellationToken).ToList();
                var collectedWorkflow = collectedWorkflows.FirstOrDefault();
                if (collectedWorkflow != null)
                {
                    var workflowInstance = await _workflowInstanceStore.FindByIdAsync(collectedWorkflow.WorkflowInstanceId, cancellationToken);
                    if (workflowInstance != null)
                    {
                        if (workflowInstance.Output != null)
                        {
                            var nextActivityId = workflowInstance.Output.ActivityId;

                            if (!workflowInstance.ActivityData.ContainsKey(nextActivityId))
                            {
                                result.ErrorMessages.Add(
                                    $"Cannot find activity Id {nextActivityId} in the workflow activity data dictionary");
                            }

                            var nextActivity =
                                workflowInstance.ActivityData.FirstOrDefault(a => a.Key == nextActivityId).Value;

                            await SaveMultipleChoiceResponse(command, nextActivityId);

                            var activityData = nextActivity.ToActivityData2();

                            result.Data = new SaveAndContinueResponse
                            {
                                WorkflowInstanceId = command.WorkflowInstanceId,
                                ActivityData = activityData,
                                ActivityId = nextActivityId
                            };
                        }
                        else
                        {
                            result.ErrorMessages.Add(
                                $"Workflow instance output for workflow instance Id {collectedWorkflow.WorkflowInstanceId} is not set. Unable to determine next activity");
                        }
                    }
                    else
                    {
                        result.ErrorMessages.Add(
                            $"Unable to find workflow instance with Id: {command.WorkflowInstanceId} in Elsa database");
                    }
                }
                else
                {
                    result.ErrorMessages.Add(
                        $"Unable to progress workflow instance Id {command.WorkflowInstanceId}. No collected workflows");
                }
            }
            catch (Exception e)
            {
                result.ErrorMessages.Add(e.Message);
            }

            return await Task.FromResult(result);
        }

        private async Task SaveMultipleChoiceResponse(SaveAndContinueCommand command, string nextActivityId)
        {
            var multipleChoiceQuestion = command.ToMultipleChoiceQuestionModel(nextActivityId);
            await _pipelineAssessmentRepository.SaveMultipleChoiceQuestionAsync(multipleChoiceQuestion);
        }
    }
}
