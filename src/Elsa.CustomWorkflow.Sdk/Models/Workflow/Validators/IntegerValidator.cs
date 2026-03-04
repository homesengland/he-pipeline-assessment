using FluentValidation;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators
{
    public class IntegerValidator : AbstractValidator<QuestionActivityData>
    {
        public IntegerValidator()
        {
            RuleFor(x => x.Answers).NotEmpty().WithMessage("The answer must be a whole number. If none, please enter 0.")
                .DependentRules(
                    () =>
                    {
                        RuleForEach(x => x.Answers).NotEmpty().WithMessage("The answer must be a whole number. If none, please enter 0.");
                        RuleForEach(x => x.Answers).Must(answer => answer != null && !string.IsNullOrEmpty(answer.AnswerText))
                            .WithMessage("The answer must be a whole number. If none, please enter 0.").DependentRules(() =>
                                RuleForEach(x => x.Answers).Must(answer =>
                                {
                                    if (answer.AnswerText == "0")
                                    {
                                        return true;
                                    }

                                    var answerText = answer.AnswerText!.Replace(",", "").TrimEnd('0').TrimEnd('.');
                                    var isNumeric = int.TryParse(answerText, out _);
                                    return isNumeric;

                                }).WithMessage((question, answer) => $"The answer {answer.AnswerText} must be a whole number"));
                    }
                );
        }
    }
}
