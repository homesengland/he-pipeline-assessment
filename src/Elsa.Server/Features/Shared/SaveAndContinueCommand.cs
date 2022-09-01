namespace Elsa.Server.Features.Shared
{
    public abstract class SaveAndContinueCommand
    {
        public string Id { get; set; } = null!;
        public string ActivityId { get; set; } = null!;

        public string WorkflowInstanceId { get; set; } = null!;
    }
}
