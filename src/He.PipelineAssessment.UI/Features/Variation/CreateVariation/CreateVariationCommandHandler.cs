using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Variation.CreateVariation
{
    public class CreateVariationCommandHandler : IRequestHandler<CreateVariationCommand, int>
    {
        private readonly IInterventionService _interventionService;

        public CreateVariationCommandHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }


        public async Task<int> Handle(CreateVariationCommand command,  CancellationToken cancellationToken)
        {
            return await _interventionService.CreateAssessmentIntervention(command);
                      
        }
    }
}
