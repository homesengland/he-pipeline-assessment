namespace Elsa.Server.Features.Workflow.SaveAndContinue
{
    public class SaveAndContinueResponse
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string NextActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
    }
}
