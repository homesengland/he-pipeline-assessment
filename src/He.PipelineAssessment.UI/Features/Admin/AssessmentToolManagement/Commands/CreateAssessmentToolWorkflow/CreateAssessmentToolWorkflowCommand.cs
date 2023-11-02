using MediatR;
using System.ComponentModel.DataAnnotations;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolWorkflow
{
    public class CreateAssessmentToolWorkflowDto
    {
        public int AssessmentToolId { get; set; }

        public CreateAssessmentToolWorkflowCommand CreateAssessmentToolWorkflowCommand { get; set; } = new();
        public ValidationResult? ValidationResult { get; set; }

    }
    public class CreateAssessmentToolWorkflowCommand : IRequest<int>
    {
        public string Name { get; set; } = string.Empty;
        public string WorkflowDefinitionId { get; set; } = string.Empty;
        [Display(Name = "Is first workflow?")]
        public bool IsFirstWorkflow { get; set; } = false;
        [Display(Name = "Is economist workflow?")]
        public bool IsEconomistWorkflow { get; set; } = false;
        [Display(Name = "Is ammendable workflow?")]
        public bool IsAmendableWorkflow { get; set; } = false;
        public int AssessmentToolId { get; set; }
        public int Version { get; set; } = 1;
        public bool IsLatest { get; set; } = true;
        [Display(Name = "Is variation?")]
        public bool IsVariation { get; set; } = false;

        public bool IsLast { get; set; }
    }
}
