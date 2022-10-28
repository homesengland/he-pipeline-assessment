using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessments.AssessmentSummary
{
    public class AssessmentSummaryCommand : IRequest<AssessmentSummaryData>
    {
        public string AssessmentId { get; set; }
        public AssessmentSummaryCommand(string assessmentId)
        {
            AssessmentId = assessmentId;
        }
    }
}
