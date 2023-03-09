namespace He.PipelineAssessment.Models.ViewModels
{
    public abstract class AuditableEntityViewModel
    {
        public DateTime? CreatedDateTime { get; set; }

        //public string CreatedBy { get; set; }

        public DateTime? LastModifiedDateTime { get; set; }

        //public string? LastModifiedBy { get; set; }
    }
}
