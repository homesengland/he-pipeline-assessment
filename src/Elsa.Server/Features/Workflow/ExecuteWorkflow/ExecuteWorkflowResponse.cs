﻿namespace Elsa.Server.Features.Workflow.ExecuteWorkflow
{
    public class ExecuteWorkflowResponse
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string NextActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
    }
}
