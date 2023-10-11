using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.ConfirmRollback
{
    public class ConfirmRollbackCommandHandler : IRequestHandler<ConfirmRollbackCommand>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<ConfirmRollbackCommandHandler> _logger;
        private readonly IInterventionService _interventionService;

        public ConfirmRollbackCommandHandler(IAssessmentRepository assessmentRepository, IInterventionService interventionService, ILogger<ConfirmRollbackCommandHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
            _interventionService = interventionService;
        }

        public async Task Handle(ConfirmRollbackCommand command, CancellationToken cancellationToken)
        {
            await _interventionService.ConfirmIntervention(command);   
        }
    }
}
