using Elsa.Attributes;
using Elsa.Design;
using Elsa.Expressions;

namespace Elsa.CustomActivities.Activities.QuestionScreen
{
    public class Question
    {
        [ActivityInput(Hint = "Question Identifier")]
        public string Id { get; set; } = null!;

        [ActivityInput(Hint = "Section title")]
        public string Title { get; set; } = null!;

        [ActivityInput(Hint = "Question Identifier")]
        public string QuestionType { get; } = null!;

        [ActivityInput(
            Hint = "Question to ask",
            UIHint = ActivityInputUIHints.SingleLine,
            DefaultSyntax = SyntaxNames.Literal,
            SupportedSyntaxes = new[] { SyntaxNames.Literal })]
        public string QuestionText { get; set; } = null!;

        [ActivityInput(Hint = "Question hint", UIHint = ActivityInputUIHints.SingleLine)]
        public string QuestionHint { get; set; } = null!;

        [ActivityInput(Hint = "Question guidance", UIHint = ActivityInputUIHints.MultiLine)]
        public string QuestionGuidance { get; set; } = null!;

        [ActivityInput(Hint = "Include comments box", UIHint = ActivityInputUIHints.Checkbox)]
        public bool DisplayComments { get; set; }
        public string Comments { get; set; } = null!;

        public CheckboxModel Checkbox { get; set; } = new CheckboxModel();
        public RadioModel Radio { get; set; } = new RadioModel();

    }

    public class CheckboxModel
    {
        public ICollection<CheckboxRecord> Choices { get; set; } = new List<CheckboxRecord>();
    }

    public record CheckboxRecord(string Answer, bool IsSingle);

    public class RadioModel
    {
        public ICollection<RadioRecord> Choices { get; set; } = new List<RadioRecord>();
    }

    public record RadioRecord(string Answer);
}