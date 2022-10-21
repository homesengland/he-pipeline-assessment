using Elsa.CustomModels;

namespace Elsa.Server.Features.Workflow.LoadWorkflowActivity
{
    public class LoadWorkflowActivityResponse
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
        public string PreviousActivityId { get; set; } = null!;
        public string PreviousActivityInstanceId { get; set; } = null!;

        public QuestionActivityData QuestionActivityData { get; set; } = null!;
        public List<AssessmentQuestion> AssessmentQuestions { get; set; } = null!;
        public string FooterTitle { get; set; }
        public string FooterText { get; set; }
    }

    public class QuestionActivityData
    {
        public string Title { get; set; } = null!;
        public string Question { get; set; } = null!;
        public string? QuestionHint { get; set; }
        public string? QuestionGuidance { get; set; }
        public bool DisplayComments { get; set; }
        public string? Comments { get; set; }
        public object? Output { get; set; }

        public string ActivityType { get; set; } = null!;
        public string? Answer { get; set; }
        public MultipleChoiceModel MultipleChoice { get; set; } = null!;
        public SingleChoiceModel SingleChoice { get; set; } = null!;
    }


    public class MultipleChoiceModel
    {
        public Choice[] Choices { get; set; } = new List<Choice>().ToArray();

        public List<string> SelectedChoices { get; set; } = null!;
    }

    public class Choice
    {
        public string Answer { get; set; } = null!;
        public bool IsSingle { get; set; }
    }

    public class SingleChoiceModel
    {
        public SingleChoice[] Choices { get; set; } = new List<SingleChoice>().ToArray();
        public string SelectedAnswer { get; set; } = null!;
    }

    public class SingleChoice
    {
        public string Answer { get; set; } = null!;
    }

}
