using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Exceptions;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.EditOverride
{
    public class EditOverrideCommandHandler : IRequestHandler<EditOverrideCommand, int>
    {
        private readonly IAssessmentRepository _assessmentRepository;

        public EditOverrideCommandHandler(IAssessmentRepository assessmentRepository)
        {
            _assessmentRepository = assessmentRepository;
        }

        public async Task<int> Handle(EditOverrideCommand command, CancellationToken cancellationToken)
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
    }
}
