﻿using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Exceptions;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.DeleteAssessmentTool
{
    public class DeleteAssessmentToolCommandHandler : IRequestHandler<DeleteAssessmentToolCommand>
    {
        private readonly IAdminAssessmentToolRepository _adminAssessmentToolRepository;

        public DeleteAssessmentToolCommandHandler(IAdminAssessmentToolRepository adminAssessmentToolRepository)
        {
            _adminAssessmentToolRepository = adminAssessmentToolRepository;
        }

        public async Task<Unit> Handle(DeleteAssessmentToolCommand request, CancellationToken cancellationToken)
        {
            var entity = await _adminAssessmentToolRepository.GetAssessmentToolById(request.Id);
            if (entity == null)
            {
                throw new NotFoundException($"Assessment Tool with Id {request.Id} not found");
            }

            await _adminAssessmentToolRepository.DeleteAssessmentTool(entity);
            return Unit.Value;
        }
    }
}
