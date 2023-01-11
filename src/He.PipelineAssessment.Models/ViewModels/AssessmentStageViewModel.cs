namespace He.PipelineAssessment.Models.ViewModels
{
    //this maps to the result of stored procedure GetAssessmentStagesByAssessmentId
    public class AssessmentStageViewModel
    {
        public string Name { get; set; } = null!;
        public bool IsVisible { get; set; }
        public int Order { get; set; }
        public int AssessmentId { get; set; }
        public string WorkflowName { get; set; } = null!;
        public string WorkflowDefinitionId { get; set; } = null!;
        public string WorkflowInstanceId { get; set; } = null!;
        public string CurrentActivityId { get; set; } = null!;
        public string CurrentActivityType { get; set; } = null!;
        public string Status { get; set; } = null!;

        public DateTime? CreatedDateTime { get; set; }

        public DateTime? SubmittedDateTime { get; set; }
        public int AssessmentToolId { get; set; }
    }
}
