using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Intervention.Constants;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateOverride
{
    public class CreateOverrideRequestHandler : IRequestHandler<CreateOverrideRequest, CreateAssessmentInterventionDto>
    {

        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IAdminAssessmentToolWorkflowRepository _adminAssessmentToolWorkflowRepository;
        private readonly IUserProvider _userProvider;

        public CreateOverrideRequestHandler(IAssessmentRepository assessmentRepository, IUserProvider userProvider, IAdminAssessmentToolWorkflowRepository adminAssessmentToolWorkflowRepository)
        {
            _assessmentRepository = assessmentRepository;
            _userProvider = userProvider;
            _adminAssessmentToolWorkflowRepository = adminAssessmentToolWorkflowRepository;
        }

        public async Task<CreateAssessmentInterventionDto> Handle(CreateOverrideRequest request, CancellationToken cancellationToken)
        {
            try
            {
                AssessmentToolWorkflowInstance? workflowInstance = await _assessmentRepository.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId);
                var dto = DtoFromWorkflowInstance(workflowInstance);

                var assessmentToolWorkflows = await _adminAssessmentToolWorkflowRepository.GetAssessmentToolWorkflows();
                dto.TargetWorkflowDefinitions = TargetWorkflowDefinitionsFromAssessmentToolWorkflows(assessmentToolWorkflows);
                return dto;
            }
            catch (Exception e)
            {
                return new CreateAssessmentInterventionDto
                {

                };

            }
        }

        private List<TargetWorkflowDefinition> TargetWorkflowDefinitionsFromAssessmentToolWorkflows(List<AssessmentToolWorkflow> assessmentToolWorkflows)
        {
            return assessmentToolWorkflows.Select(x => new TargetWorkflowDefinition
            {
                Id = x.WorkflowDefinitionId,
                Name = x.Name
            }).ToList();
        }

        public CreateAssessmentInterventionDto DtoFromWorkflowInstance(AssessmentToolWorkflowInstance instance)
        {
            string? adminName = _userProvider.GetUserName();
            string? adminEmail = _userProvider.GetUserEmail();

            CreateAssessmentInterventionDto dto = new CreateAssessmentInterventionDto()
            {
                CreateAssessmentInterventionCommand = new CreateOverrideCommand()
                {
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
