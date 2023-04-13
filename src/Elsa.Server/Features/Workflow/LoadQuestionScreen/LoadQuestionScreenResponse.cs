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

        public List<QuestionActivityData> Questions { get; set; } = null!;
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
        public int? CharacterLimit { get; set; }
        public Checkbox Checkbox { get; set; } = null!;
        public Radio Radio { get; set; } = null!;
        public Information Information { get; set; } = null!;
        public bool IsReadOnly { get; set; }
        public List<Answer> Answers { get; set; } = new();

        public bool HasAnswers()
        {
            return Answers.Any();
        }
    }


    public class Checkbox
    {
        public QuestionChoice[] Choices { get; set; } = new List<QuestionChoice>().ToArray();

        public List<int> SelectedChoices { get; set; } = null!;
    }

    public class Radio
    {
        public QuestionChoice[] Choices { get; set; } = new List<QuestionChoice>().ToArray();
        public int? SelectedAnswer { get; set; }
    }

    public class Information
    {
        public InformationText[] InformationTextList { get; set; } = new List<InformationText>().ToArray();
    }

}
