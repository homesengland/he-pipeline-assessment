using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Variation.EditVariationAssessor
{
    public class EditVariationAssessorRequestHandler : IRequestHandler<EditVariationAssessorRequest, AssessmentInterventionDto>
    {

        private readonly IInterventionService _interventionService;

        public EditVariationAssessorRequestHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }
        public async Task<AssessmentInterventionDto> Handle(EditVariationAssessorRequest request, CancellationToken cancellationToken)
        {
            return await _interventionService.EditInterventionAssessorRequest(request);
        }
    }
}
