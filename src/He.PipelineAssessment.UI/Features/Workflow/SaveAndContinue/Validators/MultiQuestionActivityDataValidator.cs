using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using FluentValidation;

namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue.Validators
{
    public class MultiQuestionActivityDataValidator : AbstractValidator<QuestionActivityData>
    {
        public MultiQuestionActivityDataValidator()
        {
            RuleFor(x => x.MultipleChoice)
                .SetValidator(new MultiChoiceValidator())
                .When(x => x.QuestionType == QuestionTypeConstants.MultipleChoiceQuestion);

            RuleFor(x => x.Date)
                .SetValidator(new DateValidator())
                .When(x => x.QuestionType == QuestionTypeConstants.DateQuestion);
        }
    }
}
