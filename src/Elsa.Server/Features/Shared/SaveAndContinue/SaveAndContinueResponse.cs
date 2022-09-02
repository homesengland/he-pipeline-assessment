namespace Elsa.Server.Features.Shared.SaveAndContinue
{
    public class SaveAndContinueResponse
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string NextActivityId { get; set; } = null!;
    }
}
