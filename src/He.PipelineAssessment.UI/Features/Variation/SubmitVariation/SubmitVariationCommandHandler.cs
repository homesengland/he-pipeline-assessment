using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Variation.SubmitVariation
{
    public class SubmitVariationCommandHandler : IRequestHandler<SubmitVariationCommand>
    {
        private readonly IInterventionService _interventionService;

        public SubmitVariationCommandHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }

        public async Task Handle(SubmitVariationCommand command, CancellationToken cancellationToken)
        {
            await _interventionService.SubmitIntervention(command);
        }
    }
}
