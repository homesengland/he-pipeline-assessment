using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessments.AssessmentSummary
{
    public class AssessmentSummaryCommand : IRequest<AssessmentSummaryData>
    {
        public int CorrelationId { get; set; }
        public int AssessmentId { get; set; }
        public AssessmentSummaryCommand(int assessmentId, int correlationId)
        {
            AssessmentId = assessmentId;
            CorrelationId = correlationId;
        }
    }
}
