namespace Elsa.CustomWorkflow.Sdk.Models.Workflow
{
    public class LoadWorkflowActivityDto
    {
        public string ActivityId { get; set; } = null!;

        public string WorkflowInstanceId { get; set; } = null!;

        public string ActivityType { get; set; } = null!;

    }
}
