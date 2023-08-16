using He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessment.TestAssessmentSummary
{
    public class TestAssessmentSummaryRequest : IRequest<AssessmentSummaryResponse>
    {
        public int CorrelationId { get; set; }
        public int AssessmentId { get; set; }
        public TestAssessmentSummaryRequest(int assessmentId, int correlationId)
        {
            AssessmentId = assessmentId;
            CorrelationId = correlationId;
        }
    }
}
