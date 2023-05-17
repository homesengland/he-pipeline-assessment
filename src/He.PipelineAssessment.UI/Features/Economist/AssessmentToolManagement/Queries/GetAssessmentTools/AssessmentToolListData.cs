using FluentValidation.Results;

namespace He.PipelineAssessment.UI.Features.Economist.AssessmentToolManagement.Queries.GetAssessmentTools
{
    public class AssessmentToolListData
    {
        public List<AssessmentToolDto> AssessmentTools { get; set; } = new();
    }

    public class AssessmentToolDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Order { get; set; }
        public ValidationResult? ValidationResult { get; set; }
        public List<AssessmentToolWorkflowDto> AssessmentToolWorkFlows { get; set; } = new();
    }

    public class AssessmentToolWorkflowDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int AssessmentToolId { get; set; }
        public string WorkflowDefinitionId { get; set; } = string.Empty;
        public bool IsFirstWorkflow { get; set; }
        public bool IsEconomistWorkflow { get; set; }
        public int Version { get; set; }
        public bool IsLatest { get; set; }

    }

}



