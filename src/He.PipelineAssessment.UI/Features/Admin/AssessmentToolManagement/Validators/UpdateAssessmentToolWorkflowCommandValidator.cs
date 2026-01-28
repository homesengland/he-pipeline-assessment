using FluentValidation;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolWorkflow;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentToolWorkflowCommand;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Validators
{
    public class UpdateAssessmentToolWorkflowCommandValidator : AbstractValidator<UpdateAssessmentToolWorkflowCommand>
    {

        private readonly IAssessmentRepository _assessmentRepository;
        public UpdateAssessmentToolWorkflowCommandValidator(IAssessmentRepository assessmentRepository)
        {
            this._assessmentRepository = assessmentRepository;

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("The {PropertyName} cannot be empty").MaximumLength(100);

            RuleFor(c => c.WorkflowDefinitionId)
                .NotEmpty()
                .WithMessage("The {PropertyName} cannot be empty");

            RuleFor(c => c)
                .Must((c) => BeUnique(c.WorkflowDefinitionId, c.Id))
                .WithMessage("The Workflow Definition Id must be unique and not used in another Assessment Tool Workflow");

            RuleFor(c => c)
                .Must(FollowIsEarlyStageRule)
                .WithMessage("Funds can only be assigned to assessment tool workflows that are not marked as Early Stage. " +
                "Please ensure that 'No Fund' is selected when creating an Early Stage workflow.");

            RuleFor(c => c)
                .Must(FollowFundRule)
                .WithMessage("A fund must be assigned to assessment tool workflows that are not marked as Early Stage.");
        }

        private bool BeUnique(string workflowDefinitionId, int id)
        {
            var assessmentToolWorkflow =
                _assessmentRepository.GetAssessmentToolWorkflowByDefinitionId(workflowDefinitionId);
            if (assessmentToolWorkflow == null || assessmentToolWorkflow.Id == id)
                return true;
            return false;
        }
        private bool FollowIsEarlyStageRule(UpdateAssessmentToolWorkflowCommand command)
        {
            if (command.IsEarlyStage)
            {
                return !command.AssessmentFundId.HasValue || command.AssessmentFundId.Value == 0;
            }
            return true;
        }

        private bool FollowFundRule(UpdateAssessmentToolWorkflowCommand command)
        {
            if (!command.IsEarlyStage)
            {
                return command.AssessmentFundId.HasValue && command.AssessmentFundId.Value != 0;
            }
            return true;
        }
    }
}
