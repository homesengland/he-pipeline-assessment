using He.PipelineAssessment.UI.Features.Rollback.ConfirmRollback;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.LoadRollbackCheckYourAnswersAssessor
{
    public class LoadRollbackCheckYourAnswersAssessorRequest : IRequest<ConfirmRollbackCommand>
    {
        public int InterventionId { get; set; }
    }
}
