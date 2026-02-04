using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolWorkflow;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Shared
{
    public partial class UpdateAssessmentStatusHandler : IRequestHandler<UpdateAssessmentStatusRequest, Unit>
    {
        private readonly ILogger<UpdateAssessmentStatusHandler> _logger;
        private readonly IAssessmentRepository _assessmentRepository;


        public UpdateAssessmentStatusHandler(ILogger<UpdateAssessmentStatusHandler> logger, IAssessmentRepository repo)
        {
            _logger = logger;
            _assessmentRepository = repo;
        }


        public async Task<Unit> Handle(UpdateAssessmentStatusRequest request, CancellationToken cancellationToken)
        {
            await UpdateAssessmentStatus(request.AssessmentId);
            return Unit.Value;
        }

        public async Task UpdateAssessmentStatus(int assessmentId)
        {
            try
            {
                // Execute both metadata queries in parallel for efficiency
                var (fundId, workflowToolId) = await GetAssessmentStatusData(assessmentId);

                // Early return if no metadata to update
                if (!fundId.HasValue && !workflowToolId.HasValue)
                {
                    LogNoStatusUpdatesRequired(assessmentId);
                    return;
                }

                // Retrieve assessment once
                var assessment = await _assessmentRepository.GetAssessment(assessmentId);

                if (assessment == null)
                {
                    LogAssessmentNotFound(assessmentId);
                    return;
                }
                
                bool hasChanges = false;

                // Update FundId if needed
                if (fundId.HasValue && assessment.FundId != fundId.Value)
                {
                    assessment.FundId = fundId.Value;
                    hasChanges = true;
                    LogFundIdUpdated(fundId.Value, assessmentId);
                }

                // Update LatestAssessmentWorkflowToolId if needed
                if (workflowToolId.HasValue && assessment.LatestAssessmentWorkflowToolId != workflowToolId.Value)
                {
                    assessment.LatestAssessmentWorkflowToolId = workflowToolId.Value;
                    hasChanges = true;
                    LogWorkflowToolIdUpdated(workflowToolId.Value, assessmentId);
                }

                // Save changes once if any updates were made
                if (hasChanges)
                {
                    await _assessmentRepository.SaveChanges();
                    LogStatusSaved(assessmentId);
                }
            }
            catch (Exception ex)
            {
                LogStatusUpdateError(ex, assessmentId);
                throw;
            }
        }

        private async Task<(int? fundId, int? workflowToolId)> GetAssessmentStatusData(int assessmentId)
        {
            // Execute both queries in parallel for maximum efficiency
            var fundIdTask = GetCurrentFundId(assessmentId);
            var workflowToolIdTask = GetLatestCompletedWorkflowId(assessmentId);

            await Task.WhenAll(fundIdTask, workflowToolIdTask);

            return (await fundIdTask, await workflowToolIdTask);
        }

        private async Task<int?> GetCurrentFundId(int assessmentId)
        {
            try
            {
                var fundId = await _assessmentRepository.GetCurrentWorkflowFundId(assessmentId);

                if (!fundId.HasValue)
                {
                    LogNoActiveWorkflowWithFund(assessmentId);
                }

                return fundId;
            }
            catch (Exception ex)
            {
                LogFundIdRetrievalError(ex, assessmentId);
                throw;
            }
        }

        private async Task<int?> GetLatestCompletedWorkflowId(int assessmentId)
        {
            try
            {
                var workflowId = await _assessmentRepository.GetLatestCompletedWorkflowId(assessmentId);

                if (!workflowId.HasValue)
                {
                    LogNoCompletedWorkflowFound(assessmentId);
                }

                return workflowId;
            }
            catch (Exception ex)
            {
                LogWorkflowIdRetrievalError(ex, assessmentId);
                throw;
            }
        }

        // ========== High-Performance LoggerMessage Methods ==========

        [LoggerMessage(
            EventId = 1001,
            Level = LogLevel.Debug,
            Message = "No status updates required for AssessmentId: {AssessmentId}")]
        private partial void LogNoStatusUpdatesRequired(int assessmentId);

        [LoggerMessage(
            EventId = 1002,
            Level = LogLevel.Warning,
            Message = "Assessment not found for AssessmentId whilst updating Status Information: {AssessmentId}")]
        private partial void LogAssessmentNotFound(int assessmentId);

        [LoggerMessage(
            EventId = 1003,
            Level = LogLevel.Debug,
            Message = "Updated FundId to {FundId} for AssessmentId: {AssessmentId}")]
        private partial void LogFundIdUpdated(int fundId, int assessmentId);

        [LoggerMessage(
            EventId = 1004,
            Level = LogLevel.Debug,
            Message = "Updated LatestCompletedWorkflowToolId to {WorkflowToolId} for AssessmentId: {AssessmentId}")]
        private partial void LogWorkflowToolIdUpdated(int workflowToolId, int assessmentId);

        [LoggerMessage(
            EventId = 1005,
            Level = LogLevel.Information,
            Message = "Successfully saved status updates for AssessmentId: {AssessmentId}")]
        private partial void LogStatusSaved(int assessmentId);

        [LoggerMessage(
            EventId = 1006,
            Level = LogLevel.Error,
            Message = "Error updating assessment status for AssessmentId: {AssessmentId}")]
        private partial void LogStatusUpdateError(Exception ex, int assessmentId);

        [LoggerMessage(
            EventId = 1007,
            Level = LogLevel.Debug,
            Message = "No active workflow with fund found for AssessmentId: {AssessmentId}")]
        private partial void LogNoActiveWorkflowWithFund(int assessmentId);

        [LoggerMessage(
            EventId = 1008,
            Level = LogLevel.Error,
            Message = "Error retrieving fund ID for AssessmentId: {AssessmentId}")]
        private partial void LogFundIdRetrievalError(Exception ex, int assessmentId);

        [LoggerMessage(
            EventId = 1009,
            Level = LogLevel.Debug,
            Message = "No completed workflow found for AssessmentId: {AssessmentId}")]
        private partial void LogNoCompletedWorkflowFound(int assessmentId);

        [LoggerMessage(
            EventId = 1010,
            Level = LogLevel.Error,
            Message = "Error retrieving latest completed workflow ID for AssessmentId: {AssessmentId}")]
        private partial void LogWorkflowIdRetrievalError(Exception ex, int assessmentId);
    }
}
