using FluentValidation;
using He.PipelineAssessment.Infrastructure.Repository;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace He.PipelineAssessment.UI.Features.Assessment.SensitiveRecordPermissionsWhitelist.AddPermission
{
    public class AddSensitiveRecordPermissionCommandValidator : AbstractValidator<AddSensitiveRecordPermissionCommand>
    {
        private const string AllowedEmailDomain = "@homesengland.gov.uk";
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IMediator _mediator;

        public AddSensitiveRecordPermissionCommandValidator(IAssessmentRepository assessmentRepository, IMediator mediator)
        {
            _assessmentRepository = assessmentRepository;
            _mediator = mediator;

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Enter an email address");

            RuleFor(x => x.Email)
                .Must(BeValidEmailFormat)
                .WithMessage("Enter an email address in the correct format such as name@homesengland.gov.uk")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.Email)
                .Must(HaveCorrectDomain)
                .WithMessage("Enter a Homes England email address ending with @homesengland.gov.uk")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x)
                .MustAsync(NotBeDuplicateEmail)
                .WithMessage(x => $"{x.Email} is already on the permissions list for this assessment")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x)
                .MustAsync(BeAuthorizedUser)
                .WithMessage("User is not authorized to add permissions")
                .WithErrorCode("Unauthorized");
        }

        private bool BeValidEmailFormat(string email)
        {
            var emailAttribute = new EmailAddressAttribute();
            return emailAttribute.IsValid(email);
        }

        private bool HaveCorrectDomain(string email)
        {
            return email.EndsWith(AllowedEmailDomain, StringComparison.OrdinalIgnoreCase);
        }

        private async Task<bool> NotBeDuplicateEmail(AddSensitiveRecordPermissionCommand command, CancellationToken cancellationToken)
        {
            bool isUserAlreadyAdded = await _assessmentRepository.IsUserWhitelistedForSensitiveRecord(command.AssessmentId, command.Email);
            return !isUserAlreadyAdded;
        }

        private async Task<bool> BeAuthorizedUser(AddSensitiveRecordPermissionCommand command, CancellationToken cancellationToken)
        {
            var assessmentSummary = await _mediator.Send(new SensitiveRecordPermissionsWhitelistRequest(command.AssessmentId), cancellationToken);
            var isProjectManager = assessmentSummary.AssessmentSummary.ProjectManager == command.CurrentUsername;
            return command.IsAdmin || isProjectManager;
        }
    }
}