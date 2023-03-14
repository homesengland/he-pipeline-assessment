import { h } from '@stencil/core';
import { QuestionTypeConstants, ActivityTypeConstants } from '../../../constants/constants';

export interface IActivityData {
  nameConstant: string,
  displayName: string,
  description: string
}

export class QuestionData implements IActivityData {
  constructor(constant: string, displayname: string, description: string) {
    this.nameConstant = constant;
    this.displayName = displayname;
    this.description = description;
  }
    nameConstant: string;
    displayName: string;
    description: string;
}

export const QuestionLibrary = {
  Checkbox: new QuestionData(QuestionTypeConstants.CheckboxQuestion, "Checkbox Question", "A question that provides a user with a number of options presented as checkboxes.  A user may multiple values, but you can restrict this by using the 'isSingle' property."),
  Currency: new QuestionData(QuestionTypeConstants.CurrencyQuestion, "Currency Question", "A question that provides a user with a text box to enter a numeric value"),
  Date: new QuestionData(QuestionTypeConstants.DateQuestion, "Date Question", "A question that provides a user with fields to enter a single date."),
  Radio: new QuestionData(QuestionTypeConstants.RadioQuestion, "Radio Question", "A question that provides a user with a number of options presented as radio buttons.  A user may only select a single value."),
  Text: new QuestionData(QuestionTypeConstants.TextQuestion, "Text Question", "A question that provides a user with a free form, one-line text box."),
  TextArea: new QuestionData(QuestionTypeConstants.TextAreaQuestion, "Text Area Question", "A question that provides a user with a free form, multi-line text box."),
  Information: new QuestionData(ActivityTypeConstants.Information, "Information Text", "A text to display additional information about the given screen.")
};

export class QuestionProvider {

  private questionList: Array<QuestionData> = new Array<QuestionData>();

  constructor(questions: Array<QuestionData>) {
    this.questionList.push(...questions);
  }

  displayOptions() {
    return this.questionList.map(this.renderOption);
  }

  private renderOption(data: QuestionData) {
    return (<option value={data.nameConstant} data-type={JSON.stringify(data)}>{data.displayName}</option>);
  }
}
