using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Rollback.EditRollbackAssessor;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Ammendment.EditAmmendment
{
    public class EditAmmendmentCommandHandler : IRequestHandler<EditAmmendmentCommand, int>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<EditAmmendmentCommandHandler> _logger;

        public EditAmmendmentCommandHandler(IAssessmentRepository assessmentRepository, ILogger<EditAmmendmentCommandHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
        }

        public async Task<int> Handle(EditAmmendmentCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var assessmentIntervention =
                    await _assessmentRepository.GetAssessmentIntervention(command.AssessmentInterventionId);
                if (assessmentIntervention == null)
                {
                    throw new NotFoundException($"Assessment Intervention with Id {command.AssessmentInterventionId} not found");
                }
                assessmentIntervention.AssessorRationale = command.AssessorRationale;
                assessmentIntervention.InterventionReasonId = command.InterventionReasonId;
                await _assessmentRepository.SaveChanges();
                return assessmentIntervention.Id;
            }

            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to edit ammendment. AssessmentInterventionId: {command.AssessmentInterventionId}");
            }
        }
    }
}
