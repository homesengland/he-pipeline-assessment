using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Override.CreateOverride
{
    public class CreateOverrideCommandHandler : IRequestHandler<CreateOverrideCommand, int>
    {
        private readonly IInterventionService _interventionService;

        public CreateOverrideCommandHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }


        public async Task<int> Handle(CreateOverrideCommand command, CancellationToken cancellationToken)
        {
            return await _interventionService.CreateAssessmentIntervention(command);
        }
    }
}
