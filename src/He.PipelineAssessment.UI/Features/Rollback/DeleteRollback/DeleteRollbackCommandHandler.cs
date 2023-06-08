using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Exceptions;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.DeleteRollback
{
    public class DeleteRollbackCommandHandler : IRequestHandler<DeleteRollbackCommand, int>
    {
        private readonly IAssessmentRepository _assessmentRepository;

        public DeleteRollbackCommandHandler(IAssessmentRepository assessmentRepository)
        {
            _assessmentRepository = assessmentRepository;
        }

        public async Task<int> Handle(DeleteRollbackCommand command, CancellationToken cancellationToken)
        {
            var intervention =
                await _assessmentRepository.GetAssessmentIntervention(command.AssessmentInterventionId);
            if (intervention == null)
            {
                throw new NotFoundException($"Assessment Intervention with Id {command.AssessmentInterventionId} not found");
            }

            return await _assessmentRepository.DeleteIntervention(intervention);
        }
    }
}
