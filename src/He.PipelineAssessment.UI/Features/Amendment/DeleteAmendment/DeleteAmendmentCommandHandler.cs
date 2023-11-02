using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Amendment.DeleteAmendment
{
    public class DeleteAmendmentCommandHandler : IRequestHandler<DeleteAmendmentCommand, int>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IRoleValidation _roleValidation;
        private readonly ILogger<DeleteAmendmentCommandHandler> _logger;
        private readonly IInterventionService _interventionService;

        public DeleteAmendmentCommandHandler(IInterventionService interventionService, IAssessmentRepository assessmentRepository, IRoleValidation roleValidation, ILogger<DeleteAmendmentCommandHandler> logger)
        {
            _interventionService = interventionService;
            _assessmentRepository = assessmentRepository;
            _roleValidation = roleValidation;
            _logger = logger;
        }

        public async Task<int> Handle(DeleteAmendmentCommand command, CancellationToken cancellationToken)
        {
            return await _interventionService.DeleteIntervention(command);
        }
    }
}
