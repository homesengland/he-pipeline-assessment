using Auth0.ManagementApi.Models;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Intervention;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Override.CreateOverride
{
    public class CreateOverrideRequestHandler : IRequestHandler<CreateOverrideRequest, AssessmentInterventionDto>
    {

        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IAdminAssessmentToolWorkflowRepository _adminAssessmentToolWorkflowRepository;
        private readonly IUserProvider _userProvider;
        private readonly IAssessmentInterventionMapper _mapper;
        private readonly ILogger<CreateOverrideRequestHandler> _logger;
        private readonly IAssessmentToolWorkflowInstanceHelpers _assessmentToolWorkflowInstanceHelpers;

        public CreateOverrideRequestHandler(IAssessmentRepository assessmentRepository,
            IUserProvider userProvider,
            IAdminAssessmentToolWorkflowRepository adminAssessmentToolWorkflowRepository,
            IAssessmentInterventionMapper mapper, ILogger<CreateOverrideRequestHandler> logger, IAssessmentToolWorkflowInstanceHelpers assessmentToolWorkflowInstanceHelpers)
        {
            _assessmentRepository = assessmentRepository;
            _userProvider = userProvider;
            _adminAssessmentToolWorkflowRepository = adminAssessmentToolWorkflowRepository;
            _mapper = mapper;
            _logger = logger;
            _assessmentToolWorkflowInstanceHelpers = assessmentToolWorkflowInstanceHelpers;
        }

        public async Task<AssessmentInterventionDto> Handle(CreateOverrideRequest request, CancellationToken cancellationToken)
        {
            AssessmentToolWorkflowInstance? workflowInstance = await _assessmentRepository.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId);
            if (workflowInstance == null)
            {
                throw new NotFoundException($"Assessment Tool Workflow Instance with Id {request.WorkflowInstanceId} not found");
            }

            var isLatest = await _assessmentToolWorkflowInstanceHelpers.IsOrderEqualToLatestSubmittedWorkflowOrder(workflowInstance);
            if (!isLatest)
            {
                throw new Exception(
                    $"Unable to create override for Assessment Tool Workflow Instance as this is not the latest submitted Workflow Instance for this Assessment.");
            }

            var activeInterventionsForAssessment = await _assessmentRepository.GetOpenAssessmentInterventions(workflowInstance.AssessmentId);
            if(activeInterventionsForAssessment.Any())
            {
                throw new Exception(
                    $"Unable to create request as an open request already exists for this assessment.");
            }

            var userName = _userProvider.GetUserName()!;
            var email = _userProvider.GetUserEmail()!;

            var dtoConfig = new DtoConfig()
            {
                AdministratorName = userName,
                UserName = userName,
                AdministratorEmail = email,
                UserEmail = email,
                DecisionType = InterventionDecisionTypes.Override,
                Status = InterventionStatus.Pending
            };
            var dto = _mapper.AssessmentInterventionDtoFromWorkflowInstance(workflowInstance, dtoConfig);
            List<AssessmentToolWorkflow> assessmentToolWorkflows =
                await _adminAssessmentToolWorkflowRepository.GetAssessmentToolWorkflowsForOverride(workflowInstance
                    .AssessmentToolWorkflow.AssessmentTool.Order);

            if (assessmentToolWorkflows == null || !assessmentToolWorkflows.Any())
            {
                throw new NotFoundException($"No suitable assessment tool workflows found for override");
            }

            dto.TargetWorkflowDefinitions = _mapper.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(assessmentToolWorkflows);
            return dto;
        }
    }
}
