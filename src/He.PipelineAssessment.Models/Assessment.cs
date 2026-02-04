using He.PipelineAssessment.Models.Helper;
using Microsoft.EntityFrameworkCore;

namespace He.PipelineAssessment.Models
{
    [Index(nameof(Assessment.SpId), IsUnique = true)]
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
        public string BusinessArea { get; set; } = string.Empty;
        public string LandType { get; set; } = string.Empty;
        public virtual List<AssessmentToolWorkflowInstance>? AssessmentToolWorkflowInstances { get; set; }
        public string? SensitiveStatus { get; set; }
        public int? FundId { get; set; }

        //Corresponds to the most recently completed AssessmentToolWorkflow's Id.
        public int? LatestAssessmentWorkflowToolId { get; set; }

        public virtual AssessmentFund? AssessmentFund { get; set; }

        public bool IsSensitiveRecord()
        {
            return SensitiveStatusHelper.IsSensitiveStatus(SensitiveStatus);
        }
    }
}