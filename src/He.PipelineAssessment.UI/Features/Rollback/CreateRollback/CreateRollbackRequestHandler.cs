using Auth0.ManagementApi.Models;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Intervention;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.CreateRollback
{
    public class CreateRollbackRequestHandler : IRequestHandler<CreateRollbackRequest, AssessmentInterventionDto>
    {

        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IUserProvider _userProvider;
        private readonly IAssessmentInterventionMapper _mapper;
        private readonly ILogger<CreateRollbackRequestHandler> _logger;
        private readonly IAssessmentToolWorkflowInstanceHelpers _assessmentToolWorkflowInstanceHelpers;

        public CreateRollbackRequestHandler(IAssessmentRepository assessmentRepository,
            IUserProvider userProvider,
            IAssessmentInterventionMapper mapper,
            ILogger<CreateRollbackRequestHandler> logger, IAssessmentToolWorkflowInstanceHelpers assessmentToolWorkflowInstanceHelpers)
        {
            _assessmentRepository = assessmentRepository;
            _userProvider = userProvider;
            _mapper = mapper;
            _logger = logger;
            _assessmentToolWorkflowInstanceHelpers = assessmentToolWorkflowInstanceHelpers;
        }

        public async Task<AssessmentInterventionDto> Handle(CreateRollbackRequest request, CancellationToken cancellationToken)
        {
            AssessmentToolWorkflowInstance? workflowInstance = await _assessmentRepository.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId);
            if (workflowInstance == null)
            {
                throw new NotFoundException($"Assessment Tool Workflow Instance with Id {request.WorkflowInstanceId} not found");
            }

            var isLatest = _assessmentToolWorkflowInstanceHelpers.IsLatestSubmittedWorkflow(workflowInstance);
            if (!isLatest)
            {
                throw new Exception(
                    $"Unable to create rollback for Assessment Tool Workflow Instance as this is not the latest submitted Workflow Instance for this Assessment.");
            }

            var userName = _userProvider.GetUserName()!;
            var email = _userProvider.GetUserEmail()!;

            var dtoConfig = new DtoConfig()
            {
                UserName = userName,
                UserEmail = email,
                DecisionType = InterventionDecisionTypes.Rollback,
                Status = InterventionStatus.Draft
            };

            var dto = _mapper.AssessmentInterventionDtoFromWorkflowInstance(workflowInstance, dtoConfig);

            return dto;
        }
    }
}
