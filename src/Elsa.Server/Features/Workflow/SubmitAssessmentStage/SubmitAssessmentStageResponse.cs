namespace Elsa.Server.Features.Workflow.SubmitAssessmentStage
{
    public class SubmitAssessmentStageResponse
    { 
        public string WorkflowInstanceId { get; set; } = null!;
        public string NextActivityId { get; set; } = null!;
        public string ActivityType { get; set; }
    }
}
