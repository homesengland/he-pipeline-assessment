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
                .NotEmpty()
                .Matches("[a-zA-Z]")
                .MaximumLength(80)
                .WithMessage("The {PropertyName} cannot be empty");
           
            RuleFor(c => c.Name)
                .Must(BeUnique)
                .WithMessage("The {PropertyName} must be unique and not named after another Assessment Fund");


            RuleFor(c => c.Description)
                .NotEmpty()
                .Matches("[a-zA-Z]")
                .MaximumLength(180)
                .WithMessage("The {PropertyName} cannot be empty");
        }

        private bool BeUnique(string name)
        {
            //COMMENT: The line below basically checks if any existing fund has the same name (case insensitive). == false means it's unique.
            if (_assessmentRepository.GetAllFunds().Result.Any(f => f.Name.Equals(name, StringComparison.OrdinalIgnoreCase)) == false)
                //COMMENT: it returns true if unique
                return true;
            //COMMENT: it returns false if not unique.
            return false;
        }



    }
}
