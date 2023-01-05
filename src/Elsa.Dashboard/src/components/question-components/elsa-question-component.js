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
import { IconProvider, } from '../icon-provider/icon-provider';
import { QuestionEventHandler } from '../../events/component-events';
let ElsaQuestionComponent = class ElsaQuestionComponent {
    constructor() {
        this.iconProvider = new IconProvider();
        this.supportedSyntaxes = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
        this.syntaxMultiChoiceCount = 0;
    }
    async componentWillLoad() {
        this.handler = new QuestionEventHandler(this.question, this.updateQuestion);
    }
    renderQuestionField(fieldId, fieldName, fieldValue, onChangedFunction, isDisabled = false) {
        return h("div", null,
            h("div", { class: "elsa-mb-1" },
                h("div", { class: "elsa-flex" },
                    h("div", { class: "elsa-flex-1" },
                        h("label", { htmlFor: fieldId, class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700" }, fieldName)))),
            h("input", { type: "text", id: fieldId, name: fieldId, disabled: isDisabled, value: fieldValue, onChange: e => {
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
                        h("input", { id: fieldId, name: fieldId, type: "checkbox", checked: isChecked, value: 'true', onChange: e => onChangedFunction.bind(this)(e), class: "focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" })))));
    }
    render() {
        const field = `question-${this.question.id}`;
        return (h("div", null,
            this.renderQuestionField(`${field}-questionid`, `Identifier`, this.question.id, this.handler.onIdentifierChanged, true),
            this.renderQuestionField(`${field}-title`, `Title`, this.question.title, this.handler.onTitleChanged),
            this.renderQuestionField(`${field}-questionText`, `Question`, this.question.questionText, this.handler.onQuestionChanged),
            this.renderQuestionField(`${field}-questionHint`, `Hint`, this.question.questionHint, this.handler.onHintChanged),
            this.renderQuestionField(`${field}-questionGuidance`, `Guidance`, this.question.questionGuidance, this.handler.onGuidanceChanged),
            this.renderCheckboxField(`${field}-displayCommentBox`, `Display Comments`, this.question.displayComments, this.handler.onDisplayCommentsBox)));
    }
};
__decorate([
    Prop()
], ElsaQuestionComponent.prototype, "question", void 0);
__decorate([
    State()
], ElsaQuestionComponent.prototype, "iconProvider", void 0);
__decorate([
    Event({
        eventName: 'updateQuestion',
        composed: true,
        cancelable: true,
        bubbles: true,
    })
], ElsaQuestionComponent.prototype, "updateQuestion", void 0);
ElsaQuestionComponent = __decorate([
    Component({
        tag: 'elsa-question',
        shadow: false,
    })
], ElsaQuestionComponent);
export { ElsaQuestionComponent };
