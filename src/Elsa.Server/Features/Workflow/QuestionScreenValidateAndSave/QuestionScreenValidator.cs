using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Elsa.CustomWorkflow.Sdk.Models.Workflow.Validators;
using FluentValidation;

namespace Elsa.Server.Features.Workflow.QuestionScreenValidateAndSave
{
    public class QuestionScreenValidator : AbstractValidator<WorkflowActivityDataDto>
    {
        public QuestionScreenValidator()
        {

            RuleFor(x => x.Data.Questions)
                .ForEach(x =>
                {
                    x.SetValidator(new MultiQuestionActivityDataValidator());
                })
                .When(x => x.Data.ActivityType == ActivityTypeConstants.QuestionScreen);
        }
    }
}
