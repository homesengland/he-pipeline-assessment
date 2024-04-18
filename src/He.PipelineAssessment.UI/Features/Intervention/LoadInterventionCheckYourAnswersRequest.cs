using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention
{
    public class LoadInterventionCheckYourAnswersRequest : IRequest<AssessmentInterventionCommand>
    {
        public int InterventionId { get; set; }
    }
}
