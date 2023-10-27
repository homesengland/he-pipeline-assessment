namespace He.PipelineAssessment.UI.Features.Workflow.ViewModels
{
    public class InterventionOptions
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public bool IsVariationAllowed { get; set; }
        public bool IsAmendableWorkflow { get; set; }
    }
}
