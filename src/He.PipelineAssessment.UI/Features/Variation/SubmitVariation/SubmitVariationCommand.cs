using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Intervention;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Variation.SubmitVariation
{
    public class SubmitVariationCommand : AssessmentInterventionCommand, IRequest
    {
        public override string FinalInstanceStatus => AssessmentToolWorkflowInstanceConstants.SuspendedRollBack;
    }
}
