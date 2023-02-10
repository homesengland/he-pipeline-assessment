using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.UI.Features.Assessments.AssessmentList;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessment.AssessmentList
{
    public class AssessmentListCommandHandler : IRequestHandler<AssessmentListCommand, List<AssessmentDataViewModel>>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IStoredProcedureRepository _storedProcedureRepository;
        private readonly ILogger<AssessmentListCommandHandler> _logger;

        public AssessmentListCommandHandler(IAssessmentRepository repository, IStoredProcedureRepository storedProcedureRepository, ILogger<AssessmentListCommandHandler> logger)
        {
            _assessmentRepository = repository;
            _storedProcedureRepository = storedProcedureRepository;
            _logger = logger;
        }
        public async Task<List<AssessmentDataViewModel>> Handle(AssessmentListCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var dbAssessment = await _storedProcedureRepository.GetAssessments();

                return dbAssessment;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);

            }
            return new List<AssessmentDataViewModel>();
        }
    }
}
