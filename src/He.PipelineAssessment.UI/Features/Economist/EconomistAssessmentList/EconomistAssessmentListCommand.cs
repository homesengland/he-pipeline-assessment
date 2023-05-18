using He.PipelineAssessment.Models.ViewModels;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Economist.EconomistAssessmentList;

public class EconomistAssessmentListCommand : IRequest<List<AssessmentDataViewModel>>
{
}
