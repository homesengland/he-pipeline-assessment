using System.ComponentModel.DataAnnotations;

namespace He.PipelineAssessment.UI.Features.Workflow.ViewModels
{
    public class MultiQuestion
    {
        public int Index { get; set; }
        public bool IsValid { get; set; }
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
        public Date? Date { get; set; }
        public decimal? Decimal { get; set; }
        public SingleChoiceModel SingleChoice { get; set; } = new SingleChoiceModel();
        public MultipleChoiceModel MultipleChoice { get; set; } = new MultipleChoiceModel();

    }

    public class MultipleChoiceModel
    {
        public List<string> SelectedChoices { get; set; } = null!;
        public List<Choice> Choices { get; set; } = new List<Choice>();
    }

    public class SingleChoiceModel
    {
        public List<Choice> Choices { get; set; } = new List<Choice>();
        public string SelectedAnswer { get; set; } = null!;
    }


    public class Choice
    {
        public string Answer { get; set; } = null!;
        public bool IsSingle { get; set; }
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
