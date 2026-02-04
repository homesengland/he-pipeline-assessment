using He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary;

namespace He.PipelineAssessment.UI.Features.Assessment.SensitiveRecordPermissionsWhitelist
{
    public class SensitiveRecordPermissionsWhitelistResponse
    {
        public AssessmentSummaryResponse AssessmentSummary { get; set; } = null!;
        public List<SensitiveRecordPermissionsWhitelistDto> Permissions { get; set; } = new List<SensitiveRecordPermissionsWhitelistDto>();

    }

    public class SensitiveRecordPermissionsWhitelistDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }
        public string AddedBy { get; set; } = string.Empty;
    }
}
