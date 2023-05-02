using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.OptionsProviders;
using Elsa.CustomActivities.PropertyDecorator;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Expressions;

namespace Elsa.CustomActivities.Activities.QuestionScreen
{
    public class Question
    {
        [HeActivityInput(Hint = "Question Identifier")]
        public string Id { get; set; } = null!;

        public string QuestionType { get; set; } = null!;

        [HeActivityInput(
            Hint = "Question to ask",
            UIHint = HePropertyUIHints.SingleLine,
            SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.JavaScript },
            ConditionalActivityTypes = new[]
            { 
                QuestionTypeConstants.CurrencyQuestion,
                QuestionTypeConstants.DecimalQuestion,
                QuestionTypeConstants.IntegerQuestion,
                QuestionTypeConstants.PercentageQuestion,
                QuestionTypeConstants.CheckboxQuestion,
                QuestionTypeConstants.RadioQuestion,
                QuestionTypeConstants.PotScoreRadioQuestion,
                QuestionTypeConstants.DateQuestion,
                QuestionTypeConstants.TextQuestion,
                QuestionTypeConstants.TextAreaQuestion
            })]
        public string QuestionText { get; set; } = null!;

        [HeActivityInput(Hint = "Question hint", 
            UIHint = HePropertyUIHints.SingleLine, 
            ConditionalActivityTypes = new[]
            {
                QuestionTypeConstants.CurrencyQuestion,
                QuestionTypeConstants.DecimalQuestion,
                QuestionTypeConstants.IntegerQuestion,
                QuestionTypeConstants.PercentageQuestion,
                QuestionTypeConstants.CheckboxQuestion, 
                QuestionTypeConstants.RadioQuestion, 
                QuestionTypeConstants.PotScoreRadioQuestion, 
                QuestionTypeConstants.DateQuestion, 
                QuestionTypeConstants.TextQuestion, 
                QuestionTypeConstants.TextAreaQuestion
            })]
        public string QuestionHint { get; set; } = null!;

        [HeActivityInput(Hint = "Question guidance", 
            UIHint = HePropertyUIHints.MultiLine, 
            ConditionalActivityTypes = new[] 
                { QuestionTypeConstants.CurrencyQuestion,
                    QuestionTypeConstants.DecimalQuestion,
                    QuestionTypeConstants.IntegerQuestion,
                    QuestionTypeConstants.PercentageQuestion,
                    QuestionTypeConstants.CheckboxQuestion, 
                    QuestionTypeConstants.RadioQuestion, 
                    QuestionTypeConstants.PotScoreRadioQuestion, 
                    QuestionTypeConstants.DateQuestion, 
                    QuestionTypeConstants.TextQuestion, 
                    QuestionTypeConstants.TextAreaQuestion
                })]
        public string QuestionGuidance { get; set; } = null!;

        [HeActivityInput(Hint = "Include comments box", UIHint = HePropertyUIHints.Checkbox)]
        public bool DisplayComments { get; set; }

        [HeActivityInput(Hint = "Character limit", UIHint = HePropertyUIHints.SingleLine, ConditionalActivityTypes = new[] { QuestionTypeConstants.TextAreaQuestion }, ExpectedOutputType = ExpectedOutputHints.Number)]
        public int? CharacterLimit { get; set; }

        [HeActivityInput(Hint = "Question Weighting",
            UIHint = HePropertyUIHints.SingleLine,
            ConditionalActivityTypes = new[]
            {
                QuestionTypeConstants.PotScoreRadioQuestion,
                QuestionTypeConstants.WeightedCheckboxQuestion,
                QuestionTypeConstants.WeightedRadioQuestion
            },
            ExpectedOutputType = ExpectedOutputHints.Double)]
        public double QuestionWeighting { get; set; }

        [HeActivityInput(UIHint = HePropertyUIHints.CheckboxOptions, ConditionalActivityTypes = new[] { QuestionTypeConstants.CheckboxQuestion }, ExpectedOutputType = ExpectedOutputHints.Checkbox)]
        public CheckboxModel Checkbox { get; set; } = new CheckboxModel();

