using FluentValidation.Results;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentToolWorkflows
{
    public class AssessmentToolWorkflowListDto
    {
        public int AssessmentToolId { get; set; }
        public string AssessmentToolName { get; set; } = string.Empty;

        public List<AssessmentToolWorkflowDto> AssessmentToolWorkflowDtos { get; set; } = new();
    }

    public class AssessmentToolWorkflowDto
    {
        public int Id { get; set; }
        public int AssessmentToolId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string WorkflowDefinitionId { get; set; } = string.Empty;
        public bool IsFirstWorkflow { get; set; }
        public int Version { get; set; }
        public bool IsLatest { get; set; }
        public ValidationResult? ValidationResult { get; set; }
    }
}