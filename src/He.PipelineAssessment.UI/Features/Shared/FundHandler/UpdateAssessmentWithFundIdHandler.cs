using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolWorkflow;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Shared
{
    public class UpdateAssessmentWithFundIdHandler : IRequestHandler<UpdateAssessmentWithFundIdRequest, Unit>
    {
        private readonly ILogger<UpdateAssessmentWithFundIdHandler> _logger;
        private readonly IAssessmentRepository _assessmentRepository;


        public UpdateAssessmentWithFundIdHandler(ILogger<UpdateAssessmentWithFundIdHandler> logger, IAssessmentRepository repo)
        {
            _logger = logger;
            _assessmentRepository = repo;

        }


        public async Task<Unit> Handle(UpdateAssessmentWithFundIdRequest request, CancellationToken cancellationToken)
        {
            await AssignFundId(request.AssessmentId);
            return Unit.Value;
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
