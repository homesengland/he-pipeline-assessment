using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateOverride;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.EditOverride
{
    public class EditOverrideRequestHandler : IRequestHandler<EditOverrideRequest, AssessmentInterventionCommand>
    {

        private readonly IAssessmentInterventionMapper _mapper;
        private readonly IAssessmentRepository _repository;

        public EditOverrideRequestHandler(IAssessmentInterventionMapper mapper, IAssessmentRepository repo)
        {
            _mapper = mapper;
            _repository = repo;
        }
        public async Task<AssessmentInterventionCommand> Handle(EditOverrideRequest request, CancellationToken cancellationToken)
        {
            try
            {
                AssessmentIntervention? intervention = await _repository.GetAssessmentIntervention(request.InterventionId);
                AssessmentInterventionCommand command = _mapper.ToAssessmentInterventionCommand(intervention);
                return command;
            }
            catch(Exception e)
            {

            }

        }
    }
}
