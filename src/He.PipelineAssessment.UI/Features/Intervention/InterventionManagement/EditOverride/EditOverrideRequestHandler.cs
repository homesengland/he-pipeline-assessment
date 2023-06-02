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
        private readonly ILogger<EditOverrideRequestHandler> _logger;

        public EditOverrideRequestHandler(IAssessmentInterventionMapper mapper, 
            IAssessmentRepository repo, IAdminAssessmentToolWorkflowRepository adminAssessmentToolWorkflowRepository, 
            ILogger<EditOverrideRequestHandler> logger)
        {
            _mapper = mapper;
            _repository = repo;
            _adminAssessmentToolWorkflowRepository = adminAssessmentToolWorkflowRepository;
            _logger = logger;
        }
        public async Task<AssessmentInterventionDto> Handle(EditOverrideRequest request, CancellationToken cancellationToken)
        {
            try
            {
                AssessmentIntervention? intervention = await _repository.GetAssessmentIntervention(request.InterventionId);
                if(intervention == null)
                {
                    return new AssessmentInterventionDto();
                }
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
                _logger.LogError(e.Message);
                return new AssessmentInterventionDto();
            }
        }
    }
}
