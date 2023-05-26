using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateOverride
{
    public class CreateOverrideRequest : IRequest<CreateAssessmentInterventionDto>
    {
        public string WorkflowInstanceId { get; set; } = null!;
    }
}
