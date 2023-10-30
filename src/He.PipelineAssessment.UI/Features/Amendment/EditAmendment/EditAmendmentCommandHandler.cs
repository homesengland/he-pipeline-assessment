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
        private readonly IInterventionService _interventionService;


        public EditAmendmentCommandHandler(IInterventionService interventionService)
        {
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
