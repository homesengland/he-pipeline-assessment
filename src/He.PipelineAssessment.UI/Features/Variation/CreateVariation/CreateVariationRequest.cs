using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Intervention;

namespace He.PipelineAssessment.UI.Features.Variation.CreateVariation
{
    public class CreateVariationRequest : CreateInterventionRequest
    {
        public override string DecisionType => InterventionDecisionTypes.Variation;
    }
}
