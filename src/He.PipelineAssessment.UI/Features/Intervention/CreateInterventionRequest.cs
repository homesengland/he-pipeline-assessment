using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention
{
    public class CreateInterventionRequest : IRequest<AssessmentInterventionDto>
    {
        public string WorkflowInstanceId { get; set; } = null!;
    }
}
