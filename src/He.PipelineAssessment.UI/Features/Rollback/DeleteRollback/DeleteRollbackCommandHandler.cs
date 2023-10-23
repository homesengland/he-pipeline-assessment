using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.DeleteRollback
{
    public class DeleteRollbackCommandHandler : IRequestHandler<DeleteRollbackCommand, int>
    {
        private readonly IInterventionService _interventionService;

        public DeleteRollbackCommandHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }

        public async Task<int> Handle(DeleteRollbackCommand command, CancellationToken cancellationToken)
        {
            return await _interventionService.DeleteIntervention(command);
            

        }
    }
}
