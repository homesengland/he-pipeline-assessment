using He.PipelineAssessment.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
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
        public string Category { get; set; } = string.Empty;
        [Display(Name = "Is Latest Version?")]
        public bool IsLatest { get; set; } = true;
        [Display(Name = "Is First Workflow?")]
        public bool IsFirstWorkflow { get; set; } = false;
        [Display(Name = "Is Economist Workflow?")]
        public bool IsEconomistWorkflow { get; set; } = false;
        [Display(Name = "Is Ammendable Workflow?")]
        public bool IsAmendableWorkflow { get; set; } = false;
        public int AssessmentToolId { get; set; }
        public int Version { get; set; } = 1;
        [Display(Name = "Is Variation?")]
        public bool IsVariation { get; set; } = false;
        [Display(Name = "Is Early Stage?")]
        public bool IsEarlyStage { get; set; } = false;
        [Display(Name = "Is Last?")]
        public bool IsLast { get; set; }

        public string SelectedOption { get;set; } = string.Empty;
        public IEnumerable<SelectListItem> Options { get; set; } = new List<SelectListItem>();
    }
}
