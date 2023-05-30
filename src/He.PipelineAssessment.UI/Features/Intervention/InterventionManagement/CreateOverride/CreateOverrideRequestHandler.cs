using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Intervention.Constants;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateOverride
{
    public class CreateOverrideRequestHandler : IRequestHandler<CreateOverrideRequest, AssessmentInterventionDto>
    {

        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IAdminAssessmentToolWorkflowRepository _adminAssessmentToolWorkflowRepository;
        private readonly IUserProvider _userProvider;
        private readonly IAssessmentInterventionMapper _mapper;

        public CreateOverrideRequestHandler(IAssessmentRepository assessmentRepository, 
            IUserProvider userProvider, 
            IAdminAssessmentToolWorkflowRepository adminAssessmentToolWorkflowRepository, 
            IAssessmentInterventionMapper mapper)
        {
            _assessmentRepository = assessmentRepository;
            _userProvider = userProvider;
            _adminAssessmentToolWorkflowRepository = adminAssessmentToolWorkflowRepository;
            _mapper = mapper;
        }

        public async Task<AssessmentInterventionDto> Handle(CreateOverrideRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                AssessmentToolWorkflowInstance? workflowInstance =
                    await _assessmentRepository.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId);
                var dto = DtoFromWorkflowInstance(workflowInstance);

                var assessmentToolWorkflows = await _adminAssessmentToolWorkflowRepository.GetAssessmentToolWorkflows();
                dto.TargetWorkflowDefinitions =
                    _mapper.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(assessmentToolWorkflows);
                return dto;
            }
            catch (Exception e)
            {
                return new AssessmentInterventionDto
                {

                };

            }
        }

        public AssessmentInterventionDto DtoFromWorkflowInstance(AssessmentToolWorkflowInstance instance)
        {
            string? adminName = _userProvider.GetUserName();
            string? adminEmail = _userProvider.GetUserEmail();

            AssessmentInterventionDto dto = new AssessmentInterventionDto()
            {
                AssessmentInterventionCommand = new CreateOverrideCommand()
                {
                    AssessmentToolWorkflowInstanceId = instance.Id,
                    WorkflowInstanceId = instance.WorkflowInstanceId,
                    AssessmentResult = instance.Result,
                    AssessmentName = instance.WorkflowName,
                    RequestedBy = adminName,
                    RequestedByEmail = adminEmail,
                    Administrator = adminName,
                    AdministratorEmail = adminEmail,
                    DecisionType = InterventionDecisionTypes.Override,
                    Status = InterventionStatus.NotSubmitted,
                    ProjectReference = instance.Assessment.Reference
                }
            };
            return dto;
        }
    }
}
