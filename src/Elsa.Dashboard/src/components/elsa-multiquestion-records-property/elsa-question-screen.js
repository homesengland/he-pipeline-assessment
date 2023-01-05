var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, h, Prop, State, Listen } from '@stencil/core';
import TrashCanIcon from '../../icons/trash-can';
import { 
//IntellisenseContext,
SyntaxNames } from '../../models/elsa-interfaces';
import { QuestionActivity, } from '../../models/custom-component-models';
import { IconProvider } from '../icon-provider/icon-provider';
import { QuestionLibrary, QuestionProvider } from '../question-provider/question-provider';
function parseJson(json) {
    if (!json)
        return null;
    try {
        return JSON.parse(json);
    }
    catch (e) {
        console.warn(`Error parsing JSON: ${e}`);
    }
    return undefined;
}
let ElsaQuestionScreen = class ElsaQuestionScreen {
    constructor() {
        this.questionModel = new QuestionActivity();
        this.iconProvider = new IconProvider();
        this.questionProvider = new QuestionProvider(Object.values(QuestionLibrary));
        this.supportedSyntaxes = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
        this.syntaxMultiChoiceCount = 0;
        this.renderChoiceEditor = (question, index) => {
            const field = `question-${index}`;
            return (h("div", { id: `${field}-id`, class: "accordion elsa-mb-4 elsa-rounded", onClick: this.onAccordionQuestionClick },
                h("button", { type: "button elsa-mt-1 elsa-text-m elsa-text-gray-900" },
                    question.title,
                    " "),
                h("button", { type: "button", onClick: e => this.onDeleteQuestionClick(e, question), class: "elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none trashcan-icon", style: { float: "right" } },
                    h(TrashCanIcon, { options: this.iconProvider.getOptions() })),
                h("p", { class: "elsa-mt-1 elsa-text-sm elsa-text-gray-900" }, question.questionTypeName),
                this.renderQuestionComponent(question)));
        };
    }
    getQuestion(event) {
        if (event.detail) {
            this.updateQuestion(event.detail);
        }
    }
    async componentWillLoad() {
        const propertyModel = this.propertyModel;
        const choicesJson = propertyModel.expressions[SyntaxNames.Json];
        this.questionModel = parseJson(choicesJson) || this.defaultActivityModel();
    }
    defaultActivityModel() {
        var activity = new QuestionActivity();
        activity.questions = [];
        return activity;
    }
    updatePropertyModel() {
        this.enforceQuestionIdentifierUniqueness();
        this.propertyModel.expressions[SyntaxNames.Json] = JSON.stringify(this.questionModel);
    }
    updateQuestion(updatedQuestion) {
        let questionToUpdate = this.questionModel.questions.findIndex((obj) => obj.id === updatedQuestion.id);
        let newModel = this.questionModel;
        newModel.questions[questionToUpdate] = updatedQuestion;
        this.questionModel.questions.map(x => x.id != updatedQuestion.id);
        this.questionModel = Object.assign(Object.assign({}, this.questionModel), { questions: newModel.questions });
        this.updatePropertyModel();
    }
    enforceQuestionIdentifierUniqueness() {
        for (let i = 0; i < this.questionModel.questions.length; i++) {
            console.log('enforcing uniqueness');
            this.questionModel.questions[i].id = (i + 1).toString();
        }
    }
    hasChoices(questionType) {
        if (questionType == QuestionLibrary.Checkbox.nameConstant
            || questionType == QuestionLibrary.Radio.nameConstant)
            return true;
        else
            return false;
    }
    handleAddQuestion(e) {
        let value = e.currentTarget.value.trim();
        let name = e.currentTarget.selectedOptions[0].dataset.typename;
        if (value != null && value != "") {
            this.onAddQuestion(value, name);
            let element = e.currentTarget;
            element.selectedIndex = 0;
        }
    }
    onAddQuestion(questionType, questionTypeName) {
        let id = (this.questionModel.questions.length + 1).toString();
        const questionName = `Question ${id}`;
        const newQuestion = { id: id, title: questionName, questionGuidance: "", questionText: "", displayComments: false, questionHint: "", questionType: questionType, questionTypeName: questionTypeName };
        this.questionModel = Object.assign(Object.assign({}, this.questionModel), { questions: [...this.questionModel.questions, newQuestion] });
        this.updatePropertyModel();
    }
    onDeleteQuestionClick(e, question) {
        e.stopPropagation();
        this.questionModel = Object.assign(Object.assign({}, this.questionModel), { questions: this.questionModel.questions.filter(x => x != question) });
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
    renderQuestions(model) {
        return model.questions.map(this.renderChoiceEditor);
    }
    renderQuestionComponent(question) {
        switch (question.questionType) {
            case "CheckboxQuestion":
                return h("elsa-checkbox-question", { onClick: (e) => e.stopPropagation(), class: "panel elsa-rounded", question: question });
            case "RadioQuestion":
                return h("elsa-radio-question", { onClick: (e) => e.stopPropagation(), class: "panel elsa-rounded", question: question });
            default:
                return h("elsa-question", { onClick: (e) => e.stopPropagation(), class: "panel elsa-rounded", question: question });
        }
    }
    renderQuestionField(fieldId, fieldName, fieldValue, multiQuestion, onChangedFunction) {
        return h("div", null,
            h("div", { class: "elsa-mb-1 elsa-mt-2" },
                h("div", { class: "elsa-flex" },
                    h("div", { class: "elsa-flex-1" },
                        h("label", { htmlFor: fieldId, class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700" }, fieldName)))),
            h("input", { type: "text", id: fieldId, name: fieldId, value: fieldValue, onChange: e => onChangedFunction(e, multiQuestion), class: "disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" }));
    }
    renderCheckboxField(fieldId, fieldName, isChecked, multiQuestion, onChangedFunction) {
        return h("div", null,
            h("div", { class: "elsa-mb-1 elsa-mt-2" },
                h("div", { class: "elsa-flex" },
                    h("div", null,
                        h("input", { id: fieldId, name: fieldId, type: "checkbox", checked: isChecked, value: 'true', onChange: e => onChangedFunction(e, multiQuestion), class: "focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" })),
                    h("div", null,
                        h("label", { htmlFor: fieldId, class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700 elsa-p-1" }, fieldName)))));
    }
    render() {
        return (h("div", null,
            h("div", { class: "elsa-mb-1" },
                h("div", { class: "elsa-flex" },
                    h("div", { class: "elsa-flex-1" },
                        h("label", { htmlFor: "Questions", class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700" }, "List of questions")))),
            this.renderQuestions(this.questionModel),
            h("select", { id: "addQuestionDropdown", onChange: (e) => this.handleAddQuestion.bind(this)(e), name: "addQuestionDropdown", class: "elsa-mt-1 elsa-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md" },
                h("option", { value: "" }, "Add a Question..."),
                this.questionProvider.displayOptions())));
    }
};
__decorate([
    Prop()
], ElsaQuestionScreen.prototype, "activityModel", void 0);
__decorate([
    Prop()
], ElsaQuestionScreen.prototype, "propertyDescriptor", void 0);
__decorate([
    Prop()
], ElsaQuestionScreen.prototype, "propertyModel", void 0);
__decorate([
    State()
], ElsaQuestionScreen.prototype, "questionModel", void 0);
__decorate([
    State()
], ElsaQuestionScreen.prototype, "iconProvider", void 0);
__decorate([
    State()
], ElsaQuestionScreen.prototype, "questionProvider", void 0);
__decorate([
    Listen('updateQuestion', { target: "body" })
], ElsaQuestionScreen.prototype, "getQuestion", null);
ElsaQuestionScreen = __decorate([
    Component({
        tag: 'elsa-question-screen',
        shadow: false,
    })
], ElsaQuestionScreen);
export { ElsaQuestionScreen };
