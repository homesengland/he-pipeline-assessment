using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.EditRollback
{
    public class EditRollbackRequestHandler : IRequestHandler<EditRollbackRequest, AssessmentInterventionDto>
    {
        private readonly IInterventionService _interventionService;
        private readonly IAssessmentInterventionMapper _mapper;

        public EditRollbackRequestHandler(IInterventionService interventionService, IAssessmentInterventionMapper mapper)
        {
            _interventionService = interventionService;
            _mapper = mapper;
        }
        public async Task<AssessmentInterventionDto> Handle(EditRollbackRequest request, CancellationToken cancellationToken)
        {
            var dto = await _interventionService.EditInterventionRequest(request);
            var assessmentToolWorkflows = await
                _interventionService.GetAssessmentToolWorkflowsForRollback(dto.AssessmentInterventionCommand.WorkflowInstanceId);
            dto.TargetWorkflowDefinitions = _mapper.TargetWorkflowDefinitionsFromAssessmentToolWorkflows(assessmentToolWorkflows, dto.AssessmentInterventionCommand.SelectedWorkflowDefinitions);
            return dto;
        }
    }
}
