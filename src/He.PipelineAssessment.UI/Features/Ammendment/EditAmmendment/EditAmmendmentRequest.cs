using He.PipelineAssessment.UI.Features.Intervention;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Ammendment.EditAmmendment
{
    public class EditAmmendmentRequest : IRequest<AssessmentInterventionDto>
    {
        public int InterventionId { get; set; }
    }
}
