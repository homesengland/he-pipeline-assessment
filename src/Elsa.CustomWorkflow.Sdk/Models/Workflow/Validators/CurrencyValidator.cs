using FluentValidation;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators
{
    public class CurrencyValidator : AbstractValidator<QuestionActivityData>
    {
        public CurrencyValidator()
        {
            RuleFor(x => x.Answer).NotNull().WithMessage("The answer must be a number")
                .DependentRules(
                    () =>
                    {
                        RuleFor(x => x.Answer).NotEmpty().WithMessage("The answer must be a number");
                        RuleFor(x => x.Answer).Must(answer =>
                        {
                            var isNumeric = decimal.TryParse(answer, out _);
                            return isNumeric;
                        }).WithMessage("The answer must be a number");
                    }
                );
        }
    }
}
