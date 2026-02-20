using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessment.SensitiveRecordPermissionsWhitelist.AddPermission
{
    public class AddSensitiveRecordPermissionCommand : IRequest<AddSensitiveRecordPermissionResponse>
    {
        public int AssessmentId { get; set; }
        public string Email { get; set; } = null!;
        public string CurrentUsername { get; set; } = null!;
        public bool IsAdmin { get; set; }
    }

    public class AddSensitiveRecordPermissionResponse
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ValidationMessage { get; set; }
        public string? SuccessMessage { get; set; }
        public string? EmailValue { get; set; }
    }
}