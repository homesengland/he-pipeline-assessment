using He.PipelineAssessment.Infrastructure.Repository;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentToolWorkflows
{
    public class AssessmentToolWorkflowRequestHandler : IRequestHandler<AssessmentToolWorkflowQuery, List<AssessmentToolWorkflowDto>>
    {
        private readonly IAdminAssessmentToolRepository _adminAssessmentToolRepository;
        private readonly IAssessmentToolMapper _assessmentToolMapper;

        public AssessmentToolWorkflowRequestHandler(IAdminAssessmentToolRepository adminAssessmentToolRepository, IAssessmentToolMapper assessmentToolMapper)
        {
            _adminAssessmentToolRepository = adminAssessmentToolRepository;
            _assessmentToolMapper = assessmentToolMapper;
        }

        public async Task<List<AssessmentToolWorkflowDto>> Handle(AssessmentToolWorkflowQuery query, CancellationToken cancellationToken)
        {
            var assessmentToolWorkflows = await _adminAssessmentToolRepository.GetAssessmentToolWorkflows(query.AssessmentToolId);
            return _assessmentToolMapper.AssessmentToolWorkflowsToAssessmentToolDto(assessmentToolWorkflows.ToList());
        }
    }
}
