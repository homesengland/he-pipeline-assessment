using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Models.ViewModels;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionList
{
    public class InterventionListRequestHandler : IRequestHandler<InterventionListRequest, List<AssessmentInterventionViewModel>>
    {
        private readonly IStoredProcedureRepository _storedProcedureRepository;
        private readonly ILogger<InterventionListRequestHandler> _logger;

        public InterventionListRequestHandler(IStoredProcedureRepository storedProcedureRepository, ILogger<InterventionListRequestHandler> logger)
        {
            _storedProcedureRepository = storedProcedureRepository;
            _logger = logger;
        }
        public async Task<List<AssessmentInterventionViewModel>> Handle(InterventionListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var interventionList = await _storedProcedureRepository.GetInterventionList();

                var filteredInterventions = interventionList.Where(x =>
                    !x.IsSensitiveRecord() || (x.IsSensitiveRecord() &&
                                               (request.CanViewSensitiveRecords ||
                                                request.Username == x.ProjectManager)));

                return filteredInterventions.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

            }
            return new List<AssessmentInterventionViewModel>();
        }
    }
}
