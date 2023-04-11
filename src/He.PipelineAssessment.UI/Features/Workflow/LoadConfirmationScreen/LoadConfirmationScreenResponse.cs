using Azure.Core;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;

namespace He.PipelineAssessment.UI.Features.Workflow.LoadConfirmationScreen
{
    public class LoadConfirmationScreenResponse : WorkflowActivityDataDto
    {
        public int AssessmentId { get; set; }
        public string CorrelationId { get; set; } = null!;
        public bool IsCorrectBusinessArea { get; set; }
    }
}
