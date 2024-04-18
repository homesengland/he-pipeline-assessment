using He.PipelineAssessment.Models;

namespace He.PipelineAssessment.UI.Integration.Project
{
    public class ProjectDTO
    {
        public int ProjectId { get; set; }
        public string SiteName { get; set; } = null!;
        public string ProjectOwner { get; set; } = null!;
        public string ProjectOwnerEmail { get; set; } = null!;
        public string Counterparties { get; set; } = null!;
        public string PcsReference { get; set; } = null!;
        public string LocalAuthority { get; set; } = null!;
        public decimal? FundingAsk { get; set; }
        public int NumberOfHomes { get; set; }
        public string BusinessArea { get; set; } = string.Empty;
        public string LandType { get; set; } = string.Empty;
        public string SensitiveStatus { get; set; } = string.Empty;
    }
}
