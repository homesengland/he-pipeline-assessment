using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentList;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentTool
{
    //public class AssessmentToolRequestHandler : IRequestHandler<AssessmentToolRequest, AssessmentToolData>
    //{
    //    private IAdminAssessmentToolRepository _adminAssessmentToolRepository;

    //    public AssessmentToolRequestHandler(IAdminAssessmentToolRepository adminAssessmentToolRepository)
    //    {
    //        _adminAssessmentToolRepository = adminAssessmentToolRepository;
    //    }
    //    //public async Task<AssessmentToolData> Handle(AssessmentToolRequest request, CancellationToken cancellationToken)
    //    //{
    //    //    //try
    //    //    //{
    //    //    //    var assessmentTools = await _adminAssessmentToolRepository.GetAssessmentTools();
    //    //    //    var assessmentListData = GetAssessmentListFromResults(listOfAssessments);

    //    //    //    return assessmentListData;
    //    //    //}
    //    //    //catch (Exception e)
    //    //    //{
    //    //    //    List<string> errors = new List<string> { $"An error occured whilst accessing our data. Exception: {e.Message}" };
    //    //    //    return new AssessmentListData
    //    //    //    {
    //    //    //        ValidationMessages = errors
    //    //    //    };
    //    //    //}

    //    //}
    //}
}
