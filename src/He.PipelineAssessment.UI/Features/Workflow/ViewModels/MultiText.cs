using System.ComponentModel.DataAnnotations;

namespace He.PipelineAssessment.UI.Features.Workflow.ViewModels
{
    public class MultiQuestion
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
        public Date Date { get; set; }
    }

    public class Date
    {
        [Range(1, 31)]
        public int? Day { get; set; }
        [Range(1, 12)]
        public int? Month { get; set; }
        [Range(1, 3000)]
        public int? Year { get; set; }
    }
}
