using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Override.EditOverride
{
    public class EditOverrideRequestHandler : IRequestHandler<EditOverrideRequest, AssessmentInterventionDto>
    {
        private readonly IInterventionService _interventionService;
        private readonly IAssessmentInterventionMapper _mapper;

        public EditOverrideRequestHandler(IInterventionService interventionService, IAssessmentInterventionMapper mapper)
        {
            _interventionService = interventionService;
            _mapper = mapper;
        }
        public async Task<AssessmentInterventionDto> Handle(EditOverrideRequest request, CancellationToken cancellationToken)
        {
            var dto = await _interventionService.EditInterventionRequest(request);
            var assessmentToolWorkflows = await
                _interventionService.GetAssessmentToolWorkflowsForOverride(dto.AssessmentInterventionCommand.WorkflowInstanceId);
            dto.TargetWorkflowDefinitions = _mapper.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(assessmentToolWorkflows);
            return dto;
        }
    }
}
