using He.PipelineAssessment.Models;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention
{
    public class CreateInterventionRequest : IRequest<AssessmentInterventionDto>
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public virtual string DecisionType => "";
        public virtual string InitialStatus => InterventionStatus.Draft;
    }
}
