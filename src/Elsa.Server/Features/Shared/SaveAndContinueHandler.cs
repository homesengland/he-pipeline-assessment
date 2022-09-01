using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Features.Shared.SaveAndContinue;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Services;
using Open.Linq.AsyncExtensions;

namespace Elsa.Server.Features.Shared
{
    public interface ISaveAndContinueHandler
    {
        Task<OperationResult<SaveAndContinueResponse>> Handle(MultipleChoiceQuestionModel dbMultipleChoiceQuestionModel, SaveAndContinueCommandBase command, CancellationToken cancellationToken);
    }

    public class SaveAndContinueHandler : ISaveAndContinueHandler
    {
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly IQuestionInvoker _invoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IPipelineAssessmentRepository _pipelineAssessmentRepository;
        private readonly ISaveAndContinueMapper _saveAndContinueMapper;
        private readonly IDateTimeProvider _dateTimeProvider;

        public SaveAndContinueHandler(IQuestionInvoker invoker, IWorkflowInstanceStore workflowInstanceStore, IPipelineAssessmentRepository pipelineAssessmentRepository, ISaveAndContinueMapper saveAndContinueMapper, IDateTimeProvider dateTimeProvider, IWorkflowRegistry workflowRegistry)
        {
            _invoker = invoker;
            _workflowInstanceStore = workflowInstanceStore;
            _pipelineAssessmentRepository = pipelineAssessmentRepository;
            _saveAndContinueMapper = saveAndContinueMapper;
            _dateTimeProvider = dateTimeProvider;
            _workflowRegistry = workflowRegistry;
        }

        public async Task<OperationResult<SaveAndContinueResponse>> Handle(MultipleChoiceQuestionModel dbMultipleChoiceQuestionModel, SaveAndContinueCommandBase command, CancellationToken cancellationToken)
        {
            var result = new OperationResult<SaveAndContinueResponse>();
            try
            {
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
            catch (Exception e)
            {
                result.ErrorMessages.Add(e.Message);
            }

            return await Task.FromResult(result);
        }

        private async Task CreateNextActivityRecord(SaveAndContinueCommandBase command, string nextActivityId, string activityType)
        {
            var multipleChoiceQuestion = _saveAndContinueMapper.SaveAndContinueCommandToNextMultipleChoiceQuestionModel(command, nextActivityId, activityType);
            await _pipelineAssessmentRepository.CreateMultipleChoiceQuestionAsync(multipleChoiceQuestion);
        }
    }
}
