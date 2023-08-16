namespace Elsa.CustomWorkflow.Sdk.Models.Workflow
{
    public class StartWorkflowCommandDto
    {
        public string CorrelationId { get; set; } = null!;
        public string WorkflowDefinitionId { get; set; } = null!;
    }
}
