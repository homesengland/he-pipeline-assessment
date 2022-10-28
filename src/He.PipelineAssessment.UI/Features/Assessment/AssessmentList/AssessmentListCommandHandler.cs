using He.PipelineAssessment.UI.Features.Assessments.AssessmentSummary;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessments.AssessmentList
{
    public class AssessmentListCommandHandler : IRequestHandler<AssessmentListCommand, AssessmentListData>
    {
        private MockAssessmentDataService _dataService;

        public AssessmentListCommandHandler()
        {
            _dataService = new MockAssessmentDataService();
        }
        public Task<AssessmentListData> Handle(AssessmentListCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_dataService.GetData(request));
        }
    }
}
