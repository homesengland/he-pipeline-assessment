using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.EditOverride
{
    public class EditOverrideRequest : IRequest<AssessmentInterventionDto>
    {
        public int InterventionId { get; set; }
    }
}
