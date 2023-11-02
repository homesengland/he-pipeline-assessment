using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Intervention;

namespace He.PipelineAssessment.UI.Features.Override.CreateOverride
{
    public class CreateOverrideRequest : CreateInterventionRequest
    {
        public override string DecisionType => InterventionDecisionTypes.Override;
        public override string InitialStatus => InterventionStatus.Pending;
    }
}
