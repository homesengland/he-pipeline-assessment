using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.ConfirmRollback;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.LoadRollbackCheckYourAnswersAssessor
{
    public class LoadRollbackCheckYourAnswersAssessorRequest : IRequest<ConfirmRollbackCommand>
    {
        public int InterventionId { get; set; }
    }
}
