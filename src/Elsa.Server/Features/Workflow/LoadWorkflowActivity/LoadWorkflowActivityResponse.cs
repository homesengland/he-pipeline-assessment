namespace Elsa.Server.Features.Workflow.LoadWorkflowActivity
{
    public class LoadWorkflowActivityResponse
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
        public string PreviousActivityId { get; set; } = null!;

        public string? PageTitle { get; set; } = null!;

        public QuestionActivityData QuestionActivityData { get; set; } = null!;

        public List<QuestionActivityData> MultiQuestionActivityData { get; set; } = null!;

    }

    public class QuestionActivityData
    {
        public string? QuestionId { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Question { get; set; } = null!;
        public string? QuestionHint { get; set; }
        public string? QuestionGuidance { get; set; }
        public bool DisplayComments { get; set; }
        public string? Comments { get; set; }
        public object? Output { get; set; }

        public string? QuestionType { get; set; } = null!;
        public string? Answer { get; set; }
        public Checkbox Checkbox { get; set; } = null!;
        public Radio Radio { get; set; } = null!;
    }


    public class Checkbox
    {
        public Choice[] Choices { get; set; } = new List<Choice>().ToArray();

        public List<string> SelectedChoices { get; set; } = null!;
    }

    public class Choice
    {
        public string Answer { get; set; } = null!;
        public bool IsSingle { get; set; }
    }

    public class Radio
    {
        public Choice[] Choices { get; set; } = new List<Choice>().ToArray();
        public string SelectedAnswer { get; set; } = null!;
    }

}
