using He.PipelineAssessment.Models.ViewModels;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessment.AssessmentList
{
    public class AssessmentListCommand : IRequest<List<AssessmentDataViewModel>>
    {
    }
}
