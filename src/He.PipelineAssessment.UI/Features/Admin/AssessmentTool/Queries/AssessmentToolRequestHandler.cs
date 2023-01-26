using He.PipelineAssessment.Infrastructure.Repository;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentTool.Queries
{
    public class AssessmentToolRequestHandler : IRequestHandler<AssessmentToolRequest, AssessmentToolData>
    {
        private readonly IAdminAssessmentToolRepository _adminAssessmentToolRepository;
        private readonly IAssessmentToolMapper _assessmentToolMapper;

        public AssessmentToolRequestHandler(IAdminAssessmentToolRepository adminAssessmentToolRepository, IAssessmentToolMapper assessmentToolMapper)
        {
            _adminAssessmentToolRepository = adminAssessmentToolRepository;
            _assessmentToolMapper = assessmentToolMapper;
        }
        public async Task<AssessmentToolData> Handle(AssessmentToolRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var assessmentTools = await _adminAssessmentToolRepository.GetAssessmentTools();
                var assessmentToolData = _assessmentToolMapper.AssessmentToolsToAssessmentToolData(assessmentTools.ToList());

                return assessmentToolData;
            }
            catch (Exception e)
            {
                List<string> errors = new List<string> { $"An error occurred whilst accessing our data. Exception: {e.Message}" };
                return new AssessmentToolData()
                {
                    ValidationMessages = errors
                };
            }
        }
    }
}
