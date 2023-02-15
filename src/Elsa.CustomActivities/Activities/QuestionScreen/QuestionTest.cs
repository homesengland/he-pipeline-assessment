using Elsa.Attributes;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Describers;
using Elsa.CustomActivities.PropertyDecorator;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Design;
using Elsa.Expressions;
using Elsa.Metadata;
using Esprima.Ast;

namespace Elsa.CustomActivities.Activities.QuestionScreen
{
    public class QuestionTest
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

        public string Comments { get; set; } = null!;


        [HeActivityInput(Hint = "Character limit", UIHint = HePropertyUIHints.SingleLine, ConditionalActivityType = QuestionTypeConstants.TextAreaQuestion)]
        public int? CharacterLimit { get; set; }

        [HeActivityInput(UIHint = HePropertyUIHints.RadioOptions, ConditionalActivityType = QuestionTypeConstants.RadioQuestion)]
        public RadioModel Radio { get; set; } = new RadioModel();

    }


}