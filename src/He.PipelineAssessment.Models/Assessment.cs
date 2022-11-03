namespace He.PipelineAssessment.Models
{
    public class Assessment
    {
        public int Id { get; set; }
        public int SpId { get; set; }
        public string SiteName { get; set; } = null!;
        public string ProjectManager { get; set; } = null!;
        public string ProjectManagerEmail { get; set; } = null!;
        public string Counterparty { get; set; } = null!;
        public string Reference { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string? WorkflowInstanceId { get; set; }
    }
}