namespace He.PipelineAssessment.Models
{
    public class AssessmentToolInstanceNextWorkflow : AuditableEntity
    {
        public int Id { get; set; }
        public int AssessmentToolWorkflowInstanceId { get; set; }
        public string NextWorkflowDefinitionId { get; set; } = null!;
        public bool IsStarted { get; set; }
        public int AssessmentId { get; set; }


        public virtual AssessmentToolWorkflowInstance AssessmentToolWorkflowInstance { get; set; } = null!;
    }
}
