using Elsa.CustomWorkflow.Sdk;
using FluentValidation;
using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue.Validators;

namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue
{
    public class SaveAndContinueCommandValidator : AbstractValidator<SaveAndContinueCommand>
    {
        public SaveAndContinueCommandValidator()
        {
            RuleFor(x => x.Data.QuestionActivityData!.MultipleChoice)
                .SetValidator(new MultiChoiceValidator())
                .When(x => x.Data.QuestionActivityData!.ActivityType == ActivityTypeConstants.MultipleChoiceQuestion);

            RuleFor(x => x.Data.QuestionActivityData!.Date)
                .SetValidator(new DateValidator())
                .When(x => x.Data.QuestionActivityData!.ActivityType == ActivityTypeConstants.DateQuestion);

            RuleFor(x => x.Data.MultiQuestionActivityData)
                .ForEach(x =>
                {
                    x.SetValidator(new MultiQuestionActivityDataValidator());
                });
        }
    }
}
