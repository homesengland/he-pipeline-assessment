using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessment.SensitiveRecordPermissionsWhitelist
{
    public class SensitiveRecordPermissionsWhitelistRequest : IRequest<SensitiveRecordPermissionsWhitelistResponse>
    {
        public int AssessmentId { get; set; }
        public SensitiveRecordPermissionsWhitelistRequest(int assessmentId)
        {
            AssessmentId = assessmentId;
        }
    }
}
