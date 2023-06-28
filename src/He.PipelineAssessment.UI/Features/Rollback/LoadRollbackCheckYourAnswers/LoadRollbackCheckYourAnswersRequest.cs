using He.PipelineAssessment.UI.Features.Rollback.SubmitRollback;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.LoadRollbackCheckYourAnswers
{
    public class LoadRollbackCheckYourAnswersRequest : IRequest<SubmitRollbackCommand>
    {
        public int InterventionId { get; set; }
    }
}
