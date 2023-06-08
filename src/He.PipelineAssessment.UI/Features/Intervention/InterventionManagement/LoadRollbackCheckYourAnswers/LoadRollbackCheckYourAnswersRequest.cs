using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.SubmitRollback;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.LoadRollbackCheckYourAnswers
{
    public class LoadRollbackCheckYourAnswersRequest : IRequest<SubmitRollbackCommand>
    {
        public int InterventionId { get; set; }
    }
}
