namespace Elsa.Server.Features.Workflow.StartWorkflow
{
    public class StartWorkflowResponse
    { 
        public string WorkflowInstanceId { get; set; } = null!;
        public string NextActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
    }
}
