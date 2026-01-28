using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolWorkflow;

namespace He.PipelineAssessment.UI.Common.Utility
{
    public interface IAssessmentFundIdProvider
    {
        public Task AssignFundId(int assessmentId);
    }
    public class AssessmentFundIdProvider : IAssessmentFundIdProvider
    {
        private readonly ILogger<AssessmentFundIdProvider> _logger;
        private readonly IAssessmentRepository _assessmentRepository;
        public AssessmentFundIdProvider(ILogger<AssessmentFundIdProvider> logger, IAssessmentRepository repo)
        {
            _logger = logger;
            _assessmentRepository = repo;

        }

        public async Task AssignFundId(int assessmentId)
        {
            int? fundId = await GetCurrentFundId(assessmentId);

            if (fundId.HasValue)
            {
                var assessment = await _assessmentRepository.GetAssessment(assessmentId);

                if (assessment != null && assessment.FundId != fundId.Value)
                {
                    assessment.FundId = fundId.Value;
                    await _assessmentRepository.SaveChanges();
                    _logger.LogInformation("Assigned FundId {FundId} to AssessmentId: {AssessmentId}", fundId.Value, assessmentId);
                }
            }
        }

        public async Task<int?> GetCurrentFundId(int assessmentId)
        {
            try
            {
                var fundId = await _assessmentRepository.GetCurrentWorkflowFundId(assessmentId);

                if (!fundId.HasValue)
                {
                    _logger.LogWarning("No active workflow with fund found for AssessmentId: {AssessmentId}", assessmentId);
                }

                return fundId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving fund ID for AssessmentId: {AssessmentId}", assessmentId);
                throw;
            }
        }
    }
}
