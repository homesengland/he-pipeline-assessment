using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Intervention;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.SubmitRollback
{
    public class SubmitRollbackCommand : AssessmentInterventionCommand, IRequest
    {
        public override string FinalInstanceStatus => AssessmentToolWorkflowInstanceConstants.SuspendedRollBack;
    }
}
