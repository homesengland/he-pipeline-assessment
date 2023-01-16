namespace He.PipelineAssessment.Models.ViewModels
{
    public class StartableToolViewModel
    {
        public int AssessmentToolId { get; set; }
        public string WorkflowDefinitionId { get; set; } = null!;
        public bool IsFirstWorkflow { get; set; }
    }
}
