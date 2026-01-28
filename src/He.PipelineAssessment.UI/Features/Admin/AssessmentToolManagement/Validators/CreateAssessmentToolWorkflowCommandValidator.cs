using FluentValidation;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolWorkflow;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

            RuleFor(c => c)
                .Must(FollowIsEarlyStageRule)
                .WithMessage("Funds can only be assigned to assessment tool workflows that are not marked as Early Stage. " +
                "Please ensure that 'No Fund' is selected when creating an Early Stage workflow.");

            RuleFor(c => c)
                .Must(FollowFundRule)
                .WithMessage("A fund must be assigned to assessment tool workflows that are not marked as Early Stage.");
        }

        private bool BeUnique(string workflowDefinitionId)
        {
            if (_assessmentRepository.GetAssessmentToolWorkflowByDefinitionId(workflowDefinitionId) == null)
                return true;
            return false;
        }

        private bool FollowIsEarlyStageRule(CreateAssessmentToolWorkflowCommand command)
        { 
            if (command.IsEarlyStage)
            {
                return !command.AssessmentFundId.HasValue || command.AssessmentFundId.Value == 0;
            }
            return true;
        }

        private bool FollowFundRule(CreateAssessmentToolWorkflowCommand command)
        {
            if (!command.IsEarlyStage)
            {
                return command.AssessmentFundId.HasValue && command.AssessmentFundId.Value != 0;
            }
            return true;
        }
    }
}
