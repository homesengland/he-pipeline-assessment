﻿using Elsa.CustomActivities.Activities.MultipleChoice;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using MediatR;
using Open.Linq.AsyncExtensions;

namespace Elsa.Server.Features.MultipleChoice.SaveAndContinue
{
    public class SaveAndContinueCommandHandler : IRequestHandler<SaveAndContinueCommand, OperationResult<SaveAndContinueResponse>>
    {

        private readonly IMultipleChoiceQuestionInvoker _invoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IPipelineAssessmentRepository _pipelineAssessmentRepository;
        private readonly ISaveAndContinueMapper _saveAndContinueMapper;
        private readonly IDateTimeProvider _dateTimeProvider;

        public SaveAndContinueCommandHandler(IMultipleChoiceQuestionInvoker invoker, IWorkflowInstanceStore workflowInstanceStore, IPipelineAssessmentRepository pipelineAssessmentRepository, ISaveAndContinueMapper saveAndContinueMapper, IDateTimeProvider dateTimeProvider)
        {
            _invoker = invoker;
            _workflowInstanceStore = workflowInstanceStore;
            _pipelineAssessmentRepository = pipelineAssessmentRepository;
            _saveAndContinueMapper = saveAndContinueMapper;
            _dateTimeProvider = dateTimeProvider;
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
                    dbMultipleChoiceQuestionModel.SetAnswer(command.Answers, _dateTimeProvider.UtcNow());
                    await _pipelineAssessmentRepository.UpdateMultipleChoiceQuestion(dbMultipleChoiceQuestionModel, cancellationToken);

                    //TODO: compare the model from the db with the dto, if no change, do not execute workflow

                    var collectedWorkflow = await _invoker.ExecuteWorkflowsAsync(command.ActivityId,
                        command.WorkflowInstanceId, dbMultipleChoiceQuestionModel, cancellationToken).FirstOrDefault();

                    var workflowSpecification =
                        new WorkflowInstanceIdSpecification(collectedWorkflow.WorkflowInstanceId);
                    var workflowInstance = await _workflowInstanceStore.FindAsync(workflowSpecification, cancellationToken);

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

        private async Task CreateNextActivityRecord(SaveAndContinueCommand command, string nextActivityId)
        {
            var multipleChoiceQuestion = _saveAndContinueMapper.SaveAndContinueCommandToNextMultipleChoiceQuestionModel(command, nextActivityId);
            await _pipelineAssessmentRepository.CreateMultipleChoiceQuestionAsync(multipleChoiceQuestion);
        }
    }
}
