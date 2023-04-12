using FluentValidation;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators
{
    public class CurrencyValidator : AbstractValidator<QuestionActivityData>
    {
        public CurrencyValidator()
        {
            RuleFor(x => x.Answers).NotEmpty().WithMessage("The answer must be a number")
                .DependentRules(
                    () =>
                    {
                        RuleForEach(x => x.Answers).NotEmpty().WithMessage("The answer must be a number");
                        RuleForEach(x => x.Answers).Must(answer =>
                        {
                            var isNumeric = decimal.TryParse(answer.AnswerText, out _);
                            return isNumeric;
                        }).WithMessage("The answer must be a number");
                    }
                );
        }
    }
}
