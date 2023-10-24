using He.PipelineAssessment.UI.Features.Ammendment.SubmitAmmendment;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Ammendment.LoadAmmendmentCheckYourAnswersAssessor
{
    public class LoadAmmendmentCheckYourAnswersAssessorRequest : IRequest<SubmitAmmendmentCommand>
    {
        public int InterventionId { get; set; }
    }
}
