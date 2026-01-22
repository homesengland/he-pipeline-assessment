import { r as registerInstance, e as createEvent, h } from './index-1542df5c.js';
import { s as state } from './store-40346019.js';
import { S as SyntaxNames } from './constants-6ea82f24.js';
import './index-0d4e8807.js';

const HEQuestionDataDictionaryProperty = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.expressionChanged = createEvent(this, "expressionChanged", 7);
    this.dataDictionaryGroup = state.dictionaryGroups.filter(x => !x.isArchived);
    this.dataDictionaryDisplayToggle = false;
    this.activityModel = undefined;
    this.propertyDescriptor = undefined;
    this.propertyModel = undefined;
    this.dataDictionary = [];
    this.selectedDataDictionaryItem = undefined;
  }
  async componentWillLoad() {
    this.dataDictionary = [];
    this.selectedDataDictionaryItem = parseInt(this.propertyModel.expressions[SyntaxNames.Literal]);
    this.selectedGroup = this.dataDictionaryGroup.filter(x => x.DataDictionaryList.filter(y => y.Id == this.selectedDataDictionaryItem)[0])[0];
    if (this.selectedGroup != undefined) {
      this.dataDictionaryDisplayToggle = true;
      this.dataDictionary = [...this.selectedGroup.DataDictionaryList.filter(x => !x.IsArchived)];
    }
  }
  updatePropertyModel(updatedValue) {
    this.propertyModel.expressions[SyntaxNames.Literal] = updatedValue;
    this.expressionChanged.emit(JSON.stringify(this.propertyModel));
  }
  onGroupDictionaryOptionChanged(e) {
    const select = e.currentTarget;
    this.selectedGroupDictionaryId = parseInt(select.value);
    let selectedGroup = this.dataDictionaryGroup.filter(x => x.Id === this.selectedGroupDictionaryId)[0];
    if (selectedGroup != undefined) {
      this.selectedDataDictionaryItem = 0;
      this.selectedGroup = selectedGroup;
      this.dataDictionaryDisplayToggle = true;
      this.dataDictionary = [...selectedGroup.DataDictionaryList];
    }
    else {
      this.dataDictionaryDisplayToggle = false;
      this.dataDictionary = [];
    }
    this.updatePropertyModel('');
  }
  onDataDictionaryOptionChanged(e) {
    const select = e.currentTarget;
    this.selectedDataDictionaryItem = parseInt(select.value);
    this.updatePropertyModel(select.value);
  }
  render() {
    const displayGroupStyle = "inline-block";
    const displayDataDictionaryItemsStyle = "inline-block";
    const selectedDataDictionaryItem = this.selectedDataDictionaryItem;
    const clearSelection = selectedDataDictionaryItem == 0;
    let selectedGroup = this.selectedGroup;
    if (selectedGroup == undefined) {
      selectedGroup = this.dataDictionaryGroup.filter(x => x.DataDictionaryList.filter(y => y.Id == selectedDataDictionaryItem)[0])[0];
    }
    return (h("div", { style: { padding: "10px 0px 10px 0px" } }, h("label", { class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700" }, "Data Dictionary"), h("div", { class: "elsa-justify-content" }, h("span", { style: { display: displayGroupStyle, padding: "0px 10px 0px 0px" } }, h("select", { onChange: e => this.onGroupDictionaryOptionChanged(e), class: "elsa-mt-1 elsa-inline-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md" }, h("option", { value: "0" }, "Choose Group"), this.dataDictionaryGroup.map(data => {
      const selected = data == selectedGroup;
      return h("option", { selected: selected, value: data.Id }, data.Name);
    }))), this.dataDictionaryDisplayToggle ?
      h("span", { style: { display: displayDataDictionaryItemsStyle, padding: "0px 5px 0px 0px" } }, h("select", { id: "test_1", onChange: e => this.onDataDictionaryOptionChanged(e), class: "elsa-mt-1 elsa-inline-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md" }, h("option", { selected: clearSelection, value: "0" }, "Choose Field"), this.dataDictionary.map(data => {
        const selected = data.Id == selectedDataDictionaryItem;
        return h("option", { selected: selected, value: data.Id }, data.Name);
      }))) : ""), h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, "Assigns a data dictionary field to the question for reporting purposes")));
  }
};

export { HEQuestionDataDictionaryProperty as he_question_data_dictionary_property };
