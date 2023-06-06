using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateRollback
{
    public class CreateRollbackRequest : IRequest<AssessmentInterventionDto>
    {
        public string WorkflowInstanceId { get; set; } = null!;
    }
}
