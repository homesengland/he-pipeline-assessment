using He.PipelineAssessment.UI.Features.Assessment.AssessmentList;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentTool
{
    public class AssessmentToolData
    {
        public List<string> ValidationMessages { get; set; } = new List<string>();
        public bool IsValid { get { return ValidationMessages != null && ValidationMessages.Count == 0; } }
        public List<AssessmentToolDto> AssessmentTools { get; set; } = new List<AssessmentToolDto>();
    }

    public class AssessmentToolDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }

        public List<AssessmentToolWorkflowDto> AssessmentToolWorkFlows { get; set; }
    }

    public class AssessmentToolWorkflowDto
    {
        public int Id { get; set; }
        public int AssessmentToolId { get; set; }
        public string WorkflowDefinitionId { get; set; }
        public bool IsFirstWorkflow { get; set; }
        public int Version { get; set; }
        public bool IsLatest  { get; set;}

    }
}



 