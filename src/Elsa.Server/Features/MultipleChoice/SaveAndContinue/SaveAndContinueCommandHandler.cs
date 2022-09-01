using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Services;
using MediatR;
using Open.Linq.AsyncExtensions;

namespace Elsa.Server.Features.MultipleChoice.SaveAndContinue
{
    public class SaveAndContinueCommandHandler : IRequestHandler<SaveAndContinueCommand, OperationResult<SaveAndContinueResponse>>
    {
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly IQuestionInvoker _invoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IPipelineAssessmentRepository _pipelineAssessmentRepository;
        private readonly ISaveAndContinueMapper _saveAndContinueMapper;
        private readonly IDateTimeProvider _dateTimeProvider;

        public SaveAndContinueCommandHandler(IQuestionInvoker invoker, IWorkflowInstanceStore workflowInstanceStore, IPipelineAssessmentRepository pipelineAssessmentRepository, ISaveAndContinueMapper saveAndContinueMapper, IDateTimeProvider dateTimeProvider, IWorkflowRegistry workflowRegistry)
        {
            _invoker = invoker;
            _workflowInstanceStore = workflowInstanceStore;
            _pipelineAssessmentRepository = pipelineAssessmentRepository;
            _saveAndContinueMapper = saveAndContinueMapper;
            _dateTimeProvider = dateTimeProvider;
            _workflowRegistry = workflowRegistry;
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
                    //TODO: this changes per ActvityType
                    dbMultipleChoiceQuestionModel.SetAnswer(command.Answers, _dateTimeProvider.UtcNow());

                    await _pipelineAssessmentRepository.UpdateMultipleChoiceQuestion(dbMultipleChoiceQuestionModel, cancellationToken);

                    //TODO: compare the model from the db with the dto, if no change, do not execute workflow

                    //TODO: this changes per ActvityType
                    var collectedWorkflow = await _invoker.ExecuteWorkflowsAsync(command.ActivityId, dbMultipleChoiceQuestionModel.ActivityType,
                        command.WorkflowInstanceId, dbMultipleChoiceQuestionModel, cancellationToken).FirstOrDefault();

                    var workflowSpecification =
                        new WorkflowInstanceIdSpecification(collectedWorkflow.WorkflowInstanceId);
                    var workflowInstance = await _workflowInstanceStore.FindAsync(workflowSpecification, cancellationToken);

                    if (workflowInstance != null)
                    {
                        if (workflowInstance.Output != null)
                        {
                            var nextActivityId = workflowInstance.Output.ActivityId;

                            var sampleWorkflow =
                                await _workflowRegistry.FindAsync(workflowInstance.DefinitionId, VersionOptions.Published, cancellationToken: cancellationToken);

                            var activity = sampleWorkflow!.Activities.FirstOrDefault(x =>
                                x.Id == nextActivityId);

                            if (activity != null)
                            {
                                var nextActivityRecord =
                                    await _pipelineAssessmentRepository.GetMultipleChoiceQuestions(nextActivityId,
                                        command.WorkflowInstanceId, cancellationToken);

                                if (nextActivityRecord == null)
                                {
                                    await CreateNextActivityRecord(command, nextActivityId, activity.Type);
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
                                    $"Unable to determine next activity ID");
                            }
                        }
                        else
                        {
                            result.ErrorMessages.Add(
                                $"Workflow instance output for workflow instance Id {command.WorkflowInstanceId} is not set. Unable to determine next activity");
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
                        $"Unable to find workflow instance with Id: {command.WorkflowInstanceId} and Activity Id: {command.ActivityId} in custom database");
                }
            }
            catch (Exception e)
            {
                result.ErrorMessages.Add(e.Message);
            }

            return await Task.FromResult(result);
        }

        private async Task CreateNextActivityRecord(SaveAndContinueCommand command, string nextActivityId, string activityType)
        {
            var multipleChoiceQuestion = _saveAndContinueMapper.SaveAndContinueCommandToNextMultipleChoiceQuestionModel(command, nextActivityId, activityType);
            await _pipelineAssessmentRepository.CreateMultipleChoiceQuestionAsync(multipleChoiceQuestion);
        }
    }
}
