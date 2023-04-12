using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators;
using FluentValidation;

namespace He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue
{
    public class SaveAndContinueCommandValidator : AbstractValidator<QuestionScreenSaveAndContinueCommand>
    {
        public SaveAndContinueCommandValidator()
        {

            RuleFor(x => x.Data.Questions)
                .ForEach(x =>
                {
                    x.SetValidator(new MultiQuestionActivityDataValidator());
                })
                .When(x => x.Data.ActivityType == ActivityTypeConstants.QuestionScreen); ;
        }
    }
}
