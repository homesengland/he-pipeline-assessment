using Elsa.Attributes;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.Describers;
using Elsa.Design;
using Elsa.Expressions;
using Elsa.Metadata;
using Esprima.Ast;

namespace Elsa.CustomActivities.Activities.QuestionScreen
{
    public class QuestionTest
    {
        [ActivityInput(Hint = "Question Identifier")]
        public string Id { get; set; } = null!;

        [ActivityInput(Hint = "Section title",
            UIHint = HePropertyUIHints.SingleLine,
            SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.JavaScript })]
        public string Title { get; set; } = null!;

        [ActivityInput(Hint = "Question Identifier", UIHint = HePropertyUIHints.SingleLine)]
        public string QuestionType { get; set; } = null!;

        [ActivityInput(
            Hint = "Question to ask",
            UIHint = HePropertyUIHints.SingleLine,
            SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.JavaScript })]
        public string QuestionText { get; set; } = null!;

        [ActivityInput(Hint = "Question hint", UIHint = HePropertyUIHints.SingleLine)]
        public string QuestionHint { get; set; } = null!;

        [ActivityInput(Hint = "Question guidance", UIHint = HePropertyUIHints.MultiLine)]
        public string QuestionGuidance { get; set; } = null!;
        
        [ActivityInput(Hint = "Include comments box", UIHint = HePropertyUIHints.Checkbox)]
        public bool DisplayComments { get; set; }
        [ActivityInput(Hint = "Comments to display", UIHint = HePropertyUIHints.MultiLine)]
        public string Comments { get; set; } = null!;


        [ActivityInput(Hint = "Character limit", UIHint = HePropertyUIHints.SingleLine)]
        public int? CharacterLimit { get; set; }


    }


}