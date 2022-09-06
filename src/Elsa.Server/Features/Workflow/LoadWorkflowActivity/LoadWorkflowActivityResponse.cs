namespace Elsa.Server.Features.Workflow.LoadWorkflowActivity
{
    public class LoadWorkflowActivityResponse
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
        public string PreviousActivityId { get; set; } = null!;

        public QuestionActivityData QuestionActivityData { get; set; } = null!;

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
        public object? Output { get; set; }

        public string ActivityType { get; set; } = null!;
        public string? Answer { get; set; }
    }

    //public class CurrencyQuestionActivityData : QuestionActivityData
    //{
    //    public decimal? Answer { get; set; }
    //}

    //public class TextQuestionActivityData : QuestionActivityData
    //{
    //    public string Answer { get; set; } = null!;
    //}

    //public class MultipleChoiceQuestionActivityData : QuestionActivityData
    //{
    //    public Choice[] Choices { get; set; } = null!;
    //}

    //public class DateQuestionActivityData : QuestionActivityData
    //{
    //    public DateTime? Answer { get; set; }
    //    public int? Day { get; set; }
    //    public int? Month { get; set; }
    //    public int? Year { get; set; }

    //}

    //public class Choice
    //{
    //    public string Answer { get; set; } = null!;
    //    public bool IsSingle { get; set; }
    //    public bool IsSelected { get; set; }
    //}
}
