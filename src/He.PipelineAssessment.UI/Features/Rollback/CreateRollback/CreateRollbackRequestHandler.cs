﻿using He.PipelineAssessment.UI.Features.Intervention;
using He.PipelineAssessment.UI.Services;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Rollback.CreateRollback
{
    public class CreateRollbackRequestHandler : IRequestHandler<CreateRollbackRequest, AssessmentInterventionDto>
    {

        private readonly IInterventionService _interventionService;

        public CreateRollbackRequestHandler(IInterventionService interventionService)
        {
            _interventionService = interventionService;
        }

        public async Task<AssessmentInterventionDto> Handle(CreateRollbackRequest request, CancellationToken cancellationToken)
        {
            return await _interventionService.CreateInterventionRequest(request);
           
        }
    }
}
