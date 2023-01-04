using FluentValidation;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators
{
    public class CurrencyValidator : AbstractValidator<QuestionActivityData>
    {
        public CurrencyValidator()
        {
            RuleFor(x => x.Answer).NotNull().WithMessage("The question has not been answered")
                .DependentRules(
                    () =>
                    {
                        RuleFor(x => x.Answer).NotEmpty().WithMessage("The question has not been answered");
                        RuleFor(x => x.Answer).Must(answer =>
                        {
                            var isNumeric = int.TryParse(answer, out _);
                            return isNumeric;
                        }).WithMessage("The answer is not a number");
                    }
                );
        }
    }
}
