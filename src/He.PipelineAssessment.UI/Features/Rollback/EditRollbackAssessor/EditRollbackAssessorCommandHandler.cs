using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Exceptions;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.EditRollbackAssessor
{
    public class EditRollbackAssessorCommandHandler : IRequestHandler<EditRollbackAssessorCommand, int>
    {
        private readonly IAssessmentRepository _assessmentRepository;

        public EditRollbackAssessorCommandHandler(IAssessmentRepository assessmentRepository)
        {
            _assessmentRepository = assessmentRepository;
        }

        public async Task<int> Handle(EditRollbackAssessorCommand command, CancellationToken cancellationToken)
        {
            var assessmentIntervention =
                await _assessmentRepository.GetAssessmentIntervention(command.AssessmentInterventionId);
            if (assessmentIntervention == null)
            {
                throw new NotFoundException($"Assessment Intervention with Id {command.AssessmentInterventionId} not found");
            }
            assessmentIntervention.AssessorRationale = command.AssessorRationale;
            await _assessmentRepository.SaveChanges();
            return assessmentIntervention.Id;
        }
    }
}
