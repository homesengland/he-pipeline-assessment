class Uri {
}
Uri.LibUri = "defaultLib:lib.es6.d.ts";
class StoreStatus {
}
StoreStatus.Fetching = "fetching";
StoreStatus.Available = "available";
StoreStatus.Empty = "empty";
class SyntaxNames {
}
SyntaxNames.Literal = "Literal";
SyntaxNames.JavaScript = "JavaScript";
SyntaxNames.Liquid = "Liquid";
SyntaxNames.Json = "Json";
SyntaxNames.SQL = "SQL";
SyntaxNames.Switch = "Switch";
SyntaxNames.Question = "Question";
SyntaxNames.QuestionList = "QuestionList";
SyntaxNames.Checked = "Checked";
SyntaxNames.ConditionalTextList = 'ConditionalTextList';
SyntaxNames.TextActivity = 'TextActivity';
SyntaxNames.TextActivityList = 'TextActivityList';
SyntaxNames.DataDictionary = 'DataDictionary';
SyntaxNames.DataTable = 'DataTable';
SyntaxNames.InformationText = 'TextActivity';
SyntaxNames.GroupedInformationText = 'TextGroupActivity';
class QuestionCategories {
}
QuestionCategories.None = "None";
QuestionCategories.Question = "Question";
QuestionCategories.Scoring = "Scoring";
class DataTableSyntax {
}
DataTableSyntax.Readonly = "ReadOnly";
DataTableSyntax.Identifier = "Identifier";
DataTableSyntax.InputType = "InputType";
DataTableSyntax.Input = "Input";
DataTableSyntax.TableHeader = "TableHeader";
DataTableSyntax.InputHeader = "InputHeader";
DataTableSyntax.Title = "Title";
DataTableSyntax.DisplayGroupId = "DisplayGroupId";
DataTableSyntax.SumTotalColumn = "SumTotalColumn";
class CheckboxOptionsSyntax {
}
CheckboxOptionsSyntax.Single = "Single";
CheckboxOptionsSyntax.ExclusiveToQuestion = "ExclusiveToQuestion";
CheckboxOptionsSyntax.PrePopulated = "PrePopulated";
CheckboxOptionsSyntax.Score = "Score";
class RadioOptionsSyntax {
}
RadioOptionsSyntax.PrePopulated = "PrePopulated";
RadioOptionsSyntax.PotScore = "PotScore";
RadioOptionsSyntax.Score = "Score";
class WeightedScoringSyntax {
}
WeightedScoringSyntax.MaxGroupScore = "MaxGroupScore";
WeightedScoringSyntax.QuestionArrayScore = "QuestionArrayScore";
WeightedScoringSyntax.GroupArrayScore = "GroupArrayScore";
class TextActivityOptionsSyntax {
}
TextActivityOptionsSyntax.Paragraph = "Paragraph";
TextActivityOptionsSyntax.Guidance = "Guidance";
TextActivityOptionsSyntax.Hyperlink = "Hyperlink";
TextActivityOptionsSyntax.Url = "Url";
TextActivityOptionsSyntax.Condition = "Condition";
TextActivityOptionsSyntax.Bold = "Bold";
TextActivityOptionsSyntax.Bulletpoints = "Bulletpoints";
TextActivityOptionsSyntax.Collapsed = "Collapsed";
TextActivityOptionsSyntax.Title = "Title";
class ValidationSyntax {
}
ValidationSyntax.UseValidation = "UseValidation";
ValidationSyntax.ValidationRule = "ValidationRule";
class QuestionTypeConstants {
}
QuestionTypeConstants.CurrencyQuestion = "CurrencyQuestion";
QuestionTypeConstants.PercentageQuestion = "PercentageQuestion";
QuestionTypeConstants.DecimalQuestion = "DecimalQuestion";
QuestionTypeConstants.IntegerQuestion = "IntegerQuestion";
QuestionTypeConstants.CheckboxQuestion = "CheckboxQuestion";
QuestionTypeConstants.RadioQuestion = "RadioQuestion";
QuestionTypeConstants.DateQuestion = "DateQuestion";
QuestionTypeConstants.TextQuestion = "TextQuestion";
QuestionTypeConstants.TextAreaQuestion = "TextAreaQuestion";
QuestionTypeConstants.DataTable = "DataTable";
QuestionTypeConstants.PotScoreRadioQuestion = "PotScoreRadioQuestion";
QuestionTypeConstants.WeightedRadioQuestion = "WeightedRadioQuestion";
QuestionTypeConstants.WeightedCheckboxQuestion = "WeightedCheckboxQuestion";
class ActivityTypeConstants {
}
ActivityTypeConstants.QuestionScreen = "QuestionScreen";
ActivityTypeConstants.SinglePipelineDataSource = "SinglePipelineDataSource";
ActivityTypeConstants.CheckYourAnswersScreen = "CheckYourAnswersScreen";
ActivityTypeConstants.ConfirmationScreen = "ConfirmationScreen";
ActivityTypeConstants.Information = "Information";
class PropertyOutputTypes {
}
PropertyOutputTypes.String = "string";
PropertyOutputTypes.Boolean = "bool";
PropertyOutputTypes.Number = "int";
PropertyOutputTypes.Checkbox = "checkbox";
PropertyOutputTypes.CheckboxGroup = "checkboxGroup";
PropertyOutputTypes.Radio = "radio";
PropertyOutputTypes.TableInput = "tableInput";
PropertyOutputTypes.RadioGroup = "radioGroup";
PropertyOutputTypes.Information = "information";
PropertyOutputTypes.InformationGroup = "informationGroup";
PropertyOutputTypes.Validation = "validation";

export { ActivityTypeConstants as A, CheckboxOptionsSyntax as C, DataTableSyntax as D, PropertyOutputTypes as P, QuestionTypeConstants as Q, RadioOptionsSyntax as R, SyntaxNames as S, TextActivityOptionsSyntax as T, Uri as U, ValidationSyntax as V, WeightedScoringSyntax as W, StoreStatus as a, QuestionCategories as b };
