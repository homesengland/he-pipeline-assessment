using FluentValidation;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators
{
    public class TextValidator : AbstractValidator<QuestionActivityData>
    {
        public TextValidator()
        {
            RuleFor(x => x.Answer).NotNull().WithMessage("The question has not been answered")
                .DependentRules(
                    () =>
                    {
                        RuleFor(x => x.Answer).NotEmpty().WithMessage("The question has not been answered");
                    }
                );
        }
    }
}
