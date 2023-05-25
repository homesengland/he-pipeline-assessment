using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.Commands.CreateAssessmentIntervention
{
    public class CreateAssessmentInterventionRequest : IRequest<CreateAssessmentInterventionDto>
    {
        public string WorkflowInstanceId { get; set; } = null!;
    }
}
