using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentToolWorkflows
{
    public class AssessmentToolWorkflowRequestHandler : IRequestHandler<AssessmentToolWorkflowQuery, AssessmentToolWorkflowListDto>
    {
        private readonly IAdminAssessmentToolRepository _adminAssessmentToolRepository;
        private readonly IAssessmentToolMapper _assessmentToolMapper;

        public AssessmentToolWorkflowRequestHandler(IAdminAssessmentToolRepository adminAssessmentToolRepository, IAssessmentToolMapper assessmentToolMapper)
        {
            _adminAssessmentToolRepository = adminAssessmentToolRepository;
            _assessmentToolMapper = assessmentToolMapper;
        }

        public async Task<AssessmentToolWorkflowListDto> Handle(AssessmentToolWorkflowQuery query, CancellationToken cancellationToken)
        {
            var entity = await _adminAssessmentToolRepository.GetAssessmentToolById(query.AssessmentToolId);
            ArgumentNullException.ThrowIfNull(entity, "Assessment not found");
            var assessmentToolWorkflowListDto = new AssessmentToolWorkflowListDto
            {
                AssessmentToolId = entity.Id,
                AssessmentToolName = entity.Name
            };

            if (entity.AssessmentToolWorkflows != null)
            {
                assessmentToolWorkflowListDto.AssessmentToolWorkflowDtos = _assessmentToolMapper.AssessmentToolWorkflowsToAssessmentToolDto(entity.AssessmentToolWorkflows.ToList());

            }

            return assessmentToolWorkflowListDto;
        }
    }
}
