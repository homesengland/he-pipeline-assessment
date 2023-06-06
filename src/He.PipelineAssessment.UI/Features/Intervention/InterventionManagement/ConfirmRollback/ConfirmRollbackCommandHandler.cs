using He.PipelineAssessment.Infrastructure.Repository;
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

            // we might need to update the status to something different? otherwise this command is not doing anything

            await _assessmentRepository.UpdateAssessmentIntervention(intervention);

            return Unit.Value;
        }
    }
}
