﻿using Elsa.Models;

namespace Elsa.Server.Providers
{
    public interface INextWorkflowDefinitionProvider
    {
        string? GetNextWorkflowDefinitionIds(WorkflowInstance workflowInstance, string activityId);
    }

    public class NextWorkflowDefinitionProvider : INextWorkflowDefinitionProvider
    {
        private readonly IActivityDataProvider _activityDataProvider;

        public NextWorkflowDefinitionProvider(IActivityDataProvider activityDataProvider)
        {
            _activityDataProvider = activityDataProvider;
        }

        public string? GetNextWorkflowDefinitionIds(WorkflowInstance workflowInstance, string activityId)
        {
            var activityData = _activityDataProvider.GetActivityData(workflowInstance, activityId);
            var nextWorkflowDefinitionIds = activityData["NextWorkflowDefinitionIds"]?.ToString();

            return nextWorkflowDefinitionIds;
        }
    }
}
