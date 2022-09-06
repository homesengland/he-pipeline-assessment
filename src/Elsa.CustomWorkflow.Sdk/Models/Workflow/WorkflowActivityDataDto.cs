using System.ComponentModel.DataAnnotations;

namespace Elsa.CustomWorkflow.Sdk.Models.Workflow
{
    public class WorkflowActivityDataDto
    {
        public WorkflowActivityData Data { get; set; } = null!;
        public bool IsValid { get; set; }
        public IList<string> ValidationMessages { get; set; } = new List<string>();
    }

    public class WorkflowActivityData
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
        public string PreviousActivityId { get; set; } = null!;

        public QuestionActivityData? QuestionActivityData { get; set; }

        //public MultipleChoiceQuestionActivityData? MultipleChoiceQuestionActivityData { get; set; }
        //public CurrencyQuestionActivityData? CurrencyQuestionActivityData { get; set; }
        //public TextQuestionActivityData? TextQuestionActivityData { get; set; }
        //public DateQuestionActivityData? DateQuestionActivityData { get; set; }
    }

    public class QuestionActivityData
    {
        public string Title { get; set; } = null!;
        public string Question { get; set; } = null!;
        public string? QuestionHint { get; set; }
        public string? QuestionGuidance { get; set; }
        public object Output { get; set; } = null!;
        public string? Answer { get; set; }
    }

    public class CurrencyQuestionActivityData : QuestionActivityData
    {
        public decimal? Answer { get; set; }
    }

    public class TextQuestionActivityData : QuestionActivityData
    {
        public string? Answer { get; set; } = null!;
    }

    public class MultipleChoiceQuestionActivityData : QuestionActivityData
    {
        public Choice[] Choices { get; set; } = null!;
    }

    public class DateQuestionActivityData : QuestionActivityData
    {
        public DateTime? Answer { get; set; }

        [Range(1, 31)]
        public int? Day { get; set; }
        [Range(1, 12)]
        public int? Month { get; set; }
        [Range(1, 3000)]
        public int? Year { get; set; }
    }

    public class Choice
    {
        public string Answer { get; set; } = null!;
        public bool IsSingle { get; set; }
        public bool IsSelected { get; set; }
    }
}

