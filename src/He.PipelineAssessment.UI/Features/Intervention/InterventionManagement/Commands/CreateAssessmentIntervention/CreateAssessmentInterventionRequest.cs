using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.Commands.CreateAssessmentIntervention
{
    public class CreateAssessmentInterventionRequest : IRequest
    {
        public int AssessmentWorkflowInstanceId { get; set; }
    }
}
