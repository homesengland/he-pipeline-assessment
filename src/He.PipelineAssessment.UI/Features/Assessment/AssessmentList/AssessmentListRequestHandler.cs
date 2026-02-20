using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Models.ViewModels;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessment.AssessmentList
{
    public class AssessmentListRequestHandler : IRequestHandler<AssessmentListRequest, List<AssessmentDataViewModel>>
    {
        private readonly IStoredProcedureRepository _storedProcedureRepository;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IUserProvider _userProvider;
        private readonly ILogger<AssessmentListRequestHandler> _logger;

        public AssessmentListRequestHandler(
            IStoredProcedureRepository storedProcedureRepository,
            IAssessmentRepository assessmentRepository,
            IUserProvider userProvider,
            ILogger<AssessmentListRequestHandler> logger)
        {
            _storedProcedureRepository = storedProcedureRepository;
            _assessmentRepository = assessmentRepository;
            _userProvider = userProvider;
            _logger = logger;
        }

        public async Task<List<AssessmentDataViewModel>> Handle(AssessmentListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var dbAssessments = await _storedProcedureRepository.GetAssessments();

                var whitelistedAssessmentIds = await GetWhitelistedAssessmentIds();

                var filteredAssessments = dbAssessments.Where(x =>
                    !x.IsSensitiveRecord() ||
                    (x.IsSensitiveRecord() && (
                        request.CanViewSensitiveRecords ||
                        request.Username == x.ProjectManager ||
                        whitelistedAssessmentIds.Contains(x.Id)
                    )));

                return filteredAssessments.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException("Unable to get list of assessments.");
            }
        }

        private async Task<HashSet<int>> GetWhitelistedAssessmentIds()
        {
            var userEmail = _userProvider.Email();

            if (string.IsNullOrWhiteSpace(userEmail))
            {
                return new HashSet<int>();
            }

            var allWhitelists = await _assessmentRepository.GetAllWhitelistedAssessmentIdsByEmail(userEmail);

            return allWhitelists.ToHashSet();
        }
    }
}