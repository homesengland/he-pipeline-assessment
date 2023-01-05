var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, h, Prop, State, Event } from '@stencil/core';
import { 
//IntellisenseContext,
SyntaxNames } from '../../models/elsa-interfaces';
import { QuestionOptions } from '../../models/custom-component-models';
import { IconProvider, } from '../icon-provider/icon-provider';
import PlusIcon from '../../icons/plus_icon';
import TrashCanIcon from '../../icons/trash-can';
import { RadioEventHandler } from '../../events/component-events';
let ElsaRadioComponent = class ElsaRadioComponent {
    constructor() {
        this.iconProvider = new IconProvider();
        this.supportedSyntaxes = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
        this.syntaxMultiChoiceCount = 0;
    }
    async componentWillLoad() {
        if (this.question && !this.question.radio) {
            this.question.radio = new QuestionOptions();
        }
        this.handler = new RadioEventHandler(this.question, this.updateQuestion);
    }
    renderQuestionField(fieldId, fieldName, fieldValue, onChangedFunction) {
        return h("div", null,
            h("div", { class: "elsa-mb-1" },
                h("div", { class: "elsa-flex" },
                    h("div", { class: "elsa-flex-1" },
                        h("label", { htmlFor: fieldId, class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700" }, fieldName)))),
            h("input", { type: "text", id: fieldId, name: fieldId, value: fieldValue, onChange: e => {
                    onChangedFunction.bind(this)(e);
                }, class: "disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" }));
    }
    renderCheckboxField(fieldId, fieldName, isChecked, onChangedFunction) {
        return h("div", null,
            h("div", { class: "elsa-mb-1 elsa-mt-2" },
                h("div", { class: "elsa-flex" },
                    h("div", null,
                        h("label", { htmlFor: fieldId, class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700 elsa-p-1" }, fieldName)),
                    h("div", null,
                        h("input", { id: fieldId, name: fieldId, type: "checkbox", checked: isChecked, value: 'true', onChange: e => onChangedFunction.bind(this)(e, this.question), class: "focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" })))));
    }
    render() {
        const renderChoiceEditor = (multiChoice, index) => {
            return (h("tr", { key: `choice-${index}` },
                h("td", { class: "elsa-py-2 elsa-pr-5" },
                    h("input", { type: "text", value: multiChoice.identifier, disabled: true, onChange: e => this.handler.onChoiceIdentifierChanged.bind(this)(e, multiChoice), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" })),
                h("td", { class: "elsa-py-2 elsa-pr-5" },
                    h("input", { type: "text", value: multiChoice.answer, onChange: e => this.handler.onChoiceNameChanged.bind(this)(e, multiChoice), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" })),
                h("td", { class: "elsa-pt-1 elsa-pr-2 elsa-text-right" },
                    h("button", { type: "button", onClick: () => this.handler.onDeleteChoiceClick.bind(this)(multiChoice), class: "elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none" },
                        h(TrashCanIcon, { options: this.iconProvider.getOptions() })))));
        };
        const field = `question-${this.question.id}`;
        return (h("div", null,
            this.renderQuestionField(`${field}-questionid`, `Identifier`, this.question.id, this.handler.onIdentifierChanged),
            this.renderQuestionField(`${field}-title`, `Question Name`, this.question.title, this.handler.onTitleChanged),
            this.renderQuestionField(`${field}-questionText`, `Question`, this.question.questionText, this.handler.onQuestionChanged),
            this.renderQuestionField(`${field}-questionHint`, `Hint`, this.question.questionHint, this.handler.onHintChanged),
            this.renderQuestionField(`${field}-questionGuidance`, `Guidance`, this.question.questionGuidance, this.handler.onGuidanceChanged),
            this.renderCheckboxField(`${field}-displayCommentBox`, `Display Comments`, this.question.displayComments, this.handler.onDisplayCommentsBox),
            h("div", null,
                h("table", { class: "elsa-min-w-full elsa-divide-y elsa-divide-gray-200" },
                    h("thead", { class: "elsa-bg-gray-50" },
                        h("tr", null,
                            h("th", { class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-3/12" }, "Identifier"),
                            h("th", { class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-9/12" }, "Answer"),
                            h("th", { class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-1/12" }, "\u00A0"))),
                    h("tbody", null, this.question.radio.choices.map(renderChoiceEditor))),
                h("button", { type: "button", onClick: () => this.handler.onAddChoiceClick.bind(this)(), class: "elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2" },
                    h(PlusIcon, { options: this.iconProvider.getOptions() }),
                    "Add Choice"))));
    }
};
__decorate([
    Prop()
], ElsaRadioComponent.prototype, "question", void 0);
__decorate([
    State()
], ElsaRadioComponent.prototype, "iconProvider", void 0);
__decorate([
    Event({
        eventName: 'updateQuestion',
        composed: true,
        cancelable: true,
        bubbles: true,
    })
], ElsaRadioComponent.prototype, "updateQuestion", void 0);
ElsaRadioComponent = __decorate([
    Component({
        tag: 'elsa-radio-question',
        shadow: false,
    })
], ElsaRadioComponent);
export { ElsaRadioComponent };
