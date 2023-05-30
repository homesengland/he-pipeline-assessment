using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.LoadOverrideCheckYourAnswers
{
    public class LoadOverrideCheckYourAnswersRequest : IRequest<LoadOverrideCheckYourAnswersCommand>
    {
        public int InterventionId { get; set; }
    }
}
