using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Exceptions;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.EditRollback
{
    public class EditRollbackCommandHandler : IRequestHandler<EditRollbackCommand, int>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IRoleValidation _roleValidation;
        private readonly ILogger<EditRollbackCommandHandler> _logger;

        public EditRollbackCommandHandler(IAssessmentRepository assessmentRepository, IRoleValidation roleValidation, ILogger<EditRollbackCommandHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _roleValidation = roleValidation;
            _logger = logger;
        }

        public async Task<int> Handle(EditRollbackCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var assessmentIntervention =
                    await _assessmentRepository.GetAssessmentIntervention(command.AssessmentInterventionId);
                if (assessmentIntervention == null)
                {
                    throw new NotFoundException($"Assessment Intervention with Id {command.AssessmentInterventionId} not found");
                }

                var isAuthorised = await _roleValidation.ValidateRole(assessmentIntervention.AssessmentToolWorkflowInstance.AssessmentId, assessmentIntervention.AssessmentToolWorkflowInstance.WorkflowDefinitionId);
                if (!isAuthorised)
                {
                    throw new UnauthorizedAccessException($"You do not have permission to access this resource.");
                }

                assessmentIntervention.AdministratorRationale = command.AdministratorRationale;
                assessmentIntervention.TargetAssessmentToolWorkflowId = command.TargetWorkflowId;
                assessmentIntervention.Administrator = command.Administrator;
                assessmentIntervention.AdministratorEmail = command.AdministratorEmail;
                await _assessmentRepository.SaveChanges();
                return assessmentIntervention.Id;
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to edit rollback. AssessmentInterventionId: {command.AssessmentInterventionId}");
            }

        }
    }
}
