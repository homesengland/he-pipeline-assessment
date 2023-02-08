using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Infrastructure.Repository.StoreProc;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.UI.Features.Assessments.AssessmentList;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessment.AssessmentList
{
    public class AssessmentListCommandHandler : IRequestHandler<AssessmentListCommand, List<AssessmentDataViewModel>>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IStoreProcRepository _storeProcRepository;
        private readonly ILogger<AssessmentListCommandHandler> _logger;

        public AssessmentListCommandHandler(IAssessmentRepository repository, IStoreProcRepository storeProcRepository, ILogger<AssessmentListCommandHandler> logger)
        {
            _assessmentRepository = repository;
            _storeProcRepository = storeProcRepository;
            _logger = logger;
        }
        public async Task<List<AssessmentDataViewModel>> Handle(AssessmentListCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbAssessment = await _storeProcRepository.GetAssessments();
                //var listOfAssessments = await _assessmentRepository.GetAssessments();
                //var assessmentListData = GetAssessmentListFromResults(listOfAssessments);

                return dbAssessment;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

            }
            return new List<AssessmentDataViewModel>();
        }

        //private AssessmentListData GetAssessmentListFromResults(List<Models.Assessment> listOfAssessments)
        //{
        //    AssessmentListData assessmentLandingPageData = new AssessmentListData();
        //    foreach (var assessment in listOfAssessments)
        //    {
        //        var assessmentDisplay = new AssessmentDisplay(assessment);
        //        assessmentDisplay.CreatedDate = DateTime.UtcNow;
        //        assessmentDisplay.LastModified = DateTime.UtcNow;
        //        assessmentDisplay.AssessmentWorkflowId = Guid.NewGuid().ToString();
        //        assessmentLandingPageData.ListOfAssessments.Add(assessmentDisplay);
        //    }
        //    return assessmentLandingPageData;
        //}




    }
}
