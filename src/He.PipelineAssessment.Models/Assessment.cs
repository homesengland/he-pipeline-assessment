namespace He.PipelineAssessment.Models
{
    public class Assessment : AuditableEntity
    {
        public int Id { get; set; }
        public int SpId { get; set; }
        public string SiteName { get; set; } = null!;
        public string ProjectManager { get; set; } = null!;
        public string ProjectManagerEmail { get; set; } = null!;
        public string Counterparty { get; set; } = null!;
        public string Reference { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string LocalAuthority { get; set; } = null!;
        public decimal? FundingAsk { get; set; }
        public int? NumberOfHomes { get; set; }

        public virtual List<AssessmentToolWorkflowInstance>? AssessmentToolWorkflowInstances { get; set; }
    }
}