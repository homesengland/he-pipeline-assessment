using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Override.SubmitOverride
{
    public class SubmitOverrideCommandHandler : IRequestHandler<SubmitOverrideCommand>
    {
        private readonly IInterventionService _interventionService;

        public SubmitOverrideCommandHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }

        public async Task Handle(SubmitOverrideCommand command, CancellationToken cancellationToken)
        {
            await _interventionService.SubmitIntervention(command);
        }
    }
}
