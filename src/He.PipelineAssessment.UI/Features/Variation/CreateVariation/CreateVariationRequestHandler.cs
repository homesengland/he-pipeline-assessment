using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Variation.CreateVariation
{
    public class CreateVariationRequestHandler : IRequestHandler<CreateVariationRequest, AssessmentInterventionDto>
    {

        private readonly IInterventionService _interventionService;

        public CreateVariationRequestHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }

        public async Task<AssessmentInterventionDto> Handle(CreateVariationRequest request, CancellationToken cancellationToken)
        {
            return await _interventionService.CreateInterventionRequest(request);
           
        }
    }
}
