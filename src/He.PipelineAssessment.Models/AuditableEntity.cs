namespace He.PipelineAssessment.Models
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedDateTime { get; set; }

        public string CreatedBy { get; set; } = null!;

        public DateTime? LastModifiedDateTime { get; set; }

        public string? LastModifiedBy { get; set; }
    }
}
