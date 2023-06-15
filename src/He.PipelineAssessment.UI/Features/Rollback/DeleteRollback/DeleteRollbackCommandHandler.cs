using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Exceptions;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.DeleteRollback
{
    public class DeleteRollbackCommandHandler : IRequestHandler<DeleteRollbackCommand, int>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IRoleValidation _roleValidation;

        public DeleteRollbackCommandHandler(IAssessmentRepository assessmentRepository, IRoleValidation roleValidation)
        {
            _assessmentRepository = assessmentRepository;
            _roleValidation = roleValidation;
        }

        public async Task<int> Handle(DeleteRollbackCommand command, CancellationToken cancellationToken)
        {
            var intervention =
                await _assessmentRepository.GetAssessmentIntervention(command.AssessmentInterventionId);
            if (intervention == null)
            {
                throw new NotFoundException($"Assessment Intervention with Id {command.AssessmentInterventionId} not found");
            }

            var isAuthorised = await _roleValidation.ValidateRole(intervention.AssessmentToolWorkflowInstance.AssessmentId, intervention.AssessmentToolWorkflowInstance.WorkflowDefinitionId);
            if (!isAuthorised)
            {
                throw new UnauthorizedAccessException($"You do not have permission to access this resource.");
            }

            return await _assessmentRepository.DeleteIntervention(intervention);
        }
    }
}
