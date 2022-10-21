using Elsa.Client.Models;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Persistence;
using Elsa.Persistence.Specifications.WorkflowInstances;
using Elsa.Server.Models;
using Elsa.Services;
using Elsa.Services.Models;
using MediatR;
using System.Threading;
using VersionOptions = Elsa.Models.VersionOptions;
using WorkflowInstance = Elsa.Models.WorkflowInstance;

namespace Elsa.Server.Features.Workflow.StartWorkflow
{
    public class StartWorkflowCommandHandler : IRequestHandler<StartWorkflowCommand, OperationResult<StartWorkflowResponse>>
    {
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly IStartsWorkflow _startsWorkflow;
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IStartWorkflowMapper _startWorkflowMapper;
        private readonly  IWorkflowInstanceStore _workflowInstanceStore;

        public StartWorkflowCommandHandler(IWorkflowRegistry workflowRegistry, IStartsWorkflow startsWorkflow, IElsaCustomRepository elsaCustomRepository, IStartWorkflowMapper startWorkflowMapper, IWorkflowInstanceStore workflowInstanceStore)
        {
            _workflowRegistry = workflowRegistry;
            _startsWorkflow = startsWorkflow;
            _elsaCustomRepository = elsaCustomRepository;
            _startWorkflowMapper = startWorkflowMapper;
            _workflowInstanceStore = workflowInstanceStore;
        }

        public async Task<OperationResult<StartWorkflowResponse>> Handle(StartWorkflowCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<StartWorkflowResponse>();
            try
            {
                var workflow =
                    await _workflowRegistry.FindAsync(request.WorkflowDefinitionId, VersionOptions.Published, cancellationToken: cancellationToken);
                var runWorkflowResult = await _startsWorkflow.StartWorkflowAsync(workflow!, cancellationToken: cancellationToken);

                if (runWorkflowResult.WorkflowInstance != null)
                {
                    var activity = workflow!.Activities.FirstOrDefault(x =>
                        x.Id == runWorkflowResult.WorkflowInstance.LastExecutedActivityId);

                    if (activity != null)
                    {
                        var workflowInstanceId = runWorkflowResult.WorkflowInstance.Id;
                        var workflowInstance = runWorkflowResult.WorkflowInstance;
                        var nextActivityId = workflowInstance.Output.ActivityId;

                        var lastExecutedActivity =
                            workflowInstance.ActivityData[workflowInstance.LastExecutedActivityId];

                        if (lastExecutedActivity.HasKey("ChildWorkflowInstanceId"))
                        {
                            workflowInstanceId = lastExecutedActivity["ChildWorkflowInstanceId"]!.ToString();
                            var workflowSpecification =
                                new WorkflowInstanceIdSpecification(workflowInstanceId);

                            workflowInstance = await _workflowInstanceStore.FindAsync(workflowSpecification, cancellationToken);
                            nextActivityId = workflowInstance.LastExecutedActivityId;
                            workflow =
                                await _workflowRegistry.FindAsync(workflowInstance.DefinitionId, VersionOptions.Published, cancellationToken: cancellationToken);
                        }

                        var nextActivity = workflow!.Activities.FirstOrDefault(x =>
                            x.Id == nextActivityId);
                        //var assessmentQuestion =
                        //    _startWorkflowMapper.RunWorkflowResultToAssessmentQuestion(runWorkflowResult, activity, workflow.Name);

                        //if (assessmentQuestion != null)
                        //{
                        await CreateNextActivityRecord(workflowInstance, nextActivity, workflow.Name);
                        result.Data = _startWorkflowMapper.RunWorkflowResultToStartWorkflowResponse(workflowInstanceId, nextActivityId);
                        //}
                        //else
                        //{
                        //    result.ErrorMessages.Add("Failed to deserialize RunWorkflowResult");
                        //}
                    }
                    else
                    {
                        result.ErrorMessages.Add("Failed to get activity");
                    }
                }

            }
            catch (Exception e)
            {
                result.ErrorMessages.Add(e.Message);
            }

            return await Task.FromResult(result);
        }

        private async Task CreateNextActivityRecord(WorkflowInstance workflowInstance, IActivityBlueprint nextActivity, string? workflowName)
        {
            var assessmentQuestion =
                _startWorkflowMapper.RunWorkflowResultToAssessmentQuestion(workflowInstance, nextActivity, workflowName);
            await _elsaCustomRepository.CreateAssessmentQuestionAsync(assessmentQuestion!);
        }
    }
}