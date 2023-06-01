using FluentValidation;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateOverride
{
    public class CreateOverrideCommandValidator : AbstractValidator<CreateOverrideCommand>
    {
        public CreateOverrideCommandValidator()
        {
            RuleFor(c => c.SignOffDocument).NotEmpty().WithMessage("The {PropertyName} cannot be empty");
            RuleFor(c => c.AdministratorRationale).NotEmpty().WithMessage("The {PropertyName} cannot be empty");
            RuleFor(c => c.TargetWorkflowId).NotEmpty().WithMessage("The target workflow definition has to be selected");
        }
    }
}
