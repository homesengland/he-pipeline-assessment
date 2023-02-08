using He.PipelineAssessment.Models.ViewModels;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessments.AssessmentList
{
    public class AssessmentListCommand : IRequest<List<AssessmentDataViewModel>>
    {
    }
}
