namespace He.PipelineAssessment.Models.ViewModels
{
    public class StartableToolViewModel
    {
        public int? AssessmentToolWorkflowId { get; set; }
        public int AssessmentToolId { get; set; }
        public string WorkflowDefinitionId { get; set; } = null!;
        public bool IsFirstWorkflow { get; set; }
        public bool? IsVariation { get; set; }
    }
}
