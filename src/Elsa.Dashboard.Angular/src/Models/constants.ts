export class Uri {
  static readonly LibUri = "defaultLib:lib.es6.d.ts";
}

export class StoreStatus {
  static readonly Fetching: string = "fetching";
  static readonly Available: string = "available";
  static readonly Empty: string = "empty";
}

export class SyntaxNames {
  static readonly Literal = "Literal";
  static readonly JavaScript = "JavaScript";
  static readonly Liquid = "Liquid";
  static readonly Json = "Json";
  static readonly SQL = "SQL";
  static readonly Switch = "Switch";
  static readonly Question = "Question";
  static readonly QuestionList = "QuestionList";
  static readonly Checked = "Checked";
  static readonly ConditionalTextList = 'ConditionalTextList';
  static readonly TextActivity = 'TextActivity';
  static readonly TextActivityList = 'TextActivityList';
  static readonly DataDictionary = 'DataDictionary';
  static readonly DataTable = 'DataTable';
  static readonly InformationText = 'TextActivity';
  static readonly GroupedInformationText = 'TextGroupActivity';
  static Variable: string;
  static Output: string;
}

export class QuestionCategories {
  static readonly None = "None";
  static readonly Question = "Question";
  static readonly Scoring = "Scoring";
}

export class DataTableSyntax {
  static readonly Readonly = "ReadOnly";
  static readonly Identifier = "Identifier";
  static readonly InputType = "InputType";
  static readonly Input = "Input"
  static readonly TableHeader = "TableHeader";
  static readonly InputHeader = "InputHeader"
  static readonly Title = "Title";
  static readonly DisplayGroupId = "DisplayGroupId";
  static readonly SumTotalColumn = "SumTotalColumn";
}

export class CheckboxOptionsSyntax {
  static readonly Single = "Single";
  static readonly ExclusiveToQuestion = "ExclusiveToQuestion";
  static readonly PrePopulated = "PrePopulated";
  static readonly Score = "Score";
}

export class RadioOptionsSyntax {
  static readonly PrePopulated = "PrePopulated";
  static readonly PotScore = "PotScore";
  static readonly Score = "Score";
}

export class WeightedScoringSyntax {
  static readonly MaxGroupScore = "MaxGroupScore";
  static readonly QuestionArrayScore = "QuestionArrayScore";
  static readonly GroupArrayScore = "GroupArrayScore";
}

export class TextActivityOptionsSyntax {
  static readonly Paragraph = "Paragraph";
  static readonly Guidance = "Guidance";
  static readonly Hyperlink = "Hyperlink";
  static readonly Url = "Url";
  static readonly Condition = "Condition";
  static readonly Bold = "Bold";
  static readonly Bulletpoints = "Bulletpoints";
  static readonly Collapsed = "Collapsed";
  static readonly Title = "Title";
}

export class ValidationSyntax {
  static readonly UseValidation = "UseValidation";
  static readonly ValidationRule = "ValidationRule";
}

export class QuestionTypeConstants {
  static readonly CurrencyQuestion = "CurrencyQuestion";
  static readonly PercentageQuestion = "PercentageQuestion";
  static readonly DecimalQuestion = "DecimalQuestion";
  static readonly IntegerQuestion = "IntegerQuestion";
  static readonly CheckboxQuestion = "CheckboxQuestion";
  static readonly RadioQuestion = "RadioQuestion";
  static readonly DateQuestion = "DateQuestion";
  static readonly TextQuestion = "TextQuestion";
  static readonly TextAreaQuestion = "TextAreaQuestion";
  static readonly DataTable = "DataTable";

  static readonly PotScoreRadioQuestion = "PotScoreRadioQuestion";
  static readonly WeightedRadioQuestion = "WeightedRadioQuestion";
  static readonly WeightedCheckboxQuestion = "WeightedCheckboxQuestion";

}

export class ActivityTypeConstants {
  static readonly QuestionScreen = "QuestionScreen";
  static readonly SinglePipelineDataSource = "SinglePipelineDataSource";
  static readonly CheckYourAnswersScreen = "CheckYourAnswersScreen";
  static readonly ConfirmationScreen = "ConfirmationScreen";
  static readonly Information = "Information";
}

export class PropertyOutputTypes {
  static readonly String = "string";
  static readonly Boolean = "bool";
  static readonly Number = "int";
  static readonly Checkbox = "checkbox";
  static readonly CheckboxGroup = "checkboxGroup";
  static readonly Radio = "radio";
  static readonly TableInput = "tableInput";
  static readonly RadioGroup = "radioGroup";
  static readonly Information = "information";
  static readonly InformationGroup = "informationGroup";
  static readonly Validation = "validation";
}
