using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Utility;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentTool
{
    public class CreateAssessmentToolCommandHandler : IRequestHandler<CreateAssessmentToolCommand, CreateAssessmentToolData>
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
        public async Task<CreateAssessmentToolData> Handle(CreateAssessmentToolCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var assessmenttool = _assessmentToolMapper.CreateAssessmentToolDtoToAssessmentTool(command.AssessmentToolDto);
                var utcNow = _dateTimeProvider.UtcNow();

                assessmenttool.CreatedDate = utcNow;
                assessmenttool.LastModified = utcNow;

                await _adminAssessmentToolRepository.CreateAssessmentTool(assessmenttool);

                command.AssessmentToolDto.Id = assessmenttool.Id;
                return new CreateAssessmentToolData
                {
                    AssessmentToolDto = command.AssessmentToolDto

                };

            }

            catch (Exception ex)
            {
                List<string> errors = new List<string> { $"An error occured whilst accessing our data. Exception: {ex.Message}" };
                return new CreateAssessmentToolData
                {
                    ValidationMessages = errors
                };

            }


        }

    }
}
