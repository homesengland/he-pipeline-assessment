﻿using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Services;
using MediatR;
using Open.Linq.AsyncExtensions;

namespace Elsa.Server.Features.Workflow.SaveAndContinue
{
    public class SaveAndContinueCommandHandler : IRequestHandler<SaveAndContinueCommand, OperationResult<SaveAndContinueResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IQuestionInvoker _invoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly ISaveAndContinueMapper _saveAndContinueMapper;


        public SaveAndContinueCommandHandler(IQuestionInvoker invoker, IWorkflowInstanceStore workflowInstanceStore, IElsaCustomRepository elsaCustomRepository, IDateTimeProvider dateTimeProvider, IWorkflowRegistry workflowRegistry, ISaveAndContinueMapper saveAndContinueMapper)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _dateTimeProvider = dateTimeProvider;
            _invoker = invoker;
            _workflowInstanceStore = workflowInstanceStore;
            _workflowRegistry = workflowRegistry;
            _saveAndContinueMapper = saveAndContinueMapper;
        }

        public async Task<OperationResult<SaveAndContinueResponse>> Handle(SaveAndContinueCommand command, CancellationToken cancellationToken)
        {
            var result = new OperationResult<SaveAndContinueResponse>();
            try
            {
                var dbAssessmentQuestion =
                    await _elsaCustomRepository.GetAssessmentQuestion(command.ActivityId, command.WorkflowInstanceId, cancellationToken);
                if (dbAssessmentQuestion != null)
                {
                    dbAssessmentQuestion.Comments = command.Comments;

                    dbAssessmentQuestion.SetAnswer(command.Answer, _dateTimeProvider.UtcNow()); //use DateTimeProvider
                    await _elsaCustomRepository.UpdateAssessmentQuestion(dbAssessmentQuestion,
                        cancellationToken);

                    var collectedWorkflow = await _invoker.ExecuteWorkflowsAsync(command.ActivityId, dbAssessmentQuestion.ActivityType,
                    command.WorkflowInstanceId, dbAssessmentQuestion, cancellationToken).FirstOrDefault();

                    var workflowSpecification =
                        new WorkflowInstanceIdSpecification(collectedWorkflow.WorkflowInstanceId);
                    var workflowInstance = await _workflowInstanceStore.FindAsync(workflowSpecification, cancellationToken);

                    if (workflowInstance != null)
                    {
                        if (workflowInstance.Output != null)
                        {
                            var nextActivityId = workflowInstance.Output.ActivityId;

                            var workflow =
                                await _workflowRegistry.FindAsync(workflowInstance.DefinitionId, VersionOptions.Published, cancellationToken: cancellationToken);

                            var nextActivity = workflow!.Activities.FirstOrDefault(x =>
                                x.Id == nextActivityId);

                            if (nextActivity != null)
                            {
                                var nextActivityRecord =
                                    await _elsaCustomRepository.GetAssessmentQuestion(nextActivityId,
                                        command.WorkflowInstanceId, cancellationToken);

                                if (nextActivityRecord == null)
                                {
                                    await CreateNextActivityRecord(command, nextActivityId, nextActivity.Type);
                                }

                                result.Data = new SaveAndContinueResponse
                                {
                                    WorkflowInstanceId = command.WorkflowInstanceId,
                                    NextActivityId = nextActivityId,
                                    ActivityType = nextActivity.Type
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
            var assessmentQuestion = _saveAndContinueMapper.SaveAndContinueCommandToNextAssessmentQuestion(command, nextActivityId, activityType);
            await _elsaCustomRepository.CreateAssessmentQuestionAsync(assessmentQuestion);
        }
    }
}
