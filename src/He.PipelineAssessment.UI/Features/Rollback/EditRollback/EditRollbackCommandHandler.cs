using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Exceptions;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.EditRollback
{
    public class EditRollbackCommandHandler : IRequestHandler<EditRollbackCommand, int>
    {
        private readonly IAssessmentRepository _assessmentRepository;

        public EditRollbackCommandHandler(IAssessmentRepository assessmentRepository)
        {
            _assessmentRepository = assessmentRepository;
        }

        public async Task<int> Handle(EditRollbackCommand command, CancellationToken cancellationToken)
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
