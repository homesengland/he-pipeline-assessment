using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Amendment.EditAmendment
{
    public class EditAmendmentRequestHandler : IRequestHandler<EditAmendmentRequest, AssessmentInterventionDto>
    {

        private readonly IAssessmentRepository _repository;
        private readonly IInterventionService _interventionService;

        public EditAmendmentRequestHandler(IAssessmentRepository repo, IInterventionService interventionService)
        {
            _repository = repo;
            _interventionService = interventionService;
        }
        public async Task<AssessmentInterventionDto> Handle(EditAmendmentRequest request, CancellationToken cancellationToken)
        {
            var dto = await _interventionService.EditInterventionRequest(request);
            var interventionReasons = await _repository.GetInterventionReasons();
            dto.InterventionReasons = interventionReasons;
            return dto;
        }
    }
}
