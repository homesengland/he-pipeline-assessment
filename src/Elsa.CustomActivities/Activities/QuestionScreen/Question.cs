using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.PropertyDecorator;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Expressions;

namespace Elsa.CustomActivities.Activities.QuestionScreen
{
    public class Question
    {
        [HeActivityInput(Hint = "Question Identifier")]
        public string Id { get; set; } = null!;

        [HeActivityInput(Hint = "Section title",
            UIHint = HePropertyUIHints.SingleLine,
            SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.JavaScript })]
        public string Title { get; set; } = null!;

        public string QuestionType { get; set; } = null!;

        [HeActivityInput(
            Hint = "Question to ask",
            UIHint = HePropertyUIHints.SingleLine,
            SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.JavaScript })]
        public string QuestionText { get; set; } = null!;

        [HeActivityInput(Hint = "Question hint", UIHint = HePropertyUIHints.SingleLine)]
        public string QuestionHint { get; set; } = null!;

        [HeActivityInput(Hint = "Question guidance", UIHint = HePropertyUIHints.MultiLine)]
        public string QuestionGuidance { get; set; } = null!;

        [HeActivityInput(Hint = "Include comments box", UIHint = HePropertyUIHints.Checkbox)]
        public bool DisplayComments { get; set; }

        [HeActivityInput(Hint = "Character limit", UIHint = HePropertyUIHints.SingleLine, ConditionalActivityTypes = new[] { QuestionTypeConstants.TextAreaQuestion }, ExpectedOutputType = ExpectedOutputHints.Number)]
        public int? CharacterLimit { get; set; }

        [HeActivityInput(UIHint = HePropertyUIHints.CheckboxOptions, ConditionalActivityTypes = new[] { QuestionTypeConstants.CheckboxQuestion }, ExpectedOutputType = ExpectedOutputHints.Checkbox)]
        public CheckboxModel Checkbox { get; set; } = new CheckboxModel();

        [HeActivityInput(UIHint = HePropertyUIHints.RadioOptions, ConditionalActivityTypes = new[] { QuestionTypeConstants.RadioQuestion }, ExpectedOutputType = ExpectedOutputHints.Radio)]
        public RadioModel Radio { get; set; } = new RadioModel();

        [HeActivityInput(UIHint = HePropertyUIHints.TextActivity, ConditionalActivityTypes = new[] { QuestionTypeConstants.Information }, ExpectedOutputType = ExpectedOutputHints.TextActivity)]
        public TextModel Text { get; set; } = new TextModel();

        [HeActivityInput(Hint = "Fill in to display a pre-populated value", UIHint = HePropertyUIHints.SingleLine,
            SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.JavaScript },
            ConditionalActivityTypes = new[]
            {
                QuestionTypeConstants.TextQuestion,
                QuestionTypeConstants.TextAreaQuestion,
                QuestionTypeConstants.CurrencyQuestion
            })]
        public string? Answer { get; set; }

        [HeActivityInput(Hint = "Tick if the pre-populated answer should be read-only",
            UIHint = HePropertyUIHints.Checkbox,
            ConditionalActivityTypes = new[]
            {
                QuestionTypeConstants.TextQuestion,
                QuestionTypeConstants.TextAreaQuestion,
                QuestionTypeConstants.CurrencyQuestion
            })]
        public bool IsReadOnly { get; set; } = false;

    }
}