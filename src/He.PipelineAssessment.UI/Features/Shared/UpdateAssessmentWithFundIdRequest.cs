using MediatR;

namespace He.PipelineAssessment.UI.Features.Shared
{
    public class UpdateAssessmentWithFundIdRequest : IRequest
    {
        public int AssessmentId { get; set; }
    }
}
