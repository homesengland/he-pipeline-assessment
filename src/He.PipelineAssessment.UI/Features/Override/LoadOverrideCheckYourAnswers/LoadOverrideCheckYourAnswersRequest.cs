using He.PipelineAssessment.UI.Features.Override.SubmitOverride;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Override.LoadOverrideCheckYourAnswers
{
    public class LoadOverrideCheckYourAnswersRequest : IRequest<SubmitOverrideCommand>
    {
        public int InterventionId { get; set; }
    }
}
