using System.Text.Json.Serialization;

namespace He.PipelineAssessment.Models
{
    public class TargetAssessmentToolWorkflow : AuditableEntity
    {
        public int Id { get; set; }
        public int AssessmentInterventionId { get; set; }
        public int AssessmentToolWorkflowId { get; set; }
        [JsonIgnore]
        public virtual AssessmentToolWorkflow AssessmentToolWorkflow { get; set; } = null!;
        [JsonIgnore]
        public virtual AssessmentIntervention AssessmentIntervention { get; set; } = null!;
    }
}
