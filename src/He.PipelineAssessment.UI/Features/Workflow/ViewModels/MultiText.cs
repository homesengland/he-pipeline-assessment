namespace He.PipelineAssessment.UI.Features.Workflow.ViewModels
{
    public class MultiText
    {
        public int Index { get; set; }
        public string QuestionId { get; set; } = null!;
        public string QuestionType { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Question { get; set; } = null!;
        public string? QuestionHint { get; set; }
        public string? QuestionGuidance { get; set; }
        public bool DisplayComments { get; set; }
        public string? Comments { get; set; }
        public string? Answer { get; set; }
    }
}
