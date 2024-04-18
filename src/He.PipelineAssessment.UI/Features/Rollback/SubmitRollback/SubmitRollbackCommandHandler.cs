using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.SubmitRollback
{
    public class SubmitRollbackCommandHandler : IRequestHandler<SubmitRollbackCommand>
    {
        private readonly IInterventionService _interventionService;

        public SubmitRollbackCommandHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }

        public async Task Handle(SubmitRollbackCommand command, CancellationToken cancellationToken)
        {
            await _interventionService.SubmitIntervention(command);
        }
    }
}
