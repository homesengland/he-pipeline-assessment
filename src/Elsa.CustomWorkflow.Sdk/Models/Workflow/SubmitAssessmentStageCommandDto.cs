namespace Elsa.CustomWorkflow.Sdk.Models.Workflow
{
    public class SubmitAssessmentStageCommandDto
    {
        public string ActivityId { get; set; } = null!;

        public string WorkflowInstanceId { get; set; } = null!;
    }
}
