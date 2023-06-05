using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateOverride
{
    public class CreateOverrideRequestHandler : IRequestHandler<CreateOverrideRequest, AssessmentInterventionDto>
    {

        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IAdminAssessmentToolWorkflowRepository _adminAssessmentToolWorkflowRepository;
        private readonly IUserProvider _userProvider;
        private readonly IAssessmentInterventionMapper _mapper;
        private readonly ILogger<CreateOverrideRequestHandler> _logger;

        public CreateOverrideRequestHandler(IAssessmentRepository assessmentRepository, 
            IUserProvider userProvider, 
            IAdminAssessmentToolWorkflowRepository adminAssessmentToolWorkflowRepository, 
            IAssessmentInterventionMapper mapper, ILogger<CreateOverrideRequestHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _userProvider = userProvider;
            _adminAssessmentToolWorkflowRepository = adminAssessmentToolWorkflowRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AssessmentInterventionDto> Handle(CreateOverrideRequest request, CancellationToken cancellationToken)
        {
            AssessmentToolWorkflowInstance? workflowInstance = await _assessmentRepository.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId);
            if (workflowInstance == null)
            {
                throw new NotFoundException($"Assessment Tool Workflow Instance with Id {request.WorkflowInstanceId} not found");
            }

            var dto = _mapper.AssessmentInterventionDtoFromWorkflowInstance(workflowInstance, _userProvider.GetUserName()!, _userProvider.GetUserEmail()!);
            List<AssessmentToolWorkflow> assessmentToolWorkflows = await _adminAssessmentToolWorkflowRepository.GetAssessmentToolWorkflows();

            if (assessmentToolWorkflows == null || !assessmentToolWorkflows.Any())
            {
                throw new NotFoundException($"No Assessment tool workflows found");
            }

            dto.TargetWorkflowDefinitions = _mapper.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(assessmentToolWorkflows);
            return dto;
        }
    }
}
