var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, h, Event, Prop, State } from '@stencil/core';
import { SyntaxNames } from '../../../constants/constants';
import { getUniversalUniqueId } from '../../../utils/utils';
let HeSingleLineProperty = 
//Copy of Elsa Switch Case
//Copied to allow us control over how the expression editor is displayed.
class HeSingleLineProperty {
    onChange(e) {
        const input = e.currentTarget;
        const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
        this.propertyModel.expressions[defaultSyntax] = this.currentValue = input.value;
        this.expressionChanged.emit(JSON.stringify(this.propertyModel));
    }
    componentWillLoad() {
        const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
        this.currentValue = this.propertyModel.expressions[defaultSyntax] || undefined;
    }
    componentWillRender() {
        const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
        this.currentValue = this.propertyModel.expressions[defaultSyntax] || undefined;
        this.keyId = getUniversalUniqueId();
    }
    onDefaultSyntaxValueChanged(e) {
        this.currentValue = e.detail;
    }
    render() {
        const propertyDescriptor = this.propertyDescriptor;
        const propertyModel = this.propertyModel;
        const propertyName = propertyDescriptor.name;
        const isReadOnly = propertyDescriptor.isReadOnly;
        const fieldId = propertyName;
        const fieldName = propertyName;
        let value = this.currentValue;
        if (value == undefined) {
            const defaultValue = this.propertyDescriptor.defaultValue;
            value = defaultValue ? defaultValue.toString() : undefined;
        }
        if (isReadOnly) {
            const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
            this.propertyModel.expressions[defaultSyntax] = value;
        }
        return (h("he-property-editor", { key: `property-editor-${fieldId}-${this.keyId}`, activityModel: this.activityModel, propertyDescriptor: propertyDescriptor, propertyModel: propertyModel, onDefaultSyntaxValueChanged: e => this.onDefaultSyntaxValueChanged(e), "editor-height": "100%", "single-line": true },
            h("input", { type: "number", id: fieldId, name: fieldName, max: 5000, value: value, onChange: e => this.onChange(e), class: "disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300", disabled: isReadOnly })));
    }
};
__decorate([
    Prop()
], HeSingleLineProperty.prototype, "activityModel", void 0);
__decorate([
    Prop()
], HeSingleLineProperty.prototype, "propertyDescriptor", void 0);
__decorate([
    Prop()
], HeSingleLineProperty.prototype, "propertyModel", void 0);
__decorate([
    Prop()
], HeSingleLineProperty.prototype, "keyId", void 0);
__decorate([
    State()
], HeSingleLineProperty.prototype, "currentValue", void 0);
__decorate([
    Event()
], HeSingleLineProperty.prototype, "expressionChanged", void 0);
HeSingleLineProperty = __decorate([
    Component({
        tag: 'he-numeric-property',
        shadow: false,
    })
    //Copy of Elsa Switch Case
    //Copied to allow us control over how the expression editor is displayed.
], HeSingleLineProperty);
export { HeSingleLineProperty };
