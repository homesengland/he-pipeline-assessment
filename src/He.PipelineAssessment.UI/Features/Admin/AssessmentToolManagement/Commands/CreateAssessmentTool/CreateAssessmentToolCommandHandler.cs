using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentTool
{
    public class CreateAssessmentToolCommandHandler : IRequestHandler<CreateAssessmentToolCommand>
    {
        private readonly IAdminAssessmentToolRepository _adminAssessmentToolRepository;
        private readonly IAssessmentToolMapper _assessmentToolMapper;

        public CreateAssessmentToolCommandHandler(IAdminAssessmentToolRepository adminAssessmentToolRepository, IAssessmentToolMapper assessmentToolMapper)
        {
            _adminAssessmentToolRepository = adminAssessmentToolRepository;
            _assessmentToolMapper = assessmentToolMapper;
        }

        public async Task<Unit> Handle(CreateAssessmentToolCommand command, CancellationToken cancellationToken)
        {
            var assessmentTool = _assessmentToolMapper.CreateAssessmentToolCommandToAssessmentTool(command);


            await _adminAssessmentToolRepository.CreateAssessmentTool(assessmentTool);
            return Unit.Value;
        }

    }
}
