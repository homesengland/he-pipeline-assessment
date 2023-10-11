using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention
{
    public class EditInterventionRequest : IRequest<AssessmentInterventionDto>
    {
        public int InterventionId { get; set; }
    }
}
