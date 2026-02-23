using MediatR;

namespace He.PipelineAssessment.UI.Features.Shared
{
    public class UpdateAssessmentWithFundIdRequest : IRequest<Unit>
    {
        public int AssessmentId { get; set; }
    }
}
