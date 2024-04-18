using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Intervention;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Amendment.SubmitAmendment
{
    public class SubmitAmendmentCommand : AssessmentInterventionCommand, IRequest
    {
        public override string FinalInstanceStatus => AssessmentToolWorkflowInstanceConstants.SuspendedAmendment;
    }
}
