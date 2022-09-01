namespace Elsa.Server.Features.Workflow.LoadWorkflowActivity
{
    public class LoadWorkflowActivityResponse
    {

        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
        public string PreviousActivityId { get; set; } = null!;

        public MultipleChoiceQuestionActivityData? MultipleChoiceQuestionActivityData { get; set; }
        public CurrencyQuestionActivityData? CurrencyQuestionActivityData { get; set; }
        public TextQuestionActivityData? TextQuestionActivityData { get; set; }
        public DateQuestionActivityData? DateQuestionActivityData { get; set; }

    }

    public class CurrencyQuestionActivityData
    {
        public string Title { get; set; } = null!;
        public string Question { get; set; } = null!;
        public string? QuestionHint { get; set; }
        public string? QuestionGuidance { get; set; }
        public decimal? Answer { get; set; }
        public object Output { get; set; } = null!;
    }

    public class TextQuestionActivityData
    {
        public string Title { get; set; } = null!;
        public string Question { get; set; } = null!;
        public string? QuestionHint { get; set; }
        public string? QuestionGuidance { get; set; }
        public string Answer { get; set; } = null!;
        public object Output { get; set; } = null!;
    }

    public class MultipleChoiceQuestionActivityData
    {
        public string Title { get; set; } = null!;
        public string Question { get; set; } = null!;
        public string? QuestionHint { get; set; }
        public string? QuestionGuidance { get; set; }
        public Choice[] Choices { get; set; } = null!;
        public object? Output { get; set; }
    }

    public class DateQuestionActivityData
    {
        public string Title { get; set; } = null!;
        public string Question { get; set; } = null!;
        public string? QuestionHint { get; set; }
        public string? QuestionGuidance { get; set; }
        public DateTime? Answer { get; set; }
        public object Output { get; set; } = null!;
    }

    public class Choice
    {
        public string Answer { get; set; } = null!;
        public bool IsSingle { get; set; }
        public bool IsSelected { get; set; }
    }
}
