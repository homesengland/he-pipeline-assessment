using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Exceptions;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Ammendment.DeleteAmmendment
{
    public class DeleteAmmendmentCommandHandler : IRequestHandler<DeleteAmmendmentCommand, int>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IRoleValidation _roleValidation;
        private readonly ILogger<DeleteAmmendmentCommandHandler> _logger;

        public DeleteAmmendmentCommandHandler(IAssessmentRepository assessmentRepository, IRoleValidation roleValidation, ILogger<DeleteAmmendmentCommandHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _roleValidation = roleValidation;
            _logger = logger;
        }

        public async Task<int> Handle(DeleteAmmendmentCommand command, CancellationToken cancellationToken)
        {
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
                throw new ApplicationException($"Unable to delete ammendment. WorkflowInstanceId: {command.WorkflowInstanceId}");
            }

        }
    }
}
