namespace He.PipelineAssessment.Models
{
    public class InterventionReason: AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Order { get; set; }
        public string? Status { get; set; }
        public bool IsVariation { get; set; }
    }
}
