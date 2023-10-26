using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Variation.EditVariation
{
    public class EditVariationCommandHandler : IRequestHandler<EditVariationCommand, int>
    {
        private readonly IInterventionService _interventionService;

        public EditVariationCommandHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }

        public async Task<int> Handle(EditVariationCommand command, CancellationToken cancellationToken)
        {
            return await _interventionService.EditIntervention(command);
        }
    }
}
