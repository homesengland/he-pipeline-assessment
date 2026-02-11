using FluentValidation;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentFund;
using System.Drawing.Text;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Validators
{
    public class UpdateAssessmentFundCommandValidator : AbstractValidator<UpdateAssessmentFundCommand>
    {
        private readonly IAssessmentRepository _assessmentRepository;

        public UpdateAssessmentFundCommandValidator(IAssessmentRepository assessmentRepository)
        {
            this._assessmentRepository = assessmentRepository;

            RuleFor(c => c.Name)
     .NotEmpty().WithMessage("{PropertyName} cannot be empty.")
     .Matches("^[a-zA-Z0-9 ]+$").WithMessage("{PropertyName} can only contain letters a-z, A-Z, numbers 0-9, and spaces. Special characters are not allowed.")
     .MaximumLength(80).WithMessage("{PropertyName} must be at most 80 characters.");

            RuleFor(c => c)
                .Must((c) => BeUnique(c.Name, c.Id))
                .WithMessage("The {PropertyName} must be unique and not named after another Assessment Fund.");

            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("{PropertyName} cannot be empty")
                .Matches("^[a-zA-Z0-9 ]+$").WithMessage("{PropertyName} can only contain letters a-z, A-Z, numbers 0-9, and spaces. Special characters are not allowed.")
                .MaximumLength(160).WithMessage("{PropertyName} must be at most 160 characters.")
                .WithMessage("The {PropertyName} cannot be empty.");
        }

        private bool BeUnique(string name, int id)
        {
            if (_assessmentRepository.GetAllFunds().Result.Any(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && 
                f.Id != id) == false)
                return true;
            return false;
        }
    }
}
