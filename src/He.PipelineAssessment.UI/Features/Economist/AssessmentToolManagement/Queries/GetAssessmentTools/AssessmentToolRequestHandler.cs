using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Economist.AssessmentToolManagement.Mappers;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Economist.AssessmentToolManagement.Queries.GetAssessmentTools
{
    public class AssessmentToolRequestHandler : IRequestHandler<AssessmentToolQuery, AssessmentToolListData>
    {
        private readonly IEconomistAssessmentToolRepository _economistAssessmentToolRepository;
        private readonly IEconomistAssessmentToolMapper _economistAssessmentToolMapper;

        public AssessmentToolRequestHandler(IEconomistAssessmentToolRepository economistAssessmentToolRepository, IEconomistAssessmentToolMapper economistAssessmentToolMapper)
        {
            _economistAssessmentToolRepository = economistAssessmentToolRepository;
            _economistAssessmentToolMapper = economistAssessmentToolMapper;
        }
        public async Task<AssessmentToolListData> Handle(AssessmentToolQuery request, CancellationToken cancellationToken)
        {
            var assessmentTools = await _economistAssessmentToolRepository.GetAssessmentTools();
            var assessmentToolData =
                _economistAssessmentToolMapper.AssessmentToolsToAssessmentToolData(assessmentTools.ToList());

            return assessmentToolData;
        }
    }
}
