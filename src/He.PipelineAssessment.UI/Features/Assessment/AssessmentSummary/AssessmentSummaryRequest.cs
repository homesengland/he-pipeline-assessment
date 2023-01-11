using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary
{
    public class AssessmentSummaryRequest : IRequest<AssessmentSummaryResponse>
    {
        public int CorrelationId { get; set; }
        public int AssessmentId { get; set; }
        public AssessmentSummaryRequest(int assessmentId, int correlationId)
        {
            AssessmentId = assessmentId;
            CorrelationId = correlationId;
        }
    }
}
