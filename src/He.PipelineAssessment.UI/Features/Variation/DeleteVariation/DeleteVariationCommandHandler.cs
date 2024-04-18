using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Variation.DeleteVariation
{
    public class DeleteVariationCommandHandler : IRequestHandler<DeleteVariationCommand, int>
    {
        private readonly IInterventionService _interventionService;

        public DeleteVariationCommandHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }

        public async Task<int> Handle(DeleteVariationCommand command, CancellationToken cancellationToken)
        {
            return await _interventionService.DeleteIntervention(command);
            

        }
    }
}
