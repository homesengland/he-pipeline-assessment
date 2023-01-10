namespace He.PipelineAssessment.Models
{
    public class AssessmentToolWorkflowInstance
    {
        public int Id { get; set; }
        public int AssessmentId { get; set; }
        public string WorkflowName { get; set; } = null!;
        public string WorkflowDefinitionId { get; set; } = null!;
        public string WorkflowInstanceId { get; set; } = null!;
        public string CurrentActivityId { get; set; } = null!;
        public string CurrentActivityType { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime CreatedDateTime { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
        public DateTime? SubmittedDateTime { get; set; }
        public virtual Assessment Assessment { get; set; } = null!;
        public virtual List<AssessmentToolInstanceNextWorkflow> AssessmentToolInstanceNextWorkflows { get; set; } = null!;
    }
}
