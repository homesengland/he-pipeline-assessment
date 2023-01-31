using FluentValidation;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentTool;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Validators
{
    public class CreateAssessmentToolCommandValidator : AbstractValidator<CreateAssessmentToolCommand>
    {
        public CreateAssessmentToolCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("The name cannot be empty");
            RuleFor(c => c.Order).NotEmpty().GreaterThan(0);
      
        }
    }
}
