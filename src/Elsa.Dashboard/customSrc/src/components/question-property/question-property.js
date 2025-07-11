var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, Event, Prop, State, h } from '@stencil/core';
import state from '../../stores/store';
import { IconProvider, } from '../providers/icon-provider/icon-provider';
import { HePropertyDisplayManager } from '../../nested-drivers/display-managers/display-manager';
import { parseJson, updateNestedActivitiesByDescriptors, filterPropertiesByType, createQuestionProperty } from '../../utils/utils';
import { SyntaxNames } from '../../constants/constants';
let QuestionProperty = class QuestionProperty {
    constructor() {
        this.iconProvider = new IconProvider();
        this.displayManager = new HePropertyDisplayManager();
        this.syntaxMultiChoiceCount = 0;
        this.dataDictionaryGroup = [];
    }
    async componentWillLoad() {
        this.getOrCreateQuestionProperties();
        this.dataDictionaryGroup = state.dictionaryGroups;
    }
    async componentWillRender() {
        this.getOrCreateQuestionProperties();
    }
    getOrCreateQuestionProperties() {
        const model = this.questionModel;
        this.questionModel.descriptor = filterPropertiesByType(this.questionModel.descriptor, this.questionModel.ActivityType.nameConstant);
        const propertyJson = model.value.expressions[SyntaxNames.QuestionList];
        if (propertyJson != null && propertyJson != undefined && parseJson(propertyJson).length > 0) {
            const nestedProperties = parseJson(propertyJson);
            this.nestedQuestionProperties = updateNestedActivitiesByDescriptors(this.questionModel.descriptor, nestedProperties, this.questionModel);
        }
        else {
            this.nestedQuestionProperties = this.createQuestionProperties();
        }
    }
    createQuestionProperties() {
        let propertyArray = [];
        const descriptor = this.questionModel.descriptor;
        descriptor.forEach(d => {
            var prop = createQuestionProperty(d, this.questionModel);
            propertyArray.push(prop);
        });
        return propertyArray;
    }
    onPropertyExpressionChange(event, property) {
        event = event;
        property = property;
        this.updateQuestionModel();
    }
    updateQuestionModel() {
        let nestedQuestionsJson = JSON.stringify(this.nestedQuestionProperties);
        this.questionModel.value.expressions[SyntaxNames.QuestionList] = nestedQuestionsJson;
        this.updateQuestionScreen.emit(JSON.stringify(this.questionModel));
    }
    render() {
        const displayManager = this.displayManager;
        const renderPropertyEditor = (property) => {
            var content = displayManager.displayNested(this.activityModel, property, this.onPropertyExpressionChange.bind(this));
            let id = property.descriptor.name + "Category";
            return (h("he-elsa-control", { id: id, content: content, class: "sm:elsa-col-span-6 hydrated" }));
        };
        return (h("div", { class: "elsa-grid elsa-grid-cols-1 elsa-gap-y-6 elsa-gap-x-4 sm:elsa-grid-cols-6" }, this.nestedQuestionProperties.map(renderPropertyEditor)));
    }
};
__decorate([
    Prop()
], QuestionProperty.prototype, "activityModel", void 0);
__decorate([
    Prop()
], QuestionProperty.prototype, "questionModel", void 0);
__decorate([
    State()
], QuestionProperty.prototype, "iconProvider", void 0);
__decorate([
    State()
], QuestionProperty.prototype, "currentValue", void 0);
__decorate([
    State()
], QuestionProperty.prototype, "nestedQuestionProperties", void 0);
__decorate([
    State()
], QuestionProperty.prototype, "displayManager", void 0);
__decorate([
    Event()
], QuestionProperty.prototype, "updateQuestionScreen", void 0);
QuestionProperty = __decorate([
    Component({
        tag: 'question-property',
        shadow: false,
    })
], QuestionProperty);
export { QuestionProperty };
