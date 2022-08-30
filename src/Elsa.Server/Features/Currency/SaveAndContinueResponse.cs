namespace Elsa.Server.Features.Currency
{
    public class SaveAndContinueResponse
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string NextActivityId { get; set; } = null!;
    }
}
