using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Exceptions;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Amendment.EditAmendment
{
    public class EditAmendmentCommandHandler : IRequestHandler<EditAmendmentCommand, int>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<EditAmendmentCommandHandler> _logger;
        private readonly IRoleValidation _roleValidation;


        public EditAmendmentCommandHandler(IAssessmentRepository assessmentRepository, ILogger<EditAmendmentCommandHandler> logger, IRoleValidation roleValidation)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
            _roleValidation = roleValidation;
        }

        public async Task<int> Handle(EditAmendmentCommand command, CancellationToken cancellationToken)
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

                assessmentIntervention.AssessorRationale = command.AssessorRationale;
                assessmentIntervention.InterventionReasonId = command.InterventionReasonId;
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
                throw new ApplicationException($"Unable to edit amendment. AssessmentInterventionId: {command.AssessmentInterventionId}");
            }
        }
    }
}
