namespace Elsa.CustomWorkflow.Sdk.Models.Workflow.Archive
{
    public class WorkflowActivityDataDto<T>
    {
        public WorkflowActivityData<T> Data { get; set; } = null!;
        public bool IsValid { get; set; }
        public IList<string> ValidationMessages { get; set; } = new List<string>();
    }

    public class WorkflowActivityData<T>
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
        public string PreviousActivityId { get; set; } = null!;
        public T? ActivityData { get; set; }

        //public MultipleChoiceQuestionActivityData? MultipleChoiceQuestionActivityData { get; set; }
        //public CurrencyQuestionActivityData? CurrencyQuestionActivityData { get; set; }
        //public TextQuestionActivityData? TextQuestionActivityData { get; set; }
        //public DateQuestionActivityData? DateQuestionActivityData { get; set; }
    }

    public abstract class QuestionActivityData
    {
        public string Title { get; set; } = null!;
        public string Question { get; set; } = null!;
        public string? QuestionHint { get; set; }
        public string? QuestionGuidance { get; set; }
        public object Output { get; set; } = null!;
    }

    public class CurrencyQuestionActivityData : QuestionActivityData
    {
        public decimal? Answer { get; set; }
    }

    public class TextQuestionActivityData : QuestionActivityData
    {
        public string Answer { get; set; } = null!;
    }

    public class MultipleChoiceQuestionActivityData : QuestionActivityData
    {
        public Choice[] Choices { get; set; } = null!;
    }

    public class DateQuestionActivityData : QuestionActivityData
    {
        public DateTime? Answer { get; set; }

        public int? DayFromDate()
        {
            if (Answer.HasValue)
            {
                return Answer.Value.Day;
            }
            return null;
        }
        public int? MonthFromDate()
        {
            if (Answer.HasValue)
            {
                return Answer.Value.Month;
            }
            return null;
        }
        public int? YearFromDate()
        {
            if (Answer.HasValue)
            {
                return Answer.Value.Year;
            }
            return null;
        }
    }

    public class Choice
    {
        public string Answer { get; set; } = null!;
        public bool IsSingle { get; set; }
        public bool IsSelected { get; set; }
    }
}
