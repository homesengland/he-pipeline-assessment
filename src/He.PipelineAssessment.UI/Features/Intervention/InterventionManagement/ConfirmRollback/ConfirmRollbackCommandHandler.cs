using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.ConfirmRollback
{
    public class ConfirmRollbackCommandHandler : IRequestHandler<ConfirmRollbackCommand>
    {
        private readonly IAssessmentRepository _assessmentRepository;

        public ConfirmRollbackCommandHandler(IAssessmentRepository assessmentRepository)
        {
            _assessmentRepository = assessmentRepository;
        }

        public async Task<Unit> Handle(ConfirmRollbackCommand command, CancellationToken cancellationToken)
        {
            var intervention =
                await _assessmentRepository.GetAssessmentIntervention(command.AssessmentInterventionId);
            if (intervention == null)
            {
                throw new NotFoundException($"Assessment Intervention with Id {command.AssessmentInterventionId} not found");
            }

            intervention.Status = InterventionStatus.Pending;
            await _assessmentRepository.UpdateAssessmentIntervention(intervention);

            return Unit.Value;
        }
    }
}
