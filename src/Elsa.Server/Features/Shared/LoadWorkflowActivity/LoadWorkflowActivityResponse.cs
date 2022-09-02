namespace Elsa.Server.Features.Shared.LoadWorkflowActivity
{
    public abstract class LoadWorkflowActivityResponse
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
        public string PreviousActivityId { get; set; } = null!;
        public string PreviousActivityType { get; set; } = null!;
    }

    public abstract class QuestionActivityData
    {
        public string Title { get; set; } = null!;
        public string Question { get; set; } = null!;
        public string? QuestionHint { get; set; }
        public string? QuestionGuidance { get; set; }
        public object Output { get; set; } = null!;
    }

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
    //}

    //public class Choice
    //{
    //    public string Answer { get; set; } = null!;
    //    public bool IsSingle { get; set; }
    //    public bool IsSelected { get; set; }
    //}
}
