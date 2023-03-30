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
                        RuleForEach(x => x.Answers.Select(y => y.AnswerText)).NotEmpty().WithMessage("The question has not been answered");
                    }
                );
        }
    }
}
