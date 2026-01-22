import { h, r as registerInstance } from './index-1542df5c.js';
import { S as SortIcon, a as Sortable } from './sortable.esm-581578f1.js';
import { I as IconProvider } from './icon-provider-7131deff.js';
import { Q as QuestionTypeConstants, A as ActivityTypeConstants, b as QuestionCategories, S as SyntaxNames } from './constants-6ea82f24.js';
import { T as TrashCanIcon } from './trash-can-639efdf2.js';
import { p as parseJson, g as getUniversalUniqueId, a as newOptionNumber, f as filterPropertiesByType } from './utils-89b7e981.js';
import './index-912d1a21.js';

class QuestionScreenProperty {
  constructor() {
    this.activities = [];
  }
}
class NestedProperty {
}
class NestedPropertyModel {
  constructor() {
    this.descriptor = [];
  }
}

class QuestionData {
  constructor(constant, displayname, description, hasScoring = false) {
    this.nameConstant = constant;
    this.displayName = displayname;
    this.description = description;
    this.hasScoring = hasScoring;
  }
}
const QuestionLibrary = {
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
class QuestionProvider {
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

const QuestionScreen = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.supportedSyntaxes = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
    this.syntaxMultiChoiceCount = 0;
    this.renderChoiceEditor = (question, index) => {
      const field = `question-${index}`;
      return (h("div", { id: `${field}-id`, class: "accordion elsa-mb-4 elsa-rounded", onClick: this.onAccordionQuestionClick }, h("div", { class: "elsa-w-1 sortablejs-custom-handle" }, h(SortIcon, { options: this.iconProvider.getOptions() })), h("div", null, h("p", { class: "elsa-mt-1 elsa-text-base elsa-text-gray-900 accordion-paragraph" }, question.value.name), h("button", { type: "button", onClick: e => this.onDeleteQuestionClick(e, question), class: "elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none trashcan-icon", style: { float: "right" } }, h(TrashCanIcon, { options: this.iconProvider.getOptions() })), h("p", { class: "elsa-mt-1 elsa-text-sm elsa-text-gray-900 accordion-paragraph" }, question.ActivityType.displayName), this.renderQuestionComponent(question))));
    };
    this.activityModel = undefined;
    this.propertyDescriptor = undefined;
    this.propertyModel = undefined;
    this.questionProperties = undefined;
    this.questionModel = new QuestionScreenProperty();
    this.iconProvider = new IconProvider();
    this.questionProvider = new QuestionProvider(Object.values(QuestionLibrary));
    this.questionDropdownDisplay = QuestionCategories.None;
    this.keyId = undefined;
  }
  async componentWillLoad() {
    const propertyModel = this.propertyModel;
    const choicesJson = propertyModel.expressions[SyntaxNames.QuestionList];
    this.questionModel = parseJson(choicesJson) || this.defaultActivityModel();
    this.questionModel.activities.forEach(x => x.descriptor = this.questionProperties);
  }
  async componentDidLoad() {
    const dragEventHandler = this.onDragActivity.bind(this);
    //creates draggable area
    Sortable.create(this.container, {
      animation: 150,
      handle: ".sortablejs-custom-handle",
      ghostClass: "dragTarget",
      onEnd(evt) {
        dragEventHandler(evt.oldIndex, evt.newIndex);
      }
    });
  }
  async componentWillRender() {
    this.keyId = getUniversalUniqueId();
  }
  onDragActivity(oldIndex, newIndex) {
    const activity = this.questionModel.activities.splice(oldIndex, 1)[0];
    this.questionModel.activities.splice(newIndex, 0, activity);
    this.updatePropertyModel();
  }
  defaultActivityModel() {
    var activity = new QuestionScreenProperty();
    activity.activities = [];
    return activity;
  }
  updatePropertyModel() {
    this.propertyModel.expressions[SyntaxNames.QuestionList] = JSON.stringify(this.questionModel);
  }
  onDeleteQuestionClick(e, question) {
    e.stopPropagation();
    this.questionModel = Object.assign(Object.assign({}, this.questionModel), { activities: this.questionModel.activities.filter(x => x != question) });
    this.updatePropertyModel();
  }
  onAccordionQuestionClick(e) {
    let element = e.currentTarget;
    element.classList.toggle("active");
    let panel = element.querySelector('.panel');
    if (panel.style.display === "block") {
      panel.style.display = "none";
    }
    else {
      panel.style.display = "block";
    }
  }
  onUpdateQuestion(e) {
    e = e;
    this.updatePropertyModel();
  }
  renderQuestions(model) {
    return model.activities.map(this.renderChoiceEditor);
  }
  handleAddQuestion(e) {
    let value = e.currentTarget.value.trim();
    let data = JSON.parse(e.currentTarget.selectedOptions[0].dataset.type);
    if (value != null && value != "") {
      this.onAddQuestion(data);
      let element = e.currentTarget;
      element.selectedIndex = 0;
    }
  }
  handleFilterQuestions(e) {
    let value = e.currentTarget.value.trim();
    let data = e.currentTarget.selectedOptions[0].dataset.type;
    if (value != null && value != "") {
      this.onToggleDropdownFilter(data);
    }
  }
  onToggleDropdownFilter(toggleValue) {
    this.questionDropdownDisplay = toggleValue;
  }
  displayDropdowns(categoryType) {
    if (categoryType == this.questionDropdownDisplay) {
      return true;
    }
    return false;
  }
  newQuestionValue(name, id) {
    var value = id;
    var defaultSyntax = SyntaxNames.QuestionList;
    var expression = { name: name, value: value, syntax: defaultSyntax, expressions: { defaultSyntax: value }, };
    return expression;
  }
  activityIds() {
    let activityIds = [];
    if (this.questionModel.activities.length > 0) {
      activityIds = this.questionModel.activities.map(function (v) {
        return parseInt(v.value.value);
      });
    }
    return activityIds;
  }
  onAddQuestion(questionType) {
    let id = newOptionNumber(this.activityIds());
    const questionName = `Question ${id}`;
    const newValue = this.newQuestionValue(questionName, id);
    let propertyDescriptors = filterPropertiesByType(this.questionProperties, questionType.nameConstant);
    let newQuestion = { value: newValue, descriptor: propertyDescriptors, ActivityType: questionType };
    this.questionModel = Object.assign(Object.assign({}, this.questionModel), { activities: [...this.questionModel.activities, newQuestion] });
    this.updatePropertyModel();
  }
  onMultiExpressionEditorValueChanged(e) {
    const json = e.detail;
    const parsed = parseJson(json);
    if (!parsed)
      return;
    if (!Array.isArray(parsed))
      return;
    this.propertyModel.expressions[SyntaxNames.Json] = json;
    this.questionModel.activities = parsed;
  }
  onMultiExpressionEditorSyntaxChanged(e) {
    e = e;
  }
  renderQuestionComponent(question) {
    return h("question-property", { key: this.keyId, class: "panel elsa-rounded", activityModel: this.activityModel, questionModel: question, onClick: (e) => e.stopPropagation(), onUpdateQuestionScreen: e => this.onUpdateQuestion(e) });
  }
  render() {
    const context = {
      activityTypeName: this.activityModel.type,
      propertyName: this.propertyDescriptor.name
    };
    const json = JSON.stringify(this.questionModel, null, 2);
    const displayScoringQuestions = this.displayDropdowns(QuestionCategories.Scoring) ? "inline-block" : "none";
    const displayStandardQuestions = this.displayDropdowns(QuestionCategories.Question) ? "inline-block" : "none";
    return (h("div", null, h("div", { ref: el => (this.container = el) }, this.renderQuestions(this.questionModel)), h("elsa-multi-expression-editor", { ref: el => this.multiExpressionEditor = el, label: this.propertyDescriptor.label, defaultSyntax: SyntaxNames.Json, supportedSyntaxes: [SyntaxNames.Json], context: context, expressions: { 'Json': json }, "editor-height": "20rem", onExpressionChanged: e => this.onMultiExpressionEditorValueChanged(e), onSyntaxChanged: e => this.onMultiExpressionEditorSyntaxChanged(e) }, h("div", { class: "elsa-justify-content" }, h("div", { class: "elsa-pr-5 elsa-mt-1 elsa-w-full sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md" }, h("select", { id: "toggleCategoryDropdown", onChange: (e) => this.handleFilterQuestions.bind(this)(e), name: "toggleCategoryDropdown", class: "elsa-inline-block focus:elsa-ring-blue-500 px-6 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md" }, this.questionProvider.displayDropdownToggleOptions())), h("div", { class: "elsa-mt-1 elsa-w-full sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md", style: { display: displayStandardQuestions } }, h("select", { id: "addQuestionDropdown", onChange: (e) => this.handleAddQuestion.bind(this)(e), name: "addQuestionDropdown", class: "elsa-inline-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md" }, h("option", { value: "" }, "Add a Question..."), this.questionProvider.displayOptions())), h("div", { class: "elsa-mt-1 elsa-w-full sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md", style: { display: displayScoringQuestions } }, h("select", { id: "addScoringQuestionDropdown", onChange: (e) => this.handleAddQuestion.bind(this)(e), name: "addScoringQuestionDropdown", class: "elsa-inline-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md" }, h("option", { value: "" }, "Add a Scoring Question..."), this.questionProvider.displayScoringOptions()))))));
  }
};

export { QuestionScreen as question_screen_property };
