using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Exceptions;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentTool
{
    public class UpdateAssessmentToolCommandHandler : IRequestHandler<UpdateAssessmentToolCommand>
    {
        private readonly IAdminAssessmentToolRepository _adminAssessmentToolRepository;

        public UpdateAssessmentToolCommandHandler(IAdminAssessmentToolRepository adminAssessmentToolRepository)
        {
            _adminAssessmentToolRepository = adminAssessmentToolRepository;
        }

        public async Task<Unit> Handle(UpdateAssessmentToolCommand request, CancellationToken cancellationToken)
        {
            var entity = await _adminAssessmentToolRepository.GetAssessmentToolById(request.Id);
            if (entity == null)
            {
                throw new NotFoundException($"Assessment Tool with Id {request.Id} not found");
            }
            entity.Name = request.Name;
            entity.Order = request.Order;
            await _adminAssessmentToolRepository.UpdateAssessmentTool(entity);
            return Unit.Value;
        }
    }
}
