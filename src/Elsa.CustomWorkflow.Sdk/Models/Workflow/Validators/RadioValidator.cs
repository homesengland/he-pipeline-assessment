using FluentValidation;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators
{
    public class RadioValidator : AbstractValidator<Radio>
    {
        public RadioValidator()
        {
            RuleFor(x => x.SelectedAnswer).NotEmpty().WithMessage("The question has not been answered");
        }
    }
}
