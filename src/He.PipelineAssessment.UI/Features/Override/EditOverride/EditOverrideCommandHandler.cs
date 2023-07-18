using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Exceptions;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Override.EditOverride
{
    public class EditOverrideCommandHandler : IRequestHandler<EditOverrideCommand, int>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger _logger;

        public EditOverrideCommandHandler(IAssessmentRepository assessmentRepository, ILogger logger)
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
                if (assessmentIntervention == null)
                {
                    throw new NotFoundException($"Assessment Intervention with Id {command.AssessmentInterventionId} not found");
                }
                assessmentIntervention.SignOffDocument = command.SignOffDocument;
                assessmentIntervention.AdministratorRationale = command.AdministratorRationale;
                assessmentIntervention.TargetAssessmentToolWorkflowId = command.TargetWorkflowId;
                await _assessmentRepository.SaveChanges();
                return assessmentIntervention.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ApplicationException($"Unalbe to edit override. WorkflowInstanceId: {command.WorkflowInstanceId} AssessmentInterventionId: {command.AssessmentInterventionId}.");
            }
        }
    }
}
