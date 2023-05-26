using FluentValidation;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators
{
    public class IntegerValidator : AbstractValidator<QuestionActivityData>
    {
        public IntegerValidator()
        {
            RuleFor(x => x.Answers).NotEmpty().WithMessage("The answer must be a whole number")
                .DependentRules(
                    () =>
                    {
                        RuleForEach(x => x.Answers).NotEmpty().WithMessage("The answer must be a whole number");
                        RuleForEach(x => x.Answers).Must(answer =>
                        {
                            if (answer !=null)
                            {
                                var isNumeric = int.TryParse(answer!.AnswerText.Replace(",", ""), out _);
                                return isNumeric;
                            }
                            else
                            {
                                return false;
                            }
                            
                        }).WithMessage("The answer must be a whole number");
                    }
                );
        }
    }
}
