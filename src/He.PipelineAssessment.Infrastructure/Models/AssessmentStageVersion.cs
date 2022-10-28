using System.ComponentModel.DataAnnotations;

namespace He.PipelineAssessment.Infrastructure.Models
{
    public class AssessmentStageVersion
    {
        [Key]
        public int Id { get; set; }
        public int Version { get; set; }
        public string WorkflowDefinitionId { get; set; }
        public int MinimumWorkflowVersion { get; set; }
        public int MaximumWorkflowVersion { get; set; }
    }
}
