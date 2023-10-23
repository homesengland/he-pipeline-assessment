using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Override.CreateOverride
{
    public class CreateOverrideRequestHandler : IRequestHandler<CreateOverrideRequest, AssessmentInterventionDto>
    {
        private readonly IInterventionService _interventionService;
        private readonly IAssessmentInterventionMapper _mapper;

        public CreateOverrideRequestHandler(IInterventionService interventionService, IAssessmentInterventionMapper mapper)
        {
            _interventionService = interventionService;
            _mapper = mapper;
        }

        public async Task<AssessmentInterventionDto> Handle(CreateOverrideRequest request, CancellationToken cancellationToken)
        {
            var dto = await _interventionService.CreateInterventionRequest(request);
            var assessmentToolWorkflows = await
                _interventionService.GetAssessmentToolWorkflowsForOverride(request.WorkflowInstanceId);
            dto.TargetWorkflowDefinitions = _mapper.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(assessmentToolWorkflows);

            return dto;
        }
    }
}
