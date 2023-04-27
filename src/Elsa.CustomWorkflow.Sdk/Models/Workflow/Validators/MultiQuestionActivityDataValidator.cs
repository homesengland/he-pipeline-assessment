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

            RuleFor(x => x.Radio)
                .SetValidator(new RadioValidator())
                .When(x => x.QuestionType == QuestionTypeConstants.RadioQuestion || x.QuestionType == QuestionTypeConstants.PotScoreRadioQuestion);

            RuleFor(x => x.Date)
                .SetValidator(new DateValidator())
                .When(x => x.QuestionType == QuestionTypeConstants.DateQuestion);

            RuleFor(x => x)
                .SetValidator(new DecimalValidator())
                .When(x => x.QuestionType == QuestionTypeConstants.CurrencyQuestion);

            RuleFor(x => x)
                .SetValidator(new DecimalValidator())
                .When(x => x.QuestionType == QuestionTypeConstants.DecimalQuestion);

            RuleFor(x => x)
                .SetValidator(new DecimalValidator())
                .When(x => x.QuestionType == QuestionTypeConstants.PercentageQuestion);

            RuleFor(x => x)
                .SetValidator(new IntegerValidator())
                .When(x => x.QuestionType == QuestionTypeConstants.IntegerQuestion);

            RuleFor(x => x)
                .SetValidator(new TextValidator())
                .When(x => x.QuestionType == QuestionTypeConstants.TextQuestion);

            RuleFor(x => x)
                .SetValidator(new TextValidator())
                .When(x => x.QuestionType == QuestionTypeConstants.TextAreaQuestion);
        }
    }
}
