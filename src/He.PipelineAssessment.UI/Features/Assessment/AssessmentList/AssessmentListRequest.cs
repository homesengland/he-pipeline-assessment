using He.PipelineAssessment.Models.ViewModels;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessment.AssessmentList
{
    public class AssessmentListRequest : IRequest<List<AssessmentDataViewModel>>
    {
        public string? Username { get; set; }
        public bool CanViewSensitiveRecords { get; set; }
        public bool IsAdmin { get; set; } // Support admin filtering
    }
}
