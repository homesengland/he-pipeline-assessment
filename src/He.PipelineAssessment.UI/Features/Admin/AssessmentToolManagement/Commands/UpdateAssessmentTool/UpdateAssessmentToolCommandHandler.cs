using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Utility;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentTool
{
    public class UpdateAssessmentToolCommandHandler : IRequestHandler<UpdateAssessmentToolCommand>
    {
        private readonly IAdminAssessmentToolRepository _adminAssessmentToolRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public UpdateAssessmentToolCommandHandler(IAdminAssessmentToolRepository adminAssessmentToolRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _adminAssessmentToolRepository = adminAssessmentToolRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(UpdateAssessmentToolCommand request, CancellationToken cancellationToken)
        {
            var entity = await _adminAssessmentToolRepository.GetAssessmentToolById(request.Id);
            if (entity == null)
            {
                throw new NotImplementedException();
            }
            entity.Name = request.Name;
            entity.Order = request.Order;
            entity.LastModified = _dateTimeProvider.UtcNow();
            await _adminAssessmentToolRepository.UpdateAssessmentTool(entity);
            return Unit.Value;
        }
    }
}
