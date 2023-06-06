using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateRollback
{
    public class CreateRollbackRequestHandler : IRequestHandler<CreateRollbackRequest, AssessmentInterventionDto>
    {

        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IUserProvider _userProvider;
        private readonly IAssessmentInterventionMapper _mapper;
        private readonly ILogger<CreateRollbackRequestHandler> _logger;

        public CreateRollbackRequestHandler(IAssessmentRepository assessmentRepository, 
            IUserProvider userProvider, 
            IAssessmentInterventionMapper mapper, 
            ILogger<CreateRollbackRequestHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _userProvider = userProvider;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AssessmentInterventionDto> Handle(CreateRollbackRequest request, CancellationToken cancellationToken)
        {
            AssessmentToolWorkflowInstance? workflowInstance = await _assessmentRepository.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId);
            if (workflowInstance == null)
            {
                throw new NotFoundException($"Assessment Tool Workflow Instance with Id {request.WorkflowInstanceId} not found");
            }

            var dto = _mapper.AssessmentInterventionDtoFromWorkflowInstance(workflowInstance, _userProvider.GetUserName()!, _userProvider.GetUserEmail()!, InterventionDecisionTypes.Rollback);

            return dto;
        }
    }
}
