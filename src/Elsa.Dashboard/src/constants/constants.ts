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
  static Variable: string;
  static Output: string;
}

export class QuestionCategories {
  static readonly None = "None";
  static readonly Question = "Question";
  static readonly Scoring = "Scoring";
}

export class CheckboxOptionsSyntax {
  static readonly Single = "Single";
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
}

export class QuestionTypeConstants {
  static readonly CurrencyQuestion = "CurrencyQuestion";
  static readonly CheckboxQuestion = "CheckboxQuestion";
  static readonly RadioQuestion = "RadioQuestion";
  static readonly DateQuestion = "DateQuestion";
  static readonly TextQuestion = "TextQuestion";
  static readonly TextAreaQuestion = "TextAreaQuestion";

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
  static readonly RadioGroup = "radioGroup";
  static readonly Information = "information";
}
