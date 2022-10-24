using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Services;
using Elsa.Services.Models;
using MediatR;
using Open.Linq.AsyncExtensions;

namespace Elsa.Server.Features.Workflow.SubmitAssessmentStage
{
    public class SubmitAssessmentStageCommandHandler : IRequestHandler<SubmitAssessmentStageCommand, OperationResult<SubmitAssessmentStageResponse>>
    {
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IQuestionInvoker _invoker;
        private readonly IWorkflowInstanceStore _workflowInstanceStore;
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly ISubmitAssessmentStageCommandMapper _submitAssessmentStageCommandMapper;

        public SubmitAssessmentStageCommandHandler(IQuestionInvoker invoker, IWorkflowInstanceStore workflowInstanceStore, IElsaCustomRepository elsaCustomRepository, IDateTimeProvider dateTimeProvider, IWorkflowRegistry workflowRegistry, ISubmitAssessmentStageCommandMapper submitAssessmentStageCommandMapper)
        {
            _elsaCustomRepository = elsaCustomRepository;
            _dateTimeProvider = dateTimeProvider;
            _invoker = invoker;
            _workflowInstanceStore = workflowInstanceStore;
            _workflowRegistry = workflowRegistry;
            _submitAssessmentStageCommandMapper = submitAssessmentStageCommandMapper;
        }

        public async Task<OperationResult<SubmitAssessmentStageResponse>> Handle(SubmitAssessmentStageCommand command, CancellationToken cancellationToken)
        {
            var result = new OperationResult<SubmitAssessmentStageResponse>();
            try
            {
                var dbAssessmentQuestion =
                    await _elsaCustomRepository.GetAssessmentQuestion(command.ActivityId, command.WorkflowInstanceId, cancellationToken);
                if (dbAssessmentQuestion != null)
                {
                    dbAssessmentQuestion.SetAnswer("true", _dateTimeProvider.UtcNow()); //use DateTimeProvider
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
                            var workflowDefinitionId = workflowInstance.DefinitionId;
                            var workflowInstanceId = command.WorkflowInstanceId;

                            if (workflowInstance.LastExecutedActivityId != null)
                            {
                                var lastExecutedActivity =
                                    workflowInstance.ActivityData[workflowInstance.LastExecutedActivityId];
                                if (lastExecutedActivity.HasKey("ChildWorkflowInstanceId"))
                                {
                                    workflowInstanceId = lastExecutedActivity["ChildWorkflowInstanceId"]!.ToString();
                                }

                                workflowSpecification =
                                    new WorkflowInstanceIdSpecification(workflowInstanceId);

                                workflowInstance = await _workflowInstanceStore.FindAsync(workflowSpecification, cancellationToken);
                                nextActivityId = workflowInstance.LastExecutedActivityId;
                                workflowDefinitionId = workflowInstance.DefinitionId;
                            }

                            var workflow =
                                await _workflowRegistry.FindAsync(workflowDefinitionId, VersionOptions.Published, cancellationToken: cancellationToken);

                            var nextActivity = workflow!.Activities.FirstOrDefault(x =>
                                x.Id == nextActivityId);

                            if (nextActivity != null)
                            {
                                var nextActivityRecord =
                                    await _elsaCustomRepository.GetAssessmentQuestion(nextActivityId,
                                        workflowInstanceId, cancellationToken);

                                if (nextActivityRecord == null)
                                {
                                    await CreateNextActivityRecord(command.ActivityId, workflowInstance, nextActivity, workflow.Name);
                                }

                                result.Data = new SubmitAssessmentStageResponse()
                                {
                                    WorkflowInstanceId = workflowInstanceId,
                                    NextActivityId = nextActivityId,
                                    ActivityType = nextActivity.Type
                                };

                                workflowInstance.WorkflowStatus = WorkflowStatus.Finished;
                                await _workflowInstanceStore.SaveAsync(workflowInstance, cancellationToken);
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

        private async Task CreateNextActivityRecord(string activityId, WorkflowInstance workflowInstance,
            IActivityBlueprint activity, string? workflowName)
        {
            var assessmentQuestion =
                _submitAssessmentStageCommandMapper.SubmitAssessmentStageCommandToNextAssessmentQuestion(activityId, workflowInstance,
                    activity, workflowName);
            await _elsaCustomRepository.CreateAssessmentQuestionAsync(assessmentQuestion);
        }
    }
}
