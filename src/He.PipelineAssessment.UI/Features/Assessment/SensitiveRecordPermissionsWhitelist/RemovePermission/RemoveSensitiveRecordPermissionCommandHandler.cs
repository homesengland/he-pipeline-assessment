using He.PipelineAssessment.Infrastructure.Repository;
using MediatR;
using Microsoft.Extensions.Logging;

namespace He.PipelineAssessment.UI.Features.Assessment.SensitiveRecordPermissionsWhitelist.RemovePermission
{
    public class RemoveSensitiveRecordPermissionCommandHandler : IRequestHandler<RemoveSensitiveRecordPermissionCommand, RemoveSensitiveRecordPermissionResponse>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<RemoveSensitiveRecordPermissionCommandHandler> _logger;

        public RemoveSensitiveRecordPermissionCommandHandler(
            IAssessmentRepository assessmentRepository,
            ILogger<RemoveSensitiveRecordPermissionCommandHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
        }

        public async Task<RemoveSensitiveRecordPermissionResponse> Handle(RemoveSensitiveRecordPermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var whitelist = await _assessmentRepository.GetSensitiveRecordWhitelistById(request.Id);
                
                if (whitelist == null)
                {
                    return new RemoveSensitiveRecordPermissionResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Permission not found."
                    };
                }

                var result = await _assessmentRepository.DeleteSensitiveRecordWhitelist(whitelist);
                
                _logger.LogInformation(
                    "Permission removed successfully. WhitelistId={Id}, AssessmentId={AssessmentId}, Result={Result}", 
                    request.Id, request.AssessmentId, result);

                return new RemoveSensitiveRecordPermissionResponse
                {
                    IsSuccess = true,
                    SuccessMessage = $"Permission for {whitelist.Email} removed successfully."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing permission for Id={Id}, AssessmentId={AssessmentId}", request.Id, request.AssessmentId);
                
                return new RemoveSensitiveRecordPermissionResponse
                {
                    IsSuccess = false,
                    ErrorMessage = $"Failed to remove permission: {ex.Message}"
                };
            }
        }
    }
}