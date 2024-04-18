using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Amendment.CreateAmendment
{
    public class CreateAmendmentCommandHandler : IRequestHandler<CreateAmendmentCommand, int>
    {
        private readonly IInterventionService _interventionService;

        public CreateAmendmentCommandHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }



        public async Task<int> Handle(CreateAmendmentCommand command, CancellationToken cancellationToken)
        {
            return await _interventionService.CreateAssessmentIntervention(command);
        }
    }
}
