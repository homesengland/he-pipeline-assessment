using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.EditRollbackAssessor
{
    public class EditRollbackAssessorCommandHandler : IRequestHandler<EditRollbackAssessorCommand, int>
    {
        private readonly IInterventionService _interventionService;

        public EditRollbackAssessorCommandHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }

        public async Task<int> Handle(EditRollbackAssessorCommand command, CancellationToken cancellationToken)
        {
            return await _interventionService.EditInterventionAssessor(command);

        }
    }
}
