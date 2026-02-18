using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace He.PipelineAssessment.UI.Features.Assessment.SensitiveRecordPermissionsWhitelist.AddPermission
{
    public class AddSensitiveRecordPermissionCommandHandler : IRequestHandler<AddSensitiveRecordPermissionCommand, AddSensitiveRecordPermissionResponse>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<AddSensitiveRecordPermissionCommandHandler> _logger;

        public AddSensitiveRecordPermissionCommandHandler(
            IAssessmentRepository assessmentRepository,
            ILogger<AddSensitiveRecordPermissionCommandHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _logger = logger;
        }

        public async Task<AddSensitiveRecordPermissionResponse> Handle(AddSensitiveRecordPermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var whitelist = new SensitiveRecordWhitelist
                {
                    AssessmentId = request.AssessmentId,
                    Email = request.Email.Trim()
                };

                var result = await _assessmentRepository.CreateSensitiveRecordWhitelist(whitelist);
                
                _logger.LogInformation(
                    "Permission added successfully. AssessmentId={AssessmentId}, NewId={Id}, Result={Result}", 
                    request.AssessmentId, whitelist.Id, result);

                return new AddSensitiveRecordPermissionResponse
                {
                    IsSuccess = true,
                    SuccessMessage = $"Permission for {whitelist.Email} added successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding permission for AssessmentId={AssessmentId}", request.AssessmentId);
                
                return new AddSensitiveRecordPermissionResponse
                {
                    IsSuccess = false,
                    ErrorMessage = $"Failed to add permission: {ex.Message}"
                };
            }
        }
    }
}