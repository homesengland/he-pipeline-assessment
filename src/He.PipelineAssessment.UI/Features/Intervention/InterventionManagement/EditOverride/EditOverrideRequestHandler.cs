using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.EditOverride
{
    public class EditOverrideRequestHandler : IRequestHandler<EditOverrideRequest, AssessmentInterventionDto>
    {

        private readonly IAssessmentInterventionMapper _mapper;
        private readonly IAssessmentRepository _repository;
        private readonly IAdminAssessmentToolWorkflowRepository _adminAssessmentToolWorkflowRepository;

        public EditOverrideRequestHandler(IAssessmentInterventionMapper mapper, IAssessmentRepository repo, IAdminAssessmentToolWorkflowRepository adminAssessmentToolWorkflowRepository)
        {
            _mapper = mapper;
            _repository = repo;
            _adminAssessmentToolWorkflowRepository = adminAssessmentToolWorkflowRepository;
        }
        public async Task<AssessmentInterventionDto> Handle(EditOverrideRequest request, CancellationToken cancellationToken)
        {
            try
            {
                AssessmentIntervention? intervention = await _repository.GetAssessmentIntervention(request.InterventionId);
                AssessmentInterventionCommand command = _mapper.AssessmentInterventionCommandFromAssessmentIntervention(intervention);
                var assessmentToolWorkflows = await _adminAssessmentToolWorkflowRepository.GetAssessmentToolWorkflows();
                var dto = new AssessmentInterventionDto
                {
                    AssessmentInterventionCommand = command,
                    TargetWorkflowDefinitions =
                        _mapper.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(assessmentToolWorkflows)
                };
                return dto;
            }
            catch(Exception e)
            {
                return new AssessmentInterventionDto();
            }
        }
    }
}
