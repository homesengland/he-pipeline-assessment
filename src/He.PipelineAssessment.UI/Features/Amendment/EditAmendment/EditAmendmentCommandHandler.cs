using Azure.Core;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Amendment.EditAmendment
{
    public class EditAmendmentCommandHandler : IRequestHandler<EditAmendmentCommand, int>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<EditAmendmentCommandHandler> _logger;
        private readonly IRoleValidation _roleValidation;
        private readonly IInterventionService _interventionService;


        public EditAmendmentCommandHandler(IAssessmentRepository assessmentRepository, ILogger<EditAmendmentCommandHandler> logger, IRoleValidation roleValidation, IInterventionService interventionService)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
            _roleValidation = roleValidation;
            _interventionService = interventionService;
        }

        public async Task<int> Handle(EditAmendmentCommand command, CancellationToken cancellationToken)
        {
            //Needs Testing as this may not play nicely with the child class due to nullable values for interventionReasons and not having an admin rationale?
            var id = await _interventionService.EditIntervention(command);
            return id;
        }
    }
}
