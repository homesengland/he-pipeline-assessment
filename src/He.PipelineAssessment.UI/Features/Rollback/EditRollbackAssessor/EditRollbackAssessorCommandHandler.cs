using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Exceptions;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.EditRollbackAssessor
{
    public class EditRollbackAssessorCommandHandler : IRequestHandler<EditRollbackAssessorCommand, int>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<EditRollbackAssessorCommandHandler> _logger;

        public EditRollbackAssessorCommandHandler(IAssessmentRepository assessmentRepository, ILogger<EditRollbackAssessorCommandHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
        }

        public async Task<int> Handle(EditRollbackAssessorCommand command, CancellationToken cancellationToken)
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
                throw new ApplicationException($"Unable to edit rollback. AssessmentInterventionId: {command.AssessmentInterventionId}");
            }
        }
    }
}
