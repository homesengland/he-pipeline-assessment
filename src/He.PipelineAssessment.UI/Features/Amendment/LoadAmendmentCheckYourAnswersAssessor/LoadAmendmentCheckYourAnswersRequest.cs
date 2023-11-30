using He.PipelineAssessment.UI.Features.Amendment.SubmitAmendment;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Amendment.LoadAmendmentCheckYourAnswers
{
    public class LoadAmendmentCheckYourAnswersRequest : IRequest<SubmitAmendmentCommand>
    {
        public int InterventionId { get; set; }
    }
}
