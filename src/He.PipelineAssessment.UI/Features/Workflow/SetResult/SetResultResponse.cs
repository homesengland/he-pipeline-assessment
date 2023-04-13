using Elsa.CustomWorkflow.Sdk.Models.Workflow;

namespace He.PipelineAssessment.UI.Features.Workflow.SetResult
{
    public class SetResultResponse : WorkflowActivityDataDto
    {
        public int AssessmentId { get; set; }
        public string CorrelationId { get; set; } = null!;
    }
}
