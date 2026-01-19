using FluentValidation;
using He.PipelineAssessment.Infrastructure.Repository;
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

            RuleFor(c => c.IsEarlyStage)
                .Must(FollowIsEarlyStageRule)
                .WithMessage("Funds can only be assigned to assessment tool workflows that are not marked as Early Stage. " +
                "Please ensure that no fund is selected when creating an Early Stage workflow.");

            RuleFor(c => c.AssessmentFundId)
                .Must((command, assessmentFundId) => FollowFundRule(command.IsEarlyStage, assessmentFundId))
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
