using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Override.EditOverride
{
    public class EditOverrideCommandHandler : IRequestHandler<EditOverrideCommand, int>
    {
        private readonly IInterventionService _interventionService;

        public EditOverrideCommandHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }

        public async Task<int> Handle(EditOverrideCommand command, CancellationToken cancellationToken)
        {
            return await _interventionService.EditIntervention(command);
        }
    }
}
