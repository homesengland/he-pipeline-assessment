using FluentValidation;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators
{
    public class TextValidator : AbstractValidator<QuestionActivityData>
    {
        public TextValidator()
        {
            RuleFor(x => x.Answers).NotNull().WithMessage("The question has not been answered")
                .DependentRules(
                    () =>
                    {
                        RuleForEach(x => x.Answers).NotEmpty().WithMessage("The question has not been answered");
                        RuleForEach(x => x.Answers).Must(answer =>
                        {
                            var isNotNull = answer.AnswerText != null && answer.AnswerText != string.Empty;
                            return isNotNull;
                        }).WithMessage("The question has not been answered");
                    }
                );
            RuleFor(x => x.Answers).NotEmpty().WithMessage("The question has not been answered")
                .DependentRules(
                    () =>
                    {
                        RuleForEach(x => x.Answers).NotEmpty().WithMessage("The question has not been answered");
                        RuleForEach(x => x.Answers).Must(answer =>
                        {
                            var isNotNull = answer.AnswerText != null && answer.AnswerText != string.Empty;
                            return isNotNull;
                        }).WithMessage("The question has not been answered");
                    }
                );
        }
    }
}
