﻿using Elsa.Models;
using Elsa.Services;
using Elsa.Services.Models;

namespace Elsa.Server.Providers
{
    public class WorkflowNextActivityProvider : IWorkflowNextActivityProvider
    {
        private readonly IWorkflowRegistry _workflowRegistry;
        public WorkflowNextActivityProvider(IWorkflowRegistry workflowRegistry)
        {
            _workflowRegistry = workflowRegistry;
        }

        public async Task<IActivityBlueprint> GetNextActivity(WorkflowInstance workflowInstance, CancellationToken cancellationToken)
        {
            if (workflowInstance.Output == null)
            {
                throw new Exception($"No output found for workflow instance id {workflowInstance.Id}");
            }

            var nextActivityId = workflowInstance.Output.ActivityId;//workflowInstance.LastExecutedActivityId;

            var workflow =
                await _workflowRegistry.FindAsync(workflowInstance.DefinitionId, VersionOptions.Published,
                    cancellationToken: cancellationToken);

            var nextActivity = workflow!.Activities.FirstOrDefault(x =>
                x.Id == nextActivityId);

            if (nextActivity == null)
            {
                throw new Exception($"Next activity not found for workflow instance id {workflowInstance.Id}.");
            }

            return nextActivity;
        }
    }
}

