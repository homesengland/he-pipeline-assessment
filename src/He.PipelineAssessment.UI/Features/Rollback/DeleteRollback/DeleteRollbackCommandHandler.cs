using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.DeleteRollback
{
    public class DeleteRollbackCommandHandler : IRequestHandler<DeleteRollbackCommand, int>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IRoleValidation _roleValidation;
        private readonly ILogger<DeleteRollbackCommandHandler> _logger;
        private readonly IInterventionService _interventionService;

        public DeleteRollbackCommandHandler(IAssessmentRepository assessmentRepository, IInterventionService interventionService, IRoleValidation roleValidation, ILogger<DeleteRollbackCommandHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _roleValidation = roleValidation;
            _logger = logger;
            _interventionService = interventionService;
        }

        public async Task<int> Handle(DeleteRollbackCommand command, CancellationToken cancellationToken)
        {
            return await _interventionService.DeleteIntervention(command);
            

        }
    }
}
