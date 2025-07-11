import { h } from '@stencil/core';
import { QuestionTypeConstants, ActivityTypeConstants, QuestionCategories } from '../../../constants/constants';
export class QuestionData {
    constructor(constant, displayname, description, hasScoring = false) {
        this.nameConstant = constant;
        this.displayName = displayname;
        this.description = description;
        this.hasScoring = hasScoring;
    }
}
export const QuestionLibrary = {
    Checkbox: new QuestionData(QuestionTypeConstants.CheckboxQuestion, "Checkbox Question", "A question that provides a user with a number of options presented as checkboxes.  A user may multiple values, but you can restrict this by using the 'isSingle' property."),
    Currency: new QuestionData(QuestionTypeConstants.CurrencyQuestion, "Currency Question", "A question that provides a user with a text box to enter a numeric value for currency"),
    Decimal: new QuestionData(QuestionTypeConstants.DecimalQuestion, "Decimal Question", "A question that provides a user with a text box to enter a numeric value with decimal points"),
    Integer: new QuestionData(QuestionTypeConstants.IntegerQuestion, "Integer Question", "A question that provides a user with a text box to enter a numeric value for whole numbers only"),
    Percentage: new QuestionData(QuestionTypeConstants.PercentageQuestion, "Percentage Question", "A question that provides a user with a text box to enter a numeric value with decimal points that which will be used as percentage"),
    Date: new QuestionData(QuestionTypeConstants.DateQuestion, "Date Question", "A question that provides a user with fields to enter a single date."),
    Radio: new QuestionData(QuestionTypeConstants.RadioQuestion, "Radio Question", "A question that provides a user with a number of options presented as radio buttons.  A user may only select a single value."),
    Text: new QuestionData(QuestionTypeConstants.TextQuestion, "Text Question", "A question that provides a user with a free form, one-line text box."),
    TextArea: new QuestionData(QuestionTypeConstants.TextAreaQuestion, "Text Area Question", "A question that provides a user with a free form, multi-line text box."),
    DataTable: new QuestionData(QuestionTypeConstants.DataTable, "Data Table", "A question that allows a user to display a list of inputs in a table format."),
    Information: new QuestionData(ActivityTypeConstants.Information, "Information Text", "A text to display additional information about the given screen."),
    PotScoreRadio: new QuestionData(QuestionTypeConstants.PotScoreRadioQuestion, "PotScore Radio Question", "A question that provides a user with a number of options presented as radio buttons with a score associated with each option.  A user may only select a single value.", true),
    WeightedCheckbox: new QuestionData(QuestionTypeConstants.WeightedCheckboxQuestion, "Weighted Checkbox Question", "A question that provides a user with a number of options presented as checkbox list with the ability to provide a score associated with each option.", true),
    WeightedRadio: new QuestionData(QuestionTypeConstants.WeightedRadioQuestion, "Weighted Radio Question", "A question that provides a user with a number of options presented as radio buttons with the ability to provide a score associated with each option.  A user may only select a single value.", true),
};
export class QuestionProvider {
    constructor(questions) {
        this.questionList = new Array();
        this.questionList.push(...questions);
    }
    displayOptions() {
        return this.questionList.filter(x => x.hasScoring == false).map(this.renderOption);
    }
    displayScoringOptions() {
        return this.questionList.filter(x => x.hasScoring == true).map(this.renderOption);
    }
    displayDropdownToggleOptions() {
        var optionList = new Array();
        optionList.push(new QuestionData(QuestionCategories.None, "Select a Question Category...", ""));
        optionList.push(new QuestionData(QuestionCategories.Question, "Assessment Questions", "Standard Assessment Questions without scoring or weighting features"));
        optionList.push(new QuestionData(QuestionCategories.Scoring, "Scoring Questions", "Questions that provide means to assign a score or weighting"));
        return optionList.map(this.renderCategories);
    }
    renderCategories(data) {
        return (h("option", { value: data.nameConstant, "data-type": data.nameConstant }, data.displayName));
    }
    renderOption(data) {
        return (h("option", { value: data.nameConstant, "data-type": JSON.stringify(data) }, data.displayName));
    }
}
