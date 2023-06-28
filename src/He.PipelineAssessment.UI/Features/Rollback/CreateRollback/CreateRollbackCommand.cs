using He.PipelineAssessment.UI.Features.Intervention;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.CreateRollback
{
    public class CreateRollbackCommand : AssessmentInterventionCommand, IRequest<int>
    {

    }
}
