using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Intervention;
using MediatR;
using He.PipelineAssessment.UI.Services;

namespace He.PipelineAssessment.UI.Features.Amendment.CreateAmendment
{
    public class CreateAmendmentRequestHandler : IRequestHandler<CreateAmendmentRequest, AssessmentInterventionDto>
    {
        private readonly IInterventionService _interventionService;

        public CreateAmendmentRequestHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }

        public async Task<AssessmentInterventionDto> Handle(CreateAmendmentRequest request, CancellationToken cancellationToken)
        {
            return await _interventionService.CreateInterventionRequest(request);
        }

    }
    
}
