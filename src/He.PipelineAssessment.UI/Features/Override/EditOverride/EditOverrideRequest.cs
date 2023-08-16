using He.PipelineAssessment.UI.Features.Intervention;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Override.EditOverride
{
    public class EditOverrideRequest : IRequest<AssessmentInterventionDto>
    {
        public int InterventionId { get; set; }
    }
}
