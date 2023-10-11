using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.DeleteRollback
{
    public class DeleteRollbackCommandHandler : IRequestHandler<DeleteRollbackCommand, int>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IRoleValidation _roleValidation;
        private readonly ILogger<DeleteRollbackCommandHandler> _logger;
        private readonly IInterventionService _interventionService;

        public DeleteRollbackCommandHandler(IAssessmentRepository assessmentRepository, IInterventionService interventionService, IRoleValidation roleValidation, ILogger<DeleteRollbackCommandHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _roleValidation = roleValidation;
            _logger = logger;
            _interventionService = interventionService;
        }

        public async Task<int> Handle(DeleteRollbackCommand command, CancellationToken cancellationToken)
        {
            _interventionService.DeleteIntervention(command);
            try
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
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to delete rollback. WorkflowInstanceId: {command.WorkflowInstanceId}");
            }

        }
    }
}
