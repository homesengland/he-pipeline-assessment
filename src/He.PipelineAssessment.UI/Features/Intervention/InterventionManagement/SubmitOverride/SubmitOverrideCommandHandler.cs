using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.SubmitOverride
{
    public class SubmitOverrideCommandHandler : IRequestHandler<SubmitOverrideCommand, Unit>
    {


        private readonly IAssessmentRepository _assessmentRepository;

        public SubmitOverrideCommandHandler(IAssessmentRepository assessmentRepository)
        {
            _assessmentRepository = assessmentRepository;
        }

        public async Task<Unit> Handle(SubmitOverrideCommand request, CancellationToken cancellationToken)
        {
            try
            {
                AssessmentIntervention intervention = InterventionFromCommand(request);
                await _assessmentRepository.UpdateAssessmentIntervention(intervention);
            }
            catch(Exception e)
            {

            }

            
        }

        private AssessmentIntervention InterventionFromCommand(SubmitOverrideCommand command)
        {
            return new AssessmentIntervention()
            {
                Id = command.AssessmentInterventionId,
            };
        }
    }
}
