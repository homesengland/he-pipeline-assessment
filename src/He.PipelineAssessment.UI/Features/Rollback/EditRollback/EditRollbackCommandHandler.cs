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

        public EditRollbackCommandHandler(IAssessmentRepository assessmentRepository, IRoleValidation roleValidation)
        {
            _assessmentRepository = assessmentRepository;
            _roleValidation = roleValidation;
        }

        public async Task<int> Handle(EditRollbackCommand command, CancellationToken cancellationToken)
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
    }
}
