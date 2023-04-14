import { h } from '@stencil/core';
import { QuestionTypeConstants, ActivityTypeConstants, QuestionCategories } from '../../../constants/constants';

export interface IActivityData {
  nameConstant: string,
  displayName: string,
  description: string,
  hasScoring: boolean
}

export class QuestionData implements IActivityData {
  constructor(constant: string, displayname: string, description: string, hasScoring: boolean = false) {
    this.nameConstant = constant;
    this.displayName = displayname;
    this.description = description;
    this.hasScoring = hasScoring;

  }
    nameConstant: string;
    displayName: string;
    description: string;
    hasScoring: boolean;
}

export const QuestionLibrary = {
  Checkbox: new QuestionData(QuestionTypeConstants.CheckboxQuestion, "Checkbox Question", "A question that provides a user with a number of options presented as checkboxes.  A user may multiple values, but you can restrict this by using the 'isSingle' property."),
  Currency: new QuestionData(QuestionTypeConstants.CurrencyQuestion, "Currency Question", "A question that provides a user with a text box to enter a numeric value"),
  Date: new QuestionData(QuestionTypeConstants.DateQuestion, "Date Question", "A question that provides a user with fields to enter a single date."),
  Radio: new QuestionData(QuestionTypeConstants.RadioQuestion, "Radio Question", "A question that provides a user with a number of options presented as radio buttons.  A user may only select a single value."),
  Text: new QuestionData(QuestionTypeConstants.TextQuestion, "Text Question", "A question that provides a user with a free form, one-line text box."),
  TextArea: new QuestionData(QuestionTypeConstants.TextAreaQuestion, "Text Area Question", "A question that provides a user with a free form, multi-line text box."),
  Information: new QuestionData(ActivityTypeConstants.Information, "Information Text", "A text to display additional information about the given screen."),

  PotScoreRadio: new QuestionData(QuestionTypeConstants.PotScoreRadioQuestion, "PotScore Radio Question", "A question that provides a user with a number of options presented as radio buttons with a score associated with each option.  A user may only select a single value.", true),
  WeightedCheckbox: new QuestionData(QuestionTypeConstants.WeightedCheckboxQuestion, "Weighted Checkbox Question", "A question that provides a user with a number of options presented as checkbox list with the ability to provide a score associated with each option.", true),
  WeightedRadio: new QuestionData(QuestionTypeConstants.WeightedRadioQuestion, "Weighted Radio Question", "A question that provides a user with a number of options presented as radio buttons with the ability to provide a score associated with each option.  A user may only select a single value.", true),
};

export class QuestionProvider {

  private questionList: Array<QuestionData> = new Array<QuestionData>();

  constructor(questions: Array<QuestionData>) {
    this.questionList.push(...questions);
  }

  displayOptions() {
    return this.questionList.filter(x => x.hasScoring == false).map(this.renderOption);
  }

  displayScoringOptions() {
    return this.questionList.filter(x => x.hasScoring == true).map(this.renderOption)
  }

  displayDropdownToggleOptions() {
    var optionList: Array<QuestionData> = new Array<QuestionData>();
    optionList.push(new QuestionData(QuestionCategories.None, "Select a Question Category...", ""));
    optionList.push(new QuestionData(QuestionCategories.Question, "Assessment Questions", "Standard Assessment Questions without scoring or weighting features"));
    optionList.push(new QuestionData(QuestionCategories.Scoring, "Scoring Questions", "Questions that provide means to assign a score or weighting"));
    return optionList.map(this.renderCategories)
  }

  private renderCategories(data: QuestionData) {
    return (<option value={data.nameConstant} data-type={data.nameConstant}>{data.displayName}</option>);
  }

  private renderOption(data: QuestionData) {
    return (<option value={data.nameConstant} data-type={JSON.stringify(data)}>{data.displayName}</option>);
  }
}
