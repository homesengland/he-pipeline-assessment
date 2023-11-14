using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Models.ViewModels;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Economist.EconomistAssessmentList
{
    public class EconomistAssessmentListRequestHandler : IRequestHandler<EconomistAssessmentListRequest, List<AssessmentDataViewModel>>
    {
        private readonly IStoredProcedureRepository _storedProcedureRepository;
        private readonly ILogger<EconomistAssessmentListRequestHandler> _logger;

        public EconomistAssessmentListRequestHandler(IStoredProcedureRepository storedProcedureRepository, ILogger<EconomistAssessmentListRequestHandler> logger)
        {
            _storedProcedureRepository = storedProcedureRepository;
            _logger = logger;
        }
        public async Task<List<AssessmentDataViewModel>> Handle(EconomistAssessmentListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var dbAssessments = await _storedProcedureRepository.GetEconomistAssessments();

                var filteredAssessments = dbAssessments.Where(x =>
                    !x.IsSensitiveRecord() || (x.IsSensitiveRecord() &&
                                               (request.CanSeeSensitiveRecords ||
                                                request.Username == x.ProjectManager)));

                return filteredAssessments.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ApplicationException($"Unable to get list of assessments for economists.");
            }

        }
    }
}
