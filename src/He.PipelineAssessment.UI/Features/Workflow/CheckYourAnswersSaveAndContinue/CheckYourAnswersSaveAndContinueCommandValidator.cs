using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators;
using FluentValidation;

namespace He.PipelineAssessment.UI.Features.Workflow.CheckYourAnswersSaveAndContinue
{
    public class CheckYourAnswersSaveAndContinueCommandValidator : AbstractValidator<CheckYourAnswersSaveAndContinueCommand>
    {
        public CheckYourAnswersSaveAndContinueCommandValidator()
        {

            RuleFor(x => x.Data.QuestionScreenAnswers)
                .ForEach(x =>
                {
                    x.SetValidator(new MultiQuestionActivityDataValidator());
                })
                .When(x => x.Data.ActivityType == ActivityTypeConstants.QuestionScreen); ;
        }
    }
}
