namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentToolWorkflows
{
    public class AssessmentToolWorkflowDto
    {
        public int Id { get; set; }
        public int AssessmentToolId { get; set; }
        public string AssessmentToolName { get; set; }
        public string WorkflowDefinitionId { get; set; }
        public bool IsFirstWorkflow { get; set; }
        public int Version { get; set; }
        public bool IsLatest { get; set; }
    }
}