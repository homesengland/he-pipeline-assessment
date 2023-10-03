using Elsa.CustomActivities.Activities.Common;
using Elsa.CustomActivities.Constants;
using Elsa.CustomActivities.PropertyDecorator;
using Elsa.CustomActivities.Providers;
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
                QuestionTypeConstants.TextAreaQuestion,
                QuestionTypeConstants.DataTable,
                QuestionTypeConstants.WeightedCheckboxQuestion,
                QuestionTypeConstants.WeightedRadioQuestion
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
                QuestionTypeConstants.TextAreaQuestion,
                QuestionTypeConstants.WeightedCheckboxQuestion,
                QuestionTypeConstants.WeightedRadioQuestion
            })]
        public string QuestionHint { get; set; } = null!;

        [Obsolete("This is only used for backwards compatibility. Use Enhanced Guidance for any new workflows.")]
        public string QuestionGuidance { get; set; } = null!;

        [HeActivityInput(Hint = "New enhanced Question Guidance, which adds certain formatting features, overrides any legacy Question Guidance set for this question as plain text.",
            UIHint = HePropertyUIHints.TextGroup,
            Label = "Question Guidance",
            DefaultSyntax = TextActivitySyntaxNames.TextGroup,
            ExpectedOutputType = ExpectedOutputHints.TextGroup,
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
                        QuestionTypeConstants.TextAreaQuestion,
                        QuestionTypeConstants.WeightedCheckboxQuestion,
                        QuestionTypeConstants.WeightedRadioQuestion
            })]
        public GroupedTextModel EnhancedGuidance { get; set; } = new GroupedTextModel()!;

        [HeActivityInput(Hint = "Include comments box",
            UIHint = HePropertyUIHints.Checkbox,
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
                QuestionTypeConstants.TextAreaQuestion,
                QuestionTypeConstants.WeightedCheckboxQuestion,
                QuestionTypeConstants.WeightedRadioQuestion
            })]
        public bool DisplayComments { get; set; }

        [HeActivityInput(Hint = "Includes evidence box to allow user to provide a link to evidence documents",
            UIHint = HePropertyUIHints.Checkbox,
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
                QuestionTypeConstants.TextAreaQuestion,
                QuestionTypeConstants.WeightedCheckboxQuestion,
                QuestionTypeConstants.WeightedRadioQuestion
            })]
        public bool DisplayEvidenceBox { get; set; }

        [HeActivityInput(Hint = "Character limit", UIHint = HePropertyUIHints.Numeric, DefaultValue = 1000,
            ConditionalActivityTypes = new[] { QuestionTypeConstants.TextAreaQuestion },
            ExpectedOutputType = ExpectedOutputHints.Number)]
        public int? CharacterLimit { get; set; } = 1000;

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

        [HeActivityInput(Hint = "Maximum available score for any combination of answers.", Name = "Max Question Score",
            ConditionalActivityTypes = new[] { QuestionTypeConstants.WeightedCheckboxQuestion })]
        public decimal? MaxScore { get; set; }

        [HeActivityInput(UIHint = HePropertyUIHints.MultiText,
            Hint =
                "The score for the question, based on the corresponding number of questions answered in all groups.  This is not compatible with Group Score Array, and this will always take precedence.",
            Name = "Score Array",
            ConditionalActivityTypes = new[] { QuestionTypeConstants.WeightedCheckboxQuestion }
        )]
        public List<decimal>? ScoreArray { get; set; }

        [HeActivityInput(
            Hint =
                "Set the formula for how the calculation is worked out for the Question.  This overrides all other scoring.",
            Name = "Override standard scoring calculations",
            UIHint = HePropertyUIHints.MultiLine,
            SupportedSyntaxes = new[] { SyntaxNames.JavaScript },
            DefaultSyntax = SyntaxNames.JavaScript,
            ConditionalActivityTypes = new[] { QuestionTypeConstants.WeightedCheckboxQuestion },
            IsDesignerCritical = true)]
        public decimal? Score { get; set; } = null!;

        [HeActivityInput(UIHint = HePropertyUIHints.DataTable,
            ConditionalActivityTypes = new[] { QuestionTypeConstants.DataTable },
            ExpectedOutputType = ExpectedOutputHints.DataTable,
            HasNestedProperties = true)]
        public DataTable DataTable { get; set; } = new DataTable();

        [HeActivityInput(UIHint = HePropertyUIHints.CheckboxOptions,
            ConditionalActivityTypes = new[] { QuestionTypeConstants.CheckboxQuestion },
            ExpectedOutputType = ExpectedOutputHints.Checkbox)]
        public CheckboxModel Checkbox { get; set; } = new CheckboxModel();

        [HeActivityInput(UIHint = HePropertyUIHints.RadioOptions,
            ConditionalActivityTypes = new[] { QuestionTypeConstants.RadioQuestion },
            ExpectedOutputType = ExpectedOutputHints.Radio,
            HasNestedProperties = true)]
        public RadioModel Radio { get; set; } = new RadioModel();

        [HeActivityInput(UIHint = HePropertyUIHints.PotScoreRadioOptions,
            ConditionalActivityTypes = new[] { QuestionTypeConstants.PotScoreRadioQuestion },
            ExpectedOutputType = ExpectedOutputHints.Radio, OptionsProvider = typeof(PotScoreOptionsProvider))]
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
                QuestionTypeConstants.WeightedRadioQuestion,
                QuestionTypeConstants.DataTable
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

        [HeActivityInput(UIHint = HePropertyUIHints.TextGroup, 
            DefaultSyntax = TextActivitySyntaxNames.TextGroup,
            ConditionalActivityTypes = new[] { QuestionTypeConstants.Information }, 
            ExpectedOutputType = ExpectedOutputHints.TextGroup)]
        public GroupedTextModel Text { get; set; } = new GroupedTextModel();


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
                QuestionTypeConstants.WeightedRadioQuestion,
                QuestionTypeConstants.WeightedCheckboxQuestion,
                QuestionTypeConstants.CheckboxQuestion
            })]
        public bool IsReadOnly { get; set; } = false;

        [HeActivityInput(Hint = "Tick if the pre-populated answer should always be re-evaluated",
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
                QuestionTypeConstants.WeightedRadioQuestion,
                QuestionTypeConstants.WeightedCheckboxQuestion,
                QuestionTypeConstants.CheckboxQuestion,
                QuestionTypeConstants.DataTable,
                QuestionTypeConstants.Information
            })]
        public bool ReevaluatePrePopulatedAnswers { get; set; } = false;

        [HeActivityInput(Hint = "Tick if the question should be collapsed on page load",
            UIHint = HePropertyUIHints.Checkbox,
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
                QuestionTypeConstants.WeightedRadioQuestion,
                QuestionTypeConstants.WeightedCheckboxQuestion,
                QuestionTypeConstants.CheckboxQuestion
            })]
        public bool HideQuestion { get; set; } = false;

        [HeActivityInput(Hint = "Set up the question validation rules",
    UIHint = HePropertyUIHints.Validation,
            DefaultSyntax = ValidationSyntaxNames.Validation,
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
                QuestionTypeConstants.WeightedRadioQuestion,
                QuestionTypeConstants.WeightedCheckboxQuestion,
                QuestionTypeConstants.CheckboxQuestion
    })]
        public ValidationModel Validations { get; set; } = new ValidationModel();
    }
}