using Elsa.CustomModels;

namespace Elsa.Server.Features.Workflow.LoadQuestionScreen
{
    public class LoadQuestionScreenResponse
    {
        public string WorkflowInstanceId { get; set; } = null!;
        public string ActivityId { get; set; } = null!;
        public string ActivityType { get; set; } = null!;
        public string PreviousActivityId { get; set; } = null!;
        public string PreviousActivityType { get; set; } = null!;
        public string? PageTitle { get; set; } = null!;

        public string FooterTitle { get; set; } = null!;
        public string FooterText { get; set; } = null!;

        public List<QuestionActivityData> QuestionScreenAnswers { get; set; } = null!;
    }

    public class QuestionActivityData
    {
        public string ActivityId { get; set; } = null!;
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
        public int? CharacterLimit { get; set; }
        public Checkbox Checkbox { get; set; } = null!;
        public Radio Radio { get; set; } = null!;
        public bool IsReadOnly { get; set; }
    }


    public class Checkbox
    {
        public QuestionScreenAnswer.Choice[] Choices { get; set; } = new List<QuestionScreenAnswer.Choice>().ToArray();

        public List<string> SelectedChoices { get; set; } = null!;
    }

    public class Radio
    {
        public QuestionScreenAnswer.Choice[] Choices { get; set; } = new List<QuestionScreenAnswer.Choice>().ToArray();
        public string SelectedAnswer { get; set; } = null!;
    }

}
