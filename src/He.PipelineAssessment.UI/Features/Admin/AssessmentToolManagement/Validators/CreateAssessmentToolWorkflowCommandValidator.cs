using FluentValidation;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolWorkflow;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Validators
{
    public class CreateAssessmentToolWorkflowCommandValidator : AbstractValidator<CreateAssessmentToolWorkflowCommand>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        public CreateAssessmentToolWorkflowCommandValidator(IAssessmentRepository assessmentRepository)
        {
            this._assessmentRepository = assessmentRepository;

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("The {PropertyName} cannot be empty").MaximumLength(100);

            RuleFor(c => c.WorkflowDefinitionId)
                .NotEmpty()
                .WithMessage("The {PropertyName} cannot be empty");

            RuleFor(c => c.WorkflowDefinitionId)
                .Must(BeUnique)
                .WithMessage("The {PropertyName} must be unique and not used in another Assessment Tool Workflow");
        }

        private bool BeUnique(string workflowDefinitionId)
        {
            if (_assessmentRepository.GetAssessmentToolWorkflowByDefinitionId(workflowDefinitionId) == null)
                return true;
            return false;
        }
    }
}
