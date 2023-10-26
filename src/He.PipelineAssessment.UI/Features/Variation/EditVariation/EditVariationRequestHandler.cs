using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Variation.EditVariation
{
    public class EditVariationRequestHandler : IRequestHandler<EditVariationRequest, AssessmentInterventionDto>
    {
        private readonly IInterventionService _interventionService;
        private readonly IAssessmentInterventionMapper _mapper;

        public EditVariationRequestHandler(IInterventionService interventionService, IAssessmentInterventionMapper mapper)
        {
            _interventionService = interventionService;
            _mapper = mapper;
        }
        public async Task<AssessmentInterventionDto> Handle(EditVariationRequest request, CancellationToken cancellationToken)
        {
            var dto = await _interventionService.EditInterventionRequest(request);
            var assessmentToolWorkflows = await
                _interventionService.GetAssessmentToolWorkflowsForVariation(dto.AssessmentInterventionCommand.WorkflowInstanceId);
            dto.TargetWorkflowDefinitions = _mapper.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(assessmentToolWorkflows);
            return dto;
        }
    }
}
