using FluentValidation;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
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

            RuleFor(c => c.AssessmentFundId)
                .Must((command, assessmentFundId) => !command.IsEarlyStage || !assessmentFundId.HasValue)
                .WithMessage("Funds cannot be assigned to assessment tool workflows that are marked as Early Stage. " +
                    "Please ensure that no fund is selected when creating an Early Stage workflow.");

            RuleFor(c => c.AssessmentFundId)
                .Must((command, assessmentFundId) => command.IsEarlyStage || (assessmentFundId.HasValue && assessmentFundId.Value > 0))
                .WithMessage("A fund must be assigned to assessment tool workflows that are not marked as Early Stage.");
        }

        private bool BeUnique(string workflowDefinitionId)
        {
            if (_assessmentRepository.GetAssessmentToolWorkflowByDefinitionId(workflowDefinitionId) == null)
                return true;
            return false;
        }

        private bool FollowIsEarlyStageRule(bool isEarlyStage)
        {
            if (isEarlyStage == true)
            {
                return false;
            }
            return true;
        }

        private bool FollowFundRule(bool isEarlyStage, int? assessmentFundId)
        {
            if (!isEarlyStage && (!assessmentFundId.HasValue || assessmentFundId == 0)) 
            {
                return false;
            }
            return true;
        }
    }
}
