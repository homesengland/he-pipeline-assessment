﻿using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.Models;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Elsa.Server.Services;
using Elsa.Services;
using MediatR;


namespace Elsa.Server.Features.Workflow.StartWorkflow
{
    public class StartWorkflowCommandHandler : IRequestHandler<StartWorkflowCommand, OperationResult<StartWorkflowResponse>>
    {
        private readonly IWorkflowRegistry _workflowRegistry;
        private readonly IStartsWorkflow _startsWorkflow;
        private readonly IElsaCustomRepository _elsaCustomRepository;
        private readonly IStartWorkflowMapper _startWorkflowMapper;
        private readonly IWorkflowNextActivityProvider _workflowNextActivityProvider;
        private readonly INextActivityNavigationService _nextActivityNavigationService;


        public StartWorkflowCommandHandler(IWorkflowRegistry workflowRegistry, IStartsWorkflow startsWorkflow,
                                           IElsaCustomRepository elsaCustomRepository, IStartWorkflowMapper startWorkflowMapper,
                                           IWorkflowNextActivityProvider workflowNextActivityProvider,
                                           INextActivityNavigationService nextActivityNavigationService)
        {
            _workflowRegistry = workflowRegistry;
            _startsWorkflow = startsWorkflow;
            _elsaCustomRepository = elsaCustomRepository;
            _startWorkflowMapper = startWorkflowMapper;
            _workflowNextActivityProvider = workflowNextActivityProvider;
            _nextActivityNavigationService = nextActivityNavigationService;
        }

        public async Task<OperationResult<StartWorkflowResponse>> Handle(StartWorkflowCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<StartWorkflowResponse>();
            try
            {
                var workflow =
                    await _workflowRegistry.FindAsync(request.WorkflowDefinitionId, VersionOptions.Published, cancellationToken: cancellationToken);

                var workflowName = workflow!.Name != null ? workflow.Name : "undefined workflow";
                var runWorkflowResult = await _startsWorkflow.StartWorkflowAsync(workflow!, null, null, request.CorrelationId, cancellationToken: cancellationToken);

                if (runWorkflowResult.WorkflowInstance != null)
                {
                    var workflowInstance = runWorkflowResult.WorkflowInstance;

                    var activity = workflow!.Activities.FirstOrDefault(x =>
                        x.Id == runWorkflowResult.WorkflowInstance.LastExecutedActivityId);

                    if (activity != null)
                    {
                        var workflowNextActivityModel = await _workflowNextActivityProvider.GetStartWorkflowNextActivity(activity, runWorkflowResult.WorkflowInstance.Id, cancellationToken);

                        if (workflowNextActivityModel.WorkflowInstance != null)
                            workflowInstance = workflowNextActivityModel.WorkflowInstance;

                        var nextActivityRecord = await _elsaCustomRepository.GetCustomActivityNavigation(workflowNextActivityModel.NextActivity.Id, workflowInstance.Id, cancellationToken);

                        await _nextActivityNavigationService.CreateNextActivityNavigation(activity.Id, nextActivityRecord, workflowNextActivityModel.NextActivity, workflowInstance, cancellationToken);

                        result.Data = new StartWorkflowResponse
                        {
                            WorkflowInstanceId = workflowInstance.Id,
                            NextActivityId = workflowNextActivityModel.NextActivity.Id,
                            ActivityType = workflowNextActivityModel.NextActivity.Type,
                            WorkflowName = workflowName
                        };
                    }
                    else
                    {
                        result.ErrorMessages.Add("Failed to get activity");
                    }
                }
                else
                {
                    result.ErrorMessages.Add("Workflow instance is null");
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