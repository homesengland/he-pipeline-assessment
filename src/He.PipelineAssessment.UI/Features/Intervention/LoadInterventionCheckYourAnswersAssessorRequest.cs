using He.PipelineAssessment.UI.Features.Rollback.SubmitRollback;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention
{
    public class LoadInterventionCheckYourAnswersAssessorRequest : IRequest<SubmitRollbackCommand>
    {
        public int InterventionId { get; set; }
    }
}
