using FluentValidation;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators
{
    public class MultiQuestionActivityDataValidator : AbstractValidator<QuestionActivityData>
    {
        public MultiQuestionActivityDataValidator()
        {
            RuleFor(x => x.Checkbox)
                .SetValidator(new MultiChoiceValidator())
                .When(x => x.QuestionType == QuestionTypeConstants.CheckboxQuestion);

            RuleFor(x => x.Date)
                .SetValidator(new DateValidator())
                .When(x => x.QuestionType == QuestionTypeConstants.DateQuestion);
        }
    }
}
