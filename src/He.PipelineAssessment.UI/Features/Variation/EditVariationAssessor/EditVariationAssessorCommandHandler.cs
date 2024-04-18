using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Variation.EditVariationAssessor
{
    public class EditVariationAssessorCommandHandler : IRequestHandler<EditVariationAssessorCommand, int>
    {
        private readonly IInterventionService _interventionService;

        public EditVariationAssessorCommandHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }

        public async Task<int> Handle(EditVariationAssessorCommand command, CancellationToken cancellationToken)
        {
            return await _interventionService.EditInterventionAssessor(command);

        }
    }
}
