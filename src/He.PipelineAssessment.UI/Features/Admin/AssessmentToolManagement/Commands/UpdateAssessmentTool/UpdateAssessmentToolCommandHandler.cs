using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Exceptions;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentTool
{
    public class UpdateAssessmentToolCommandHandler : IRequestHandler<UpdateAssessmentToolCommand>
    {
        private readonly IAdminAssessmentToolRepository _adminAssessmentToolRepository;
        private readonly ILogger<UpdateAssessmentToolCommandHandler> _logger;

        public UpdateAssessmentToolCommandHandler(IAdminAssessmentToolRepository adminAssessmentToolRepository, ILogger<UpdateAssessmentToolCommandHandler> logger)
        {
            _adminAssessmentToolRepository = adminAssessmentToolRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateAssessmentToolCommand request, CancellationToken cancellationToken)
        {
            try
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
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to update assessment tool. AssessmentToolID: {request.Id}");
            }

        }
    }
}