        [HeActivityInput(UIHint = HePropertyUIHints.RadioOptions, ConditionalActivityTypes = new[] { QuestionTypeConstants.RadioQuestion }, ExpectedOutputType = ExpectedOutputHints.Radio)]
        public RadioModel Radio { get; set; } = new RadioModel();

        [HeActivityInput(UIHint = HePropertyUIHints.PotScoreRadioOptions, ConditionalActivityTypes = new[] { QuestionTypeConstants.PotScoreRadioQuestion }, ExpectedOutputType = ExpectedOutputHints.Radio, OptionsProvider = typeof(PotScoreOptionsProvider))]
        public PotScoreRadioModel PotScoreRadio { get; set; } = new PotScoreRadioModel();

        [HeActivityInput(UIHint = HePropertyUIHints.QuestionDataDictionary,
            DefaultSyntax = SyntaxNames.Literal,
            Hint = "Assigns data dictionary item to this question for reporting purposes",
            ConditionalActivityTypes = new[]
            {
                QuestionTypeConstants.TextQuestion,
                QuestionTypeConstants.TextAreaQuestion,
                QuestionTypeConstants.CurrencyQuestion,
                QuestionTypeConstants.DecimalQuestion,
                QuestionTypeConstants.IntegerQuestion,
                QuestionTypeConstants.PercentageQuestion,
                QuestionTypeConstants.DateQuestion,
                QuestionTypeConstants.RadioQuestion,
                QuestionTypeConstants.PotScoreRadioQuestion,
                QuestionTypeConstants.CheckboxQuestion,
                QuestionTypeConstants.WeightedCheckboxQuestion,
                QuestionTypeConstants.WeightedRadioQuestion
            })]
        public int? DataDictionary { get; set; }

        [HeActivityInput(UIHint = HePropertyUIHints.WeightedRadioOptions,
            ConditionalActivityTypes = new[] { QuestionTypeConstants.WeightedRadioQuestion },
            ExpectedOutputType = ExpectedOutputHints.WeightedRadio)]
        public WeightedRadioModel WeightedRadio { get; set; } = new WeightedRadioModel();

        [HeActivityInput(UIHint = HePropertyUIHints.WeightedCheckboxOptions,
            ConditionalActivityTypes = new[] { QuestionTypeConstants.WeightedCheckboxQuestion },
            ExpectedOutputType = ExpectedOutputHints.WeightedCheckbox)]
        public WeightedCheckboxModel WeightedCheckbox { get; set; } = new WeightedCheckboxModel();

        [HeActivityInput(UIHint = HePropertyUIHints.TextActivity, ConditionalActivityTypes = new[] { QuestionTypeConstants.Information }, ExpectedOutputType = ExpectedOutputHints.TextActivity)]
        public TextModel Text { get; set; } = new TextModel();

        [HeActivityInput(Hint = "Fill in to display a pre-populated value", UIHint = HePropertyUIHints.SingleLine,
            SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.JavaScript },
            ConditionalActivityTypes = new[]
            {
                QuestionTypeConstants.TextQuestion,
                QuestionTypeConstants.TextAreaQuestion,
                QuestionTypeConstants.CurrencyQuestion,
                QuestionTypeConstants.DecimalQuestion,
                QuestionTypeConstants.IntegerQuestion,
                QuestionTypeConstants.PercentageQuestion
            })]
        public string? Answer { get; set; }

        [HeActivityInput(Hint = "Tick if the pre-populated answer should be read-only",
            UIHint = HePropertyUIHints.Checkbox,
            ConditionalActivityTypes = new[]
            {
                QuestionTypeConstants.TextQuestion,
                QuestionTypeConstants.TextAreaQuestion,
                QuestionTypeConstants.CurrencyQuestion,
                QuestionTypeConstants.DecimalQuestion,
                QuestionTypeConstants.IntegerQuestion,
                QuestionTypeConstants.PercentageQuestion,
                QuestionTypeConstants.RadioQuestion,
                QuestionTypeConstants.PotScoreRadioQuestion,
                QuestionTypeConstants.CheckboxQuestion
            })]
        public bool IsReadOnly { get; set; } = false;

    }
}