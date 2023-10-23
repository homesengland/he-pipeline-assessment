using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.EditRollback
{
    public class EditRollbackRequestHandler : IRequestHandler<EditRollbackRequest, AssessmentInterventionDto>
    {
        private readonly IInterventionService _interventionService;

        public EditRollbackRequestHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }
        public async Task<AssessmentInterventionDto> Handle(EditRollbackRequest request, CancellationToken cancellationToken)
        {
            return await _interventionService.EditInterventionRequest(request);

        }
    }
}
