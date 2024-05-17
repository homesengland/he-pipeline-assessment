using FluentValidation;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
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
                .Must((c) => IsAValidFirstWorklowUpdate(c.AssessmentToolId))
                .WithMessage("The Is first Workflow Cannot be updated as this is not a primary assessment tool");
        }

        private bool IsAValidFirstWorklowUpdate(int assessmentToolId)
        {
            var assessmentTool =
                    _assessmentRepository.GetAssessmentTool(assessmentToolId).Result;
            if (assessmentTool != null)
            {
               return  assessmentTool.Order == 1 ? true : false;
            }
            return false;
        }

        private bool BeUnique(string workflowDefinitionId, int id)
        {
            var assessmentToolWorkflow =
                _assessmentRepository.GetAssessmentToolWorkflowByDefinitionId(workflowDefinitionId);
            if (assessmentToolWorkflow == null || assessmentToolWorkflow.Id == id)
                return true;
            return false;
        }
    }
}
