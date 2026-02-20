using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessment.SensitiveRecordPermissionsWhitelist.RemovePermission
{
    public class RemoveSensitiveRecordPermissionCommand : IRequest<RemoveSensitiveRecordPermissionResponse>
    {
        public int Id { get; set; }
        public int AssessmentId { get; set; }
        public string CurrentUsername { get; set; } = null!;
        public bool IsAdmin { get; set; }
    }

    public class RemoveSensitiveRecordPermissionResponse
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }
    }
}