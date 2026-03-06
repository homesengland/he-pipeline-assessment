using MediatR;

namespace He.PipelineAssessment.UI.Features.Shared
{
    public class UpdateAssessmentStatusRequest : IRequest<Unit>
    {
        public int AssessmentId { get; set; }
    }
}
