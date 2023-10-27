using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Intervention;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Override.SubmitOverride
{
    public class SubmitOverrideCommand : AssessmentInterventionCommand, IRequest
    {
        //TODO: change this to override
        public override string FinalInstanceStatus => AssessmentToolWorkflowInstanceConstants.SuspendedRollBack;
    }
}
