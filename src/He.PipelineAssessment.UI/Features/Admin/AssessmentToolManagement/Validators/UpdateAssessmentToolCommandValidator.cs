using FluentValidation;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentTool;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Validators
{
    public class UpdateAssessmentToolCommandValidator : AbstractValidator<UpdateAssessmentToolCommand>
    {
        public UpdateAssessmentToolCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("The {PropertyName} cannot be empty");
            RuleFor(c => c.Order).GreaterThan(0);
        }
    }
}
