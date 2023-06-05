using Elsa.CustomWorkflow.Sdk;
using FluentValidation;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateOverride;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateRollback;

namespace He.PipelineAssessment.UI.Features.Intervention.InterventionManagement
{
    public class AssessmentInterventionCommandValidator : AbstractValidator<AssessmentInterventionCommand>
    {
        public AssessmentInterventionCommandValidator()
        {

            RuleFor(c => c.SignOffDocument).NotEmpty().WithMessage("The {PropertyName} cannot be empty")
                .When(x => x.GetType() != typeof(CreateRollbackCommand));
            RuleFor(c => c.AdministratorRationale).NotEmpty().WithMessage("The {PropertyName} cannot be empty")
                .When(x => x.GetType() != typeof(CreateRollbackCommand));
            RuleFor(c => c.TargetWorkflowId).NotEmpty().WithMessage("The target workflow definition has to be selected")
                .When(x => x.GetType() != typeof(CreateRollbackCommand));
            RuleFor(c => c.AssessorRationale).NotEmpty().WithMessage("The {PropertyName} cannot be empty")
                .When(x => x.GetType() == typeof(CreateRollbackCommand));
        }
    }
}
