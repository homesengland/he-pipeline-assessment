using He.PipelineAssessment.UI.Features.Ammendment.SubmitAmmendment;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Ammendment.LoadAmmendmentCheckYourAnswers
{
    public class LoadAmmendmentCheckYourAnswersRequest : IRequest<SubmitAmmendmentCommand>
    {
        public int InterventionId { get; set; }
    }
}
