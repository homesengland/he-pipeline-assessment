using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Models.ViewModels;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessment.AssessmentList
{
    public class AssessmentListRequestHandler : IRequestHandler<AssessmentListRequest, List<AssessmentDataViewModel>>
    {
        private readonly IStoredProcedureRepository _storedProcedureRepository;
        private readonly ILogger<AssessmentListRequestHandler> _logger;

        public AssessmentListRequestHandler(IStoredProcedureRepository storedProcedureRepository, ILogger<AssessmentListRequestHandler> logger)
        {
            _storedProcedureRepository = storedProcedureRepository;
            _logger = logger;
        }
        public async Task<List<AssessmentDataViewModel>> Handle(AssessmentListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var dbAssessments = await _storedProcedureRepository.GetAssessments();

                var filteredAssessments = dbAssessments.Where(x =>
                    !x.IsSensitiveRecord() || (x.IsSensitiveRecord() &&
                                               (request.CanViewSensitiveRecords ||
                                                request.Username == x.ProjectManager)));

                return filteredAssessments.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException("Unable to get list of assessments.");
            }
        }
    }
}
