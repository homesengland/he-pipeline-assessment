using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.EditRollback
{
    public class EditRollbackRequest : IRequest<AssessmentInterventionDto>
    {
        public int InterventionId { get; set; }
    }
}
