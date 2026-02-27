using FluentValidation;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators
{
    public class DecimalValidator : AbstractValidator<QuestionActivityData>
    {
        public DecimalValidator()
        {
            RuleFor(x => x.Answers).NotEmpty().WithMessage("The answer must be a number. If none, please enter 0.")
                .DependentRules(
                    () =>
                    {
                        RuleForEach(x => x.Answers).NotEmpty().WithMessage("The answer must be a number. If none, please enter 0.");
                        RuleForEach(x => x.Answers).Must(answer => 
                        {
                            if (answer != null && !String.IsNullOrEmpty(answer.AnswerText))
                            {
                                var isNumeric = decimal.TryParse(answer!.AnswerText.Replace(",", ""), out _);
                                return isNumeric;
                            }
                            else
                            {
                                return false;
                            }
                        }).WithMessage("The answer must be a number. If none, please enter 0.");
                    }
                );
        }
    }
}
