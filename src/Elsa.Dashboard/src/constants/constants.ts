export class SyntaxNames {
  static readonly Literal = "Literal";
  static readonly JavaScript = "JavaScript";
  static readonly Liquid = "Liquid";
  static readonly Json = "Json";
  static readonly SQL = "SQL";
  static readonly Switch = "Switch";
  static readonly Question = "Question";
  static Variable: string;
  static Output: string;
}

export class QuestionTypeConstants {
  static readonly CurrencyQuestion = "CurrencyQuestion";
  static readonly CheckboxQuestion = "CheckboxQuestion";
  static readonly RadioQuestion = "RadioQuestion";
  static readonly DateQuestion = "DateQuestion";
  static readonly TextQuestion = "TextQuestion";
  static readonly TextAreaQuestion = "TextAreaQuestion";

}

export class ActivityTypeConstants {
  static readonly QuestionScreen = "QuestionScreen";
  static readonly SinglePipelineDataSource = "SinglePipelineDataSource";
  static readonly CheckYourAnswersScreen = "CheckYourAnswersScreen";
  static readonly ConfirmationScreen = "ConfirmationScreen";
}
