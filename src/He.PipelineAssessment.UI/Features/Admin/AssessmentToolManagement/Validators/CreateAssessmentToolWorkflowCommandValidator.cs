﻿using FluentValidation;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolWorkflow;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Validators
{
    public class CreateAssessmentToolWorkflowCommandValidator : AbstractValidator<CreateAssessmentToolWorkflowCommand>
    {
        public CreateAssessmentToolWorkflowCommandValidator()
        {
            RuleFor(c => c.WorkflowDefinitionId).NotEmpty().WithMessage("The {PropertyName} cannot be empty");
            RuleFor(c => c.Name).NotEmpty().WithMessage("The name cannot be empty").MaximumLength(100);
        }
    }
}
