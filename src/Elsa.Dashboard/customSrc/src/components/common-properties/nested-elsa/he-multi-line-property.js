var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, h, Event, Prop, State } from '@stencil/core';
import { SyntaxNames } from '../../../constants/constants';
import { getUniversalUniqueId } from '../../../utils/utils';
let HEMultiLineProperty = 
//Copy of Elsa Switch Case
//Copied to allow us control over how the expression editor is displayed.
class HEMultiLineProperty {
    componentWillLoad() {
        const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
        this.currentValue = this.propertyModel.expressions[defaultSyntax] || undefined;
    }
    getEditorHeight(options) {
        const editorHeightName = options.editorHeight || 'Default';
        switch (editorHeightName) {
            case 'Large':
                return { propertyEditor: '20em', textArea: 6 };
            case 'Default':
            default:
                return { propertyEditor: '15em', textArea: 3 };
        }
    }
    onChange(e) {
        const input = e.currentTarget;
        this.propertyModel.expressions['Literal'] = this.currentValue = input.value;
    }
    onDefaultSyntaxValueChanged(e) {
        this.currentValue = e.detail;
    }
    componentWillRender() {
        const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
        this.currentValue = this.propertyModel.expressions[defaultSyntax] || undefined;
        this.keyId = getUniversalUniqueId();
    }
    render() {
        const propertyDescriptor = this.propertyDescriptor;
        const propertyModel = this.propertyModel;
        const propertyName = propertyDescriptor.name;
        const options = propertyDescriptor.options || {};
        const editorHeight = this.getEditorHeight(options);
        const context = options.context;
        const fieldId = propertyName;
        const fieldName = propertyName;
        let value = this.currentValue;
        if (value == undefined) {
            const defaultValue = this.propertyDescriptor.defaultValue;
            value = defaultValue ? defaultValue.toString() : undefined;
        }
        return (h("he-property-editor", { key: `property-editor-${this.keyId}`, activityModel: this.activityModel, propertyDescriptor: propertyDescriptor, propertyModel: propertyModel, onDefaultSyntaxValueChanged: e => this.onDefaultSyntaxValueChanged(e), "editor-height": editorHeight.propertyEditor, context: context },
            h("textarea", { id: fieldId, name: fieldName, value: value, onChange: e => this.onChange(e), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300", rows: editorHeight.textArea })));
    }
};
__decorate([
    Prop()
], HEMultiLineProperty.prototype, "activityModel", void 0);
__decorate([
    Prop()
], HEMultiLineProperty.prototype, "propertyDescriptor", void 0);
__decorate([
    Prop()
], HEMultiLineProperty.prototype, "propertyModel", void 0);
__decorate([
    Prop()
], HEMultiLineProperty.prototype, "keyId", void 0);
__decorate([
    State()
], HEMultiLineProperty.prototype, "currentValue", void 0);
__decorate([
    Event()
], HEMultiLineProperty.prototype, "expressionChanged", void 0);
HEMultiLineProperty = __decorate([
    Component({
        tag: 'he-multi-line-property',
        shadow: false,
    })
    //Copy of Elsa Switch Case
    //Copied to allow us control over how the expression editor is displayed.
], HEMultiLineProperty);
export { HEMultiLineProperty };
