using FluentValidation;
using He.PipelineAssessment.UI.Features.Override.CreateOverride;
using He.PipelineAssessment.UI.Features.Override.EditOverride;
using He.PipelineAssessment.UI.Features.Rollback.CreateRollback;
using He.PipelineAssessment.UI.Features.Rollback.EditRollback;
using He.PipelineAssessment.UI.Features.Rollback.EditRollbackAssessor;
using He.PipelineAssessment.UI.Features.Variation.CreateVariation;
using He.PipelineAssessment.UI.Features.Variation.EditVariation;
using He.PipelineAssessment.UI.Features.Variation.EditVariationAssessor;

namespace He.PipelineAssessment.UI.Features.Intervention
{
    public class AssessmentInterventionCommandValidator : AbstractValidator<AssessmentInterventionCommand>
    {
        public AssessmentInterventionCommandValidator()
        {

            RuleFor(c => c.SignOffDocument).NotEmpty().WithMessage("The {PropertyName} cannot be empty")
                .When(x =>
                    x.GetType() == typeof(CreateOverrideCommand) ||
                    x.GetType() == typeof(EditOverrideCommand) ||
                    x.GetType() == typeof(AssessmentInterventionCommand));

            RuleFor(c => c.AdministratorRationale).NotEmpty().WithMessage("The {PropertyName} cannot be empty")
                .When(x =>
                    x.GetType() == typeof(CreateOverrideCommand) ||
                    x.GetType() == typeof(EditOverrideCommand) ||
                    x.GetType() == typeof(EditRollbackCommand) ||
                    x.GetType() == typeof(EditVariationCommand) ||
                    x.GetType() == typeof(AssessmentInterventionCommand));

            RuleFor(c => c.TargetWorkflowId).NotEmpty().WithMessage("The target workflow definition has to be selected")
                .When(x =>
                    x.GetType() == typeof(CreateOverrideCommand) ||
                    x.GetType() == typeof(EditOverrideCommand) ||
                    x.GetType() == typeof(EditRollbackCommand));


            RuleFor(x => x.TargetWorkflowDefinitions).Must(
                wd =>
                {
                    return wd.Count(y =>
                        y.IsSelected) > 0;
                }).WithMessage("At least one target workflow definition has to be selected").When(x =>
                x.GetType() == typeof(EditVariationCommand) ||
                x.GetType() == typeof(AssessmentInterventionCommand));

            RuleFor(c => c.AssessorRationale).NotEmpty().WithMessage("The {PropertyName} cannot be empty")
                .When(x =>
                    x.GetType() == typeof(CreateRollbackCommand) ||
                    x.GetType() == typeof(EditRollbackAssessorCommand) ||
                    x.GetType() == typeof(CreateVariationCommand) ||
                    x.GetType() == typeof(EditVariationAssessorCommand) ||
                    x.GetType() == typeof(AssessmentInterventionCommand));

            RuleFor(c => c.InterventionReasonId).NotEmpty().WithMessage("The request reason cannot be empty")
                .When(x =>
                    x.GetType() == typeof(CreateRollbackCommand) ||
                    x.GetType() == typeof(EditRollbackAssessorCommand) ||
                    x.GetType() == typeof(CreateVariationCommand) ||
                    x.GetType() == typeof(EditVariationAssessorCommand) ||
                    x.GetType() == typeof(AssessmentInterventionCommand));

        }
    }
}
