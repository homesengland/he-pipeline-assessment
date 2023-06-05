namespace He.PipelineAssessment.Models
{
    public class AssessmentIntervention : AuditableEntity
    {
        public int Id { get; set; }
        public int AssessmentToolWorkflowInstanceId { get; set; }
        public int? TargetAssessmentToolWorkflowId { get; set; }
        public string RequestedBy { get; set; } = null!;
        public string RequestedByEmail { get; set; } = null!; 
        public string? Administrator { get; set; }
        public string? AdministratorEmail { get; set; }
        public string? SignOffDocument { get; set; }
        public string DecisionType { get; set; } = null!;
        public string? AssessorRationale { get; set; }
        public string? AdministratorRationale { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string Status { get; set; } = null!;

        public virtual AssessmentToolWorkflow? TargetAssessmentToolWorkflow { get; set; } = null!;

        public virtual AssessmentToolWorkflowInstance AssessmentToolWorkflowInstance { get; set; } = null!;
        public string? AssessmentResult { get; set; }
    }
}
