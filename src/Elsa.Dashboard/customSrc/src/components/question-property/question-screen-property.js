var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, h, Prop, State } from '@stencil/core';
import Sortable from 'sortablejs';
import { QuestionScreenProperty } from '../../models/custom-component-models';
import { IconProvider } from '../providers/icon-provider/icon-provider';
import { QuestionLibrary, QuestionProvider } from '../providers/question-provider/question-provider';
import TrashCanIcon from '../../icons/trash-can';
import { QuestionCategories, SyntaxNames } from '../../constants/constants';
import { filterPropertiesByType, parseJson, newOptionNumber, getUniversalUniqueId } from '../../utils/utils';
import SortIcon from '../../icons/sort_icon';
let QuestionScreen = class QuestionScreen {
    constructor() {
        this.questionModel = new QuestionScreenProperty();
        this.iconProvider = new IconProvider();
        this.questionProvider = new QuestionProvider(Object.values(QuestionLibrary));
        this.questionDropdownDisplay = QuestionCategories.None;
        this.supportedSyntaxes = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
        this.syntaxMultiChoiceCount = 0;
        this.renderChoiceEditor = (question, index) => {
            const field = `question-${index}`;
            return (h("div", { id: `${field}-id`, class: "accordion elsa-mb-4 elsa-rounded", onClick: this.onAccordionQuestionClick },
                h("div", { class: "elsa-w-1 sortablejs-custom-handle" },
                    h(SortIcon, { options: this.iconProvider.getOptions() })),
                h("div", null,
                    h("p", { class: "elsa-mt-1 elsa-text-base elsa-text-gray-900 accordion-paragraph" }, question.value.name),
                    h("button", { type: "button", onClick: e => this.onDeleteQuestionClick(e, question), class: "elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none trashcan-icon", style: { float: "right" } },
                        h(TrashCanIcon, { options: this.iconProvider.getOptions() })),
                    h("p", { class: "elsa-mt-1 elsa-text-sm elsa-text-gray-900 accordion-paragraph" }, question.ActivityType.displayName),
                    this.renderQuestionComponent(question))));
        };
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
        return (h("div", null,
            h("div", { ref: el => (this.container = el) }, this.renderQuestions(this.questionModel)),
            h("elsa-multi-expression-editor", { ref: el => this.multiExpressionEditor = el, label: this.propertyDescriptor.label, defaultSyntax: SyntaxNames.Json, supportedSyntaxes: [SyntaxNames.Json], context: context, expressions: { 'Json': json }, "editor-height": "20rem", onExpressionChanged: e => this.onMultiExpressionEditorValueChanged(e), onSyntaxChanged: e => this.onMultiExpressionEditorSyntaxChanged(e) },
                h("div", { class: "elsa-justify-content" },
                    h("div", { class: "elsa-pr-5 elsa-mt-1 elsa-w-full sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md" },
                        h("select", { id: "toggleCategoryDropdown", onChange: (e) => this.handleFilterQuestions.bind(this)(e), name: "toggleCategoryDropdown", class: "elsa-inline-block focus:elsa-ring-blue-500 px-6 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md" }, this.questionProvider.displayDropdownToggleOptions())),
                    h("div", { class: "elsa-mt-1 elsa-w-full sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md", style: { display: displayStandardQuestions } },
                        h("select", { id: "addQuestionDropdown", onChange: (e) => this.handleAddQuestion.bind(this)(e), name: "addQuestionDropdown", class: "elsa-inline-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md" },
                            h("option", { value: "" }, "Add a Question..."),
                            this.questionProvider.displayOptions())),
                    h("div", { class: "elsa-mt-1 elsa-w-full sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md", style: { display: displayScoringQuestions } },
                        h("select", { id: "addScoringQuestionDropdown", onChange: (e) => this.handleAddQuestion.bind(this)(e), name: "addScoringQuestionDropdown", class: "elsa-inline-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md" },
                            h("option", { value: "" }, "Add a Scoring Question..."),
                            this.questionProvider.displayScoringOptions()))))));
    }
};
__decorate([
    Prop()
], QuestionScreen.prototype, "activityModel", void 0);
__decorate([
    Prop()
], QuestionScreen.prototype, "propertyDescriptor", void 0);
__decorate([
    Prop()
], QuestionScreen.prototype, "propertyModel", void 0);
__decorate([
    Prop()
], QuestionScreen.prototype, "questionProperties", void 0);
__decorate([
    State()
], QuestionScreen.prototype, "questionModel", void 0);
__decorate([
    State()
], QuestionScreen.prototype, "iconProvider", void 0);
__decorate([
    State()
], QuestionScreen.prototype, "questionProvider", void 0);
__decorate([
    State()
], QuestionScreen.prototype, "questionDropdownDisplay", void 0);
__decorate([
    State()
], QuestionScreen.prototype, "keyId", void 0);
QuestionScreen = __decorate([
    Component({
        tag: 'question-screen-property',
        shadow: false,
    })
], QuestionScreen);
export { QuestionScreen };
