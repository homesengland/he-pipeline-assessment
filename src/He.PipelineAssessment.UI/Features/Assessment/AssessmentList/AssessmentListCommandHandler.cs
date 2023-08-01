using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Models.ViewModels;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessment.AssessmentList
{
    public class AssessmentListCommandHandler : IRequestHandler<AssessmentListCommand, List<AssessmentDataViewModel>>
    {
        private readonly IStoredProcedureRepository _storedProcedureRepository;
        private readonly ILogger<AssessmentListCommandHandler> _logger;

        public AssessmentListCommandHandler(IStoredProcedureRepository storedProcedureRepository, ILogger<AssessmentListCommandHandler> logger)
        {
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
                _logger.LogError(e, e.Message);
                throw new ApplicationException("Unable to get list of assessments.");
            }
        }
    }
}
