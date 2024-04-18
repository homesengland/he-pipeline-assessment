using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Intervention;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Amendment.CreateAmendment
{
    public class CreateAmendmentRequest : CreateInterventionRequest
    {
        public override string DecisionType => InterventionDecisionTypes.Amendment;
        public override string InitialStatus => InterventionStatus.Draft;
    }
}
