using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessments.AssessmentSummary
{
    public class AssessmentSummaryCommandHandler : IRequestHandler<AssessmentSummaryCommand, AssessmentSummaryData>
    {
        private MockAssessmentDataService _dataService;

        public AssessmentSummaryCommandHandler()
        {
            _dataService = new MockAssessmentDataService();
        }
        public Task<AssessmentSummaryData> Handle(AssessmentSummaryCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(_dataService.GetData(request));
        }
    }
}
