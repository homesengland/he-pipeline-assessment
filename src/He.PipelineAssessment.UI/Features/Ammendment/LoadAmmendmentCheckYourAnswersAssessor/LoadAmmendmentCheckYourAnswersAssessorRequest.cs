using He.PipelineAssessment.UI.Features.Ammendment.ConfirmAmmendment;
using He.PipelineAssessment.UI.Features.Rollback.ConfirmRollback;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Ammendment.LoadAmmendmentCheckYourAnswersAssessor
{
    public class LoadAmmendmentCheckYourAnswersAssessorRequest : IRequest<ConfirmAmmendmentCommand>
    {
        public int InterventionId { get; set; }
    }
}
