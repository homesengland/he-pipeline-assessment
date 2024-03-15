using Elsa.CustomWorkflow.Sdk.Models.Workflow;

namespace He.PipelineAssessment.UI.Integration.ServiceBusSend
{
    public class AssessmentDTO
    {
        public int ProjectId { get; set; }
        public string? Name { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? SubmittedDateTime { get; set; }
        public string? SubmittedBy { get; set; }
        public int AssessmentToolId { get; set; }
        public int InstanceId { get; set; }
        public string? WorkflowInstanceId { get; set; }
        public string? Status { get; set; }
        public string? Result { get; set; }
    }
}
