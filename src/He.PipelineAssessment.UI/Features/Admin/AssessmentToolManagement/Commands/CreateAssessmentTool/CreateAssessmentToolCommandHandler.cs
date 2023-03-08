using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentTool
{
    public class CreateAssessmentToolCommandHandler : IRequestHandler<CreateAssessmentToolCommand>
    {
        private readonly IAdminAssessmentToolRepository _adminAssessmentToolRepository;
        private readonly IAssessmentToolMapper _assessmentToolMapper;
        private readonly IDateTimeProvider _dateTimeProvider;


        public CreateAssessmentToolCommandHandler(IAdminAssessmentToolRepository adminAssessmentToolRepository, IAssessmentToolMapper assessmentToolMapper,
                                                     IDateTimeProvider dateTimeProvider)
        {
            _adminAssessmentToolRepository = adminAssessmentToolRepository;
            _assessmentToolMapper = assessmentToolMapper;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<Unit> Handle(CreateAssessmentToolCommand command, CancellationToken cancellationToken)
        {
            var assessmentTool = _assessmentToolMapper.CreateAssessmentToolCommandToAssessmentTool(command);
            var utcNow = _dateTimeProvider.UtcNow();

            assessmentTool.CreatedDate = utcNow;
            assessmentTool.LastModifiedDate = utcNow;

            await _adminAssessmentToolRepository.CreateAssessmentTool(assessmentTool);
            return Unit.Value;
        }

    }
}
