using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Variation.ConfirmVariation
{
    public class ConfirmVariationCommandHandler : IRequestHandler<ConfirmVariationCommand>
    {
        private readonly IInterventionService _interventionService;

        public ConfirmVariationCommandHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }

        public async Task Handle(ConfirmVariationCommand command, CancellationToken cancellationToken)
        {
            await _interventionService.ConfirmIntervention(command);   
        }
    }
}
