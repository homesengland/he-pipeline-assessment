using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.CreateRollback
{
    public class CreateRollbackCommandHandler : IRequestHandler<CreateRollbackCommand, int>
    {
        private readonly IInterventionService _interventionService;

        public CreateRollbackCommandHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }


        public async Task<int> Handle(CreateRollbackCommand command,  CancellationToken cancellationToken)
        {
            return await _interventionService.CreateAssessmentIntervention(command);
                      
        }
    }
}
