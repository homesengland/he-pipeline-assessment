namespace Elsa.Server.Features.Workflow.MultiSaveAndContinue
{
    public class MultiSaveAndContinueResponse
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string NextActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
    }
}
