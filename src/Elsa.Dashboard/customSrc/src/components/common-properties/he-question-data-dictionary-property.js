var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, h, Event, Prop, State } from '@stencil/core';
import state from '../../stores/store';
import { SyntaxNames } from '../../constants/constants';
//import { mapSyntaxToLanguage, parseJson, Map } from "../../utils/utils";
let HEQuestionDataDictionaryProperty = 
//Copy of Elsa Switch Case
//Copied to allow us control over how the expression editor is displayed.
class HEQuestionDataDictionaryProperty {
    constructor() {
        this.dataDictionary = [];
        this.dataDictionaryGroup = state.dictionaryGroups.filter(x => !x.isArchived);
        this.dataDictionaryDisplayToggle = false;
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
        return (h("div", { style: { padding: "10px 0px 10px 0px" } },
            h("label", { class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700" }, "Data Dictionary"),
            h("div", { class: "elsa-justify-content" },
                h("span", { style: { display: displayGroupStyle, padding: "0px 10px 0px 0px" } },
                    h("select", { onChange: e => this.onGroupDictionaryOptionChanged(e), class: "elsa-mt-1 elsa-inline-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md" },
                        h("option", { value: "0" }, "Choose Group"),
                        this.dataDictionaryGroup.map(data => {
                            const selected = data == selectedGroup;
                            return h("option", { selected: selected, value: data.Id }, data.Name);
                        }))),
                this.dataDictionaryDisplayToggle ?
                    h("span", { style: { display: displayDataDictionaryItemsStyle, padding: "0px 5px 0px 0px" } },
                        h("select", { id: "test_1", onChange: e => this.onDataDictionaryOptionChanged(e), class: "elsa-mt-1 elsa-inline-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md" },
                            h("option", { selected: clearSelection, value: "0" }, "Choose Field"),
                            this.dataDictionary.map(data => {
                                const selected = data.Id == selectedDataDictionaryItem;
                                return h("option", { selected: selected, value: data.Id }, data.Name);
                            }))) : ""),
            h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, "Assigns a data dictionary field to the question for reporting purposes")));
    }
};
__decorate([
    Prop()
], HEQuestionDataDictionaryProperty.prototype, "activityModel", void 0);
__decorate([
    Prop()
], HEQuestionDataDictionaryProperty.prototype, "propertyDescriptor", void 0);
__decorate([
    Prop()
], HEQuestionDataDictionaryProperty.prototype, "propertyModel", void 0);
__decorate([
    Event()
], HEQuestionDataDictionaryProperty.prototype, "expressionChanged", void 0);
__decorate([
    State()
], HEQuestionDataDictionaryProperty.prototype, "dataDictionary", void 0);
__decorate([
    State()
], HEQuestionDataDictionaryProperty.prototype, "selectedDataDictionaryItem", void 0);
HEQuestionDataDictionaryProperty = __decorate([
    Component({
        tag: 'he-question-data-dictionary-property',
        shadow: false,
    })
    //Copy of Elsa Switch Case
    //Copied to allow us control over how the expression editor is displayed.
], HEQuestionDataDictionaryProperty);
export { HEQuestionDataDictionaryProperty };
