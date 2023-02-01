using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentTools
{
    public class AssessmentToolRequestHandler : IRequestHandler<AssessmentToolRequest, AssessmentToolListData>
    {
        private readonly IAdminAssessmentToolRepository _adminAssessmentToolRepository;
        private readonly IAssessmentToolMapper _assessmentToolMapper;

        public AssessmentToolRequestHandler(IAdminAssessmentToolRepository adminAssessmentToolRepository, IAssessmentToolMapper assessmentToolMapper)
        {
            _adminAssessmentToolRepository = adminAssessmentToolRepository;
            _assessmentToolMapper = assessmentToolMapper;
        }

        public async Task<AssessmentToolListData> Handle(AssessmentToolRequest request,
            CancellationToken cancellationToken)
        {
            var assessmentTools = await _adminAssessmentToolRepository.GetAssessmentTools();
            var assessmentToolData =
                _assessmentToolMapper.AssessmentToolsToAssessmentToolData(assessmentTools.ToList());

            return assessmentToolData;
        }
    }
}
