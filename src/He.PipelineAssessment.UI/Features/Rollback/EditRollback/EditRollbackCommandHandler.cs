using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.EditRollback
{
    public class EditRollbackCommandHandler : IRequestHandler<EditRollbackCommand, int>
    {
        private readonly IInterventionService _interventionService;

        public EditRollbackCommandHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }

        public async Task<int> Handle(EditRollbackCommand command, CancellationToken cancellationToken)
        {
            return await _interventionService.EditIntervention(command);
        }
    }
}
