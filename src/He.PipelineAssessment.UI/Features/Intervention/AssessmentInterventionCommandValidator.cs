﻿using FluentValidation;
using He.PipelineAssessment.UI.Features.Override.CreateOverride;
using He.PipelineAssessment.UI.Features.Override.EditOverride;
using He.PipelineAssessment.UI.Features.Rollback.CreateRollback;
using He.PipelineAssessment.UI.Features.Rollback.EditRollbackAssessor;

namespace He.PipelineAssessment.UI.Features.Intervention
{
    public class AssessmentInterventionCommandValidator : AbstractValidator<AssessmentInterventionCommand>
    {
        public AssessmentInterventionCommandValidator()
        {

            RuleFor(c => c.SignOffDocument).NotEmpty().WithMessage("The {PropertyName} cannot be empty")
                .When(x => x.GetType() == typeof(CreateOverrideCommand) || x.GetType() == typeof(EditOverrideCommand));
            RuleFor(c => c.AdministratorRationale).NotEmpty().WithMessage("The {PropertyName} cannot be empty")
                .When(x => x.GetType() == typeof(CreateOverrideCommand) || x.GetType() == typeof(EditOverrideCommand));
            RuleFor(c => c.TargetWorkflowId).NotEmpty().WithMessage("The target workflow definition has to be selected")
                .When(x => x.GetType() == typeof(CreateOverrideCommand) || x.GetType() == typeof(EditOverrideCommand));
            RuleFor(c => c.AssessorRationale).NotEmpty().WithMessage("The {PropertyName} cannot be empty")
                .When(x => x.GetType() == typeof(CreateRollbackCommand) || x.GetType() == typeof(EditRollbackAssessorCommand));
        }
    }
}