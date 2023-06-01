using FluentValidation;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateOverride;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement
{
    public class AssessmentInterventionCommandValidator : AbstractValidator<AssessmentInterventionCommand>
    {
        public AssessmentInterventionCommandValidator()
        {
            RuleFor(c => c.SignOffDocument).NotEmpty().WithMessage("The {PropertyName} cannot be empty");
            RuleFor(c => c.AdministratorRationale).NotEmpty().WithMessage("The {PropertyName} cannot be empty");
            RuleFor(c => c.TargetWorkflowId).NotEmpty().WithMessage("The target workflow definition has to be selected");
        }
    }
}
