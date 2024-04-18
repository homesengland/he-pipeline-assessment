using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention
{
    public class EditInterventionAssessorRequest : IRequest<AssessmentInterventionDto>
    {
        public int InterventionId { get; set; }
    }
}
