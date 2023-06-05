using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.EditRollbackAssessor
{
    public class EditRollbackAssessorRequest : IRequest<AssessmentInterventionDto>
    {
        public int InterventionId { get; set; }
    }
}
