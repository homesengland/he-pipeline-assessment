using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentTool
{
    public class CreateAssessmentToolCommandHandler : IRequestHandler<CreateAssessmentToolCommand>
    {
        private readonly IAdminAssessmentToolRepository _adminAssessmentToolRepository;
        private readonly IAssessmentToolMapper _assessmentToolMapper;
        private readonly ILogger<CreateAssessmentToolCommandHandler> _logger;

        public CreateAssessmentToolCommandHandler(IAdminAssessmentToolRepository adminAssessmentToolRepository, IAssessmentToolMapper assessmentToolMapper, ILogger<CreateAssessmentToolCommandHandler> logger)
        {
            _adminAssessmentToolRepository = adminAssessmentToolRepository;
            _assessmentToolMapper = assessmentToolMapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateAssessmentToolCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var assessmentTool = _assessmentToolMapper.CreateAssessmentToolCommandToAssessmentTool(command);


                await _adminAssessmentToolRepository.CreateAssessmentTool(assessmentTool);
                return Unit.Value;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to create assessment tool.");
            }
        }

    }
}
