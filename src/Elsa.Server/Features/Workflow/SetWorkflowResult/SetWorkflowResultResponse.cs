namespace Elsa.Server.Features.Workflow.SetWorkflowResult
{
    public class SetWorkflowResultResponse
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string NextActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
    }
}
