using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.EditRollbackAssessor
{
    public class EditRollbackAssessorRequestHandler : IRequestHandler<EditRollbackAssessorRequest, AssessmentInterventionDto>
    {

        private readonly IInterventionService _interventionService;

        public EditRollbackAssessorRequestHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }
        public async Task<AssessmentInterventionDto> Handle(EditRollbackAssessorRequest request, CancellationToken cancellationToken)
        {
            return await _interventionService.EditInterventionAssessorRequest(request);
        }
    }
}
