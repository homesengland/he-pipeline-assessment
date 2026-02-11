using FluentValidation;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentFund;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Validators
{
    public class CreateAssessmentFundCommandValidator : AbstractValidator<CreateAssessmentFundCommand>
    {
        private readonly IAssessmentRepository _assessmentRepository;

        public CreateAssessmentFundCommandValidator (IAssessmentRepository assessmentRepository)
        {
            this._assessmentRepository = assessmentRepository;

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty.")
                .Matches("^[a-zA-Z0-9 ]+$").WithMessage("{PropertyName} can only contain letters a-z, A-Z, numbers 0-9, and spaces. Special characters are not allowed.")
                .MaximumLength(80).WithMessage("{PropertyName} must be at most 80 characters.");
           
            RuleFor(c => c.Name)
                .Must(BeUnique)
                .WithMessage("The {PropertyName} must be unique and not named after another Assessment Fund.");

            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty.")
                .Matches("^[a-zA-Z0-9 ]+$").WithMessage("{PropertyName} can only contain letters a-z, A-Z, numbers 0-9, and spaces. Special characters are not allowed.")
                .MaximumLength(160).WithMessage("{PropertyName} must be at most 160 characters.");
        }

        private bool BeUnique(string name)
        {
            if (_assessmentRepository.GetAllFunds().Result.Any(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) == false)
                return true;
            return false;
        }



    }
}
