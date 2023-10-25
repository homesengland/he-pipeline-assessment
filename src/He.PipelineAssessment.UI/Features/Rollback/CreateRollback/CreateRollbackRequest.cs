using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Intervention;

namespace He.PipelineAssessment.UI.Features.Rollback.CreateRollback
{
    public class CreateRollbackRequest : CreateInterventionRequest
    {
        public override string DecisionType => InterventionDecisionTypes.Rollback;
    }
}
