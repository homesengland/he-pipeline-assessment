using He.PipelineAssessment.Models.ViewModels;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Economist.EconomistAssessmentList;

public class EconomistAssessmentListRequest : IRequest<List<AssessmentDataViewModel>>
{
    public bool CanViewSensitiveRecords { get; set; }
    public string? Username { get; set; }
}
