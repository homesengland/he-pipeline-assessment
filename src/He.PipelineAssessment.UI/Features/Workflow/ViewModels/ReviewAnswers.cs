using Elsa.CustomModels;

namespace He.PipelineAssessment.UI.Features.Workflow.ViewModels
{
    public class ReviewAnswers
    {
        public List<Question>? Questions { get; set; }
        public bool RenderChangeLinks { get; set; }
        public string? WorkflowInstanceId { get; set; }
    }
}
