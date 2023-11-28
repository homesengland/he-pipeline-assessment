namespace Elsa.CustomWorkflow.Sdk
{
    public class ActivityTypeConstants
    {
        public const string QuestionScreen = "QuestionScreen";
        public const string CheckYourAnswersScreen = "CheckYourAnswersScreen";
        public const string ConfirmationScreen = "ConfirmationScreen";
        public const string PotScoreCalculation = "PotScore";

        public const string HousingNeedDataSource = "HousingNeedDataSource";
        public const string PCSProfileDataSource = "PCSProfileDataSource";
        public const string SinglePipelineDataSource = "SinglePipelineDataSource";
        public const string VFMDataSource = "VFMDataSource";
        public const string RegionalIPUDataSource = "RegionalIPUDataSource";
        public const string RegionalFigsDataSource = "RegionalFigsDataSource";
        public const string LandValues = "LandValueDataSource";
        public const string AgricultureLandValues = "AgricultureLandValueDataSource";
        public const string OfficeLandValues = "OfficeLandValueDataSource";
        public const string ReturnToActivity = "ReturnToActivity";
        public const string SetVariable = "SetVariable";
    }

    public class QuestionTypeConstants
    {
        public const string CurrencyQuestion = "CurrencyQuestion";
        public const string PercentageQuestion = "PercentageQuestion";
        public const string DecimalQuestion = "DecimalQuestion";
        public const string IntegerQuestion = "IntegerQuestion";
        public const string CheckboxQuestion = "CheckboxQuestion";
        public const string RadioQuestion = "RadioQuestion";
        public const string DataTable = "DataTable";
        public const string DateQuestion = "DateQuestion";
        public const string TextQuestion = "TextQuestion";
        public const string TextAreaQuestion = "TextAreaQuestion";
        public const string Information = "Information";
        public const string PotScoreRadioQuestion = "PotScoreRadioQuestion";
        public const string WeightedRadioQuestion = "WeightedRadioQuestion";
        public const string WeightedCheckboxQuestion = "WeightedCheckboxQuestion";
    }

    public class DataTableInputTypeConstants
    {
        public const string CurrencyDataTableInput = "Currency";
        public const string DecimalDataTableInput = "Decimal";
        public const string IntegerDataTableInput = "Integer";
        public const string TextDataTableInput = "Text";
    }
    
}
