using Elsa.CustomActivities.Activities.MultipleChoice;
using Elsa.CustomInfrastructure.Data;
using Elsa.Persistence;
using Elsa.Server.Models;
using MediatR;
using Open.Linq.AsyncExtensions;

namespace Elsa.Server.Features.MultipleChoice.SaveAndContinue
{
    public class SaveAndContinueCommandHandler : IRequestHandler<SaveAndContinueCommand, OperationResult<SaveAndContinueResponse>>
    {

        private readonly IMultipleChoiceQuestionInvoker _invoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IPipelineAssessmentRepository _pipelineAssessmentRepository;

        public SaveAndContinueCommandHandler(IMultipleChoiceQuestionInvoker invoker, IWorkflowInstanceStore workflowInstanceStore, IPipelineAssessmentRepository pipelineAssessmentRepository)
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
                var dbMultipleChoiceQuestionModel =
                    await _pipelineAssessmentRepository.GetMultipleChoiceQuestions(command.ActivityId, command.WorkflowInstanceId, cancellationToken);
                if (dbMultipleChoiceQuestionModel != null)
                {
                    dbMultipleChoiceQuestionModel.SetAnswer(command.Answer);
                    await _pipelineAssessmentRepository.UpdateMultipleChoiceQuestion(dbMultipleChoiceQuestionModel, cancellationToken);

                    //TODO: compare the model from the db with the dto, if no change, do not execute workflow

                    var collectedWorkflows = await _invoker.ExecuteWorkflowsAsync(command.ActivityId,
                        command.WorkflowInstanceId, dbMultipleChoiceQuestionModel, cancellationToken).ToList();

                    var collectedWorkflow = collectedWorkflows.FirstOrDefault();

                    if (collectedWorkflow != null)
                    {
                        var workflowInstance = await _workflowInstanceStore.FindByIdAsync(collectedWorkflow.WorkflowInstanceId, cancellationToken);

                        if (workflowInstance != null)
                        {
                            if (workflowInstance.Output != null)
                            {
                                var nextActivityId = workflowInstance.Output.ActivityId;

                                var nextActivityRecord =
                                    await _pipelineAssessmentRepository.GetMultipleChoiceQuestions(nextActivityId,
                                        command.WorkflowInstanceId, cancellationToken);

                                if (nextActivityRecord == null)
                                {
                                    await CreateNextActivityRecord(command, nextActivityId);
                                }

                                result.Data = new SaveAndContinueResponse
                                {
                                    WorkflowInstanceId = command.WorkflowInstanceId,
                                    NextActivityId = nextActivityId
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
                else
                {
                    result.ErrorMessages.Add(
                        $"Unable to find workflow instance with Id: {command.WorkflowInstanceId} and Activity Id: {command.ActivityId} in custom database");
                }


            }
            catch (Exception e)
            {
                result.ErrorMessages.Add(e.Message);
            }

            return await Task.FromResult(result);
        }

        private async Task CreateNextActivityRecord(SaveAndContinueCommand command, string nextActivityId)
        {
            var multipleChoiceQuestion = command.ToMultipleChoiceQuestionModel(nextActivityId);
            multipleChoiceQuestion.Answer = null;
            multipleChoiceQuestion.CreatedDateTime = DateTime.UtcNow;
            await _pipelineAssessmentRepository.CreateMultipleChoiceQuestionAsync(multipleChoiceQuestion);
        }
    }
}
