namespace Elsa.Server.Features.Shared.LoadWorkflowActivity
{
    public abstract class LoadWorkflowActivityRequest
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
    }
}
