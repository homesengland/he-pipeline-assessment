namespace He.PipelineAssessment.Models.ViewModels
{
    //this maps to the result of stored procedure GetAssessmentStagesByAssessmentId
    public class AssessmentStageViewModel : AuditableEntityViewModel
    {
        public string Name { get; set; } = null!;
        public bool IsVisible { get; set; }
        public int Order { get; set; }
        public string? WorkflowName { get; set; }
        public string? WorkflowDefinitionId { get; set; }
        public string? WorkflowInstanceId { get; set; }
        public string? CurrentActivityId { get; set; }
        public string? CurrentActivityType { get; set; }
        public string? Status { get; set; }
        public DateTime? SubmittedDateTime { get; set; }
        public int? AssessmentToolId { get; set; }
        public int? AssessmentToolWorkflowInstanceId { get; set; }
        public bool? IsFirstWorkflow { get; set; }
        public string? Result { get; set; } = null;
        public string? SubmittedBy { get; set; } = null;
    }
}
