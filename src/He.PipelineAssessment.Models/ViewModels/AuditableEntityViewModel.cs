namespace He.PipelineAssessment.Models.ViewModels
{
    public abstract class AuditableEntityViewModel
    {
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? LastModifiedDateTime { get; set; }
    }
}
