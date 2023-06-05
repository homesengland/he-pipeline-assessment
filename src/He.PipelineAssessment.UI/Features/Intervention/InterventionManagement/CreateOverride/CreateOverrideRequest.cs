using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateOverride
{
    public class CreateOverrideRequest : IRequest<AssessmentInterventionDto>
    {
        public string WorkflowInstanceId { get; set; } = null!;
    }
}
