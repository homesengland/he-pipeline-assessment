using He.PipelineAssessment.Infrastructure.Repository;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.EditOverride
{
    public class EditOverrideCommandHandler : IRequestHandler<EditOverrideCommand, int>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<EditOverrideCommandHandler> _logger;

        public EditOverrideCommandHandler(IAssessmentRepository assessmentRepository, ILogger<EditOverrideCommandHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
        }

        public async Task<int> Handle(EditOverrideCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var assessmentIntervention =
                    await _assessmentRepository.GetAssessmentIntervention(command.AssessmentInterventionId);
                if (assessmentIntervention != null)
                {
                    assessmentIntervention.SignOffDocument = command.SignOffDocument;
                    assessmentIntervention.AdministratorRationale = command.AdministratorRationale;
                    assessmentIntervention.TargetAssessmentToolWorkflowId = command.TargetWorkflowId;
                    await _assessmentRepository.SaveChanges();
                    return assessmentIntervention.Id;
                }
                else
                {
                    return -1;
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                //TODO Tech Debt Item in backlog to align our exception handling in controllers/handlers. 
                throw new Exception(e.Message, e.InnerException);
            }

        }
    }
}
