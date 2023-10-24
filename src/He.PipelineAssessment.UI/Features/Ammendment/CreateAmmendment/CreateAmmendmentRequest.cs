using He.PipelineAssessment.UI.Features.Intervention;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Ammendment.CreateAmmendment
{
    public class CreateAmmendmentRequest : IRequest<AssessmentInterventionDto>
    {
        public string WorkflowInstanceId { get; set; } = null!;
    }
}
