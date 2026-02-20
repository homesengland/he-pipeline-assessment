using FluentValidation;
using He.PipelineAssessment.Infrastructure.Repository;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessment.SensitiveRecordPermissionsWhitelist.RemovePermission
{
    public class RemoveSensitiveRecordPermissionCommandValidator : AbstractValidator<RemoveSensitiveRecordPermissionCommand>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IMediator _mediator;

        public RemoveSensitiveRecordPermissionCommandValidator(IAssessmentRepository assessmentRepository, IMediator mediator)
        {
            _assessmentRepository = assessmentRepository;
            _mediator = mediator;

            RuleFor(x => x)
                .MustAsync(BeAuthorizedUser)
                .WithMessage("User is not authorized to remove permissions")
                .WithErrorCode("Unauthorized");

            RuleFor(x => x)
                .MustAsync(WhitelistEntryMustExist)
                .WithMessage("Permission not found")
                .WithErrorCode("NotFound");

            RuleFor(x => x)
                .MustAsync(WhitelistEntryMustBelongToAssessment)
                .WithMessage("Invalid permission reference")
                .WithErrorCode("InvalidReference");
        }

        private async Task<bool> BeAuthorizedUser(RemoveSensitiveRecordPermissionCommand command, CancellationToken cancellationToken)
        {
            var assessmentSummary = await _mediator.Send(new SensitiveRecordPermissionsWhitelistRequest(command.AssessmentId), cancellationToken);
            var isProjectManager = assessmentSummary.AssessmentSummary.ProjectManager == command.CurrentUsername;
            return command.IsAdmin || isProjectManager;
        }

        private async Task<bool> WhitelistEntryMustExist(RemoveSensitiveRecordPermissionCommand command, CancellationToken cancellationToken)
        {
            var whitelist = await _assessmentRepository.GetSensitiveRecordWhitelistById(command.Id);
            return whitelist != null;
        }

        private async Task<bool> WhitelistEntryMustBelongToAssessment(RemoveSensitiveRecordPermissionCommand command, CancellationToken cancellationToken)
        {
            var whitelist = await _assessmentRepository.GetSensitiveRecordWhitelistById(command.Id);
            return whitelist?.AssessmentId == command.AssessmentId;
        }
    }
}