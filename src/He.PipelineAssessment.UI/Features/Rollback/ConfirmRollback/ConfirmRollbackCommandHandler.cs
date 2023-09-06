using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.ConfirmRollback
{
    public class ConfirmRollbackCommandHandler : IRequestHandler<ConfirmRollbackCommand>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<ConfirmRollbackCommandHandler> _logger;

        public ConfirmRollbackCommandHandler(IAssessmentRepository assessmentRepository, ILogger<ConfirmRollbackCommandHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
        }

        public async Task Handle(ConfirmRollbackCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var intervention =
                    await _assessmentRepository.GetAssessmentIntervention(command.AssessmentInterventionId);
                if (intervention == null)
                {
                    throw new NotFoundException($"Assessment Intervention with Id {command.AssessmentInterventionId} not found");
                }

                intervention.Status = InterventionStatus.Pending;
                await _assessmentRepository.UpdateAssessmentIntervention(intervention);
            }
            catch (Exception e)
            {
               _logger.LogError(e,e.Message);
               throw new ApplicationException($"Confirm rollback failed. AssessmentInterventionId: {command.AssessmentInterventionId}");
            }
           
        }
    }
}
