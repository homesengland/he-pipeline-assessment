using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.ConfirmRollback
{
    public class ConfirmRollbackCommandHandler : IRequestHandler<ConfirmRollbackCommand>
    {
        private readonly IInterventionService _interventionService;

        public ConfirmRollbackCommandHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }

        public async Task Handle(ConfirmRollbackCommand command, CancellationToken cancellationToken)
        {
            await _interventionService.ConfirmIntervention(command);   
        }
    }
}
