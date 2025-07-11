var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, h, Prop, Event } from '@stencil/core';
import { SyntaxNames } from "../../constants/constants";
let ElsaPropertyEditor = class ElsaPropertyEditor {
    constructor() {
        this.editorHeight = '10em';
        this.singleLineMode = false;
        this.showLabel = true;
    }
    onSyntaxChanged(e) {
        this.propertyModel.syntax = e.detail;
    }
    onExpressionChanged(e) {
        const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
        const syntax = this.propertyModel.syntax || defaultSyntax;
        this.propertyModel.expressions[syntax] = e.detail;
        if (syntax != defaultSyntax)
            return;
        this.defaultSyntaxValueChanged.emit(e.detail);
    }
    render() {
        const propertyDescriptor = this.propertyDescriptor;
        const propertyModel = this.propertyModel;
        const fieldHint = propertyDescriptor.hint;
        const fieldName = propertyDescriptor.name;
        const label = this.showLabel ? propertyDescriptor.label : null;
        const context = {
            propertyName: propertyDescriptor.name,
            activityTypeName: this.activityModel.type
        };
        return h("div", null,
            h("he-multi-expression-editor", { onSyntaxChanged: e => this.onSyntaxChanged(e), onExpressionChanged: e => this.onExpressionChanged(e), fieldName: fieldName, label: label, syntax: propertyModel.syntax, defaultSyntax: propertyDescriptor.defaultSyntax, isReadOnly: propertyDescriptor.isReadOnly, expressions: propertyModel.expressions, supportedSyntaxes: propertyDescriptor.supportedSyntaxes, "editor-height": this.editorHeight, context: context },
                h("slot", null)),
            fieldHint ? h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, fieldHint) : undefined);
    }
};
__decorate([
    Event()
], ElsaPropertyEditor.prototype, "defaultSyntaxValueChanged", void 0);
__decorate([
    Prop()
], ElsaPropertyEditor.prototype, "activityModel", void 0);
__decorate([
    Prop()
], ElsaPropertyEditor.prototype, "propertyDescriptor", void 0);
__decorate([
    Prop()
], ElsaPropertyEditor.prototype, "propertyModel", void 0);
__decorate([
    Prop({ attribute: 'editor-height', reflect: true })
], ElsaPropertyEditor.prototype, "editorHeight", void 0);
__decorate([
    Prop({ attribute: 'single-line', reflect: true })
], ElsaPropertyEditor.prototype, "singleLineMode", void 0);
__decorate([
    Prop({ attribute: 'context', reflect: true })
], ElsaPropertyEditor.prototype, "context", void 0);
__decorate([
    Prop()
], ElsaPropertyEditor.prototype, "showLabel", void 0);
ElsaPropertyEditor = __decorate([
    Component({
        tag: 'he-property-editor',
        shadow: false,
    })
], ElsaPropertyEditor);
export { ElsaPropertyEditor };
