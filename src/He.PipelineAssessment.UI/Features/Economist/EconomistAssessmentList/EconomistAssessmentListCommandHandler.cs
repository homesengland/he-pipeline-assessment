using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Models.ViewModels;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Economist.EconomistAssessmentList
{
    public class EconomistAssessmentListCommandHandler : IRequestHandler<EconomistAssessmentListCommand, List<AssessmentDataViewModel>>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IStoredProcedureRepository _storedProcedureRepository;
        private readonly ILogger<EconomistAssessmentListCommandHandler> _logger;

        public EconomistAssessmentListCommandHandler(IAssessmentRepository repository, IStoredProcedureRepository storedProcedureRepository, ILogger<EconomistAssessmentListCommandHandler> logger)
        {
            _assessmentRepository = repository;
            _storedProcedureRepository = storedProcedureRepository;
            _logger = logger;
        }
        public async Task<List<AssessmentDataViewModel>> Handle(EconomistAssessmentListCommand request, CancellationToken cancellationToken)
        {
            var dbAssessment = await _storedProcedureRepository.GetEconomistAssessments();

            return dbAssessment;

        }
    }
}
