using He.PipelineAssessment.UI.Features.Rollback.SubmitRollback;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention
{
    public class LoadInterventionCheckYourAnswersRequest<T> : IRequest<T>
    {
        public int InterventionId { get; set; }
    }
}
