using FluentValidation;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentToolWorkflowCommand;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Validators
{
    public class UpdateAssessmentToolWorkflowCommandValidator : AbstractValidator<UpdateAssessmentToolWorkflowCommand>
    {
        public UpdateAssessmentToolWorkflowCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("The {PropertyName} cannot be empty").MaximumLength(100);
            RuleFor(c => c.WorkflowDefinitionId).NotEmpty().WithMessage("The {PropertyName} cannot be empty");
        }
    }
}
