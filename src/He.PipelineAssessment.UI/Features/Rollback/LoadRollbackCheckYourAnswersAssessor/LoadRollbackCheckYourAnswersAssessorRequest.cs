using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Features.Rollback.ConfirmRollback;
using He.PipelineAssessment.UI.Features.Rollback.SubmitRollback;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.LoadRollbackCheckYourAnswersAssessor
{
    public class LoadRollbackCheckYourAnswersAssessorRequest : LoadInterventionCheckYourAnswersRequest<SubmitRollbackCommand>
    {
    }
}
