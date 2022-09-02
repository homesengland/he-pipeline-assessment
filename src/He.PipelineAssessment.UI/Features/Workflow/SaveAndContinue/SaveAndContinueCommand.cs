using He.PipelineAssessment.UI.Features.Workflow.LoadWorkflowActivity;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue
{
    public class SaveAndContinueCommand : IRequest<LoadWorkflowActivityRequest>
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
        public CurrencyQuestionActivityData CurrencyQuestionActivityData { get; set; } = null!;
        public MultipleChoiceQuestionActivityData MultipleChoiceQuestionActivityData { get; set; } = null!;
        public TextQuestionActivityData TextQuestionActivityData { get; set; } = null!;
        public DateQuestionActivityData DateQuestionActivityData { get; set; } = null!;
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
    public class MultipleChoiceQuestionActivityData : QuestionActivityData
    {
        public Choice[] Choices { get; set; } = null!;
    }

    public class TextQuestionActivityData : QuestionActivityData
    {
        public string Answer { get; set; } = null!;
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


        public DateTime? GetAnswer()
        {
            if (Day.HasValue && Month.HasValue && Year.HasValue)
            {
                return new DateTime(Year.Value, Month.Value, Day.Value);
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
