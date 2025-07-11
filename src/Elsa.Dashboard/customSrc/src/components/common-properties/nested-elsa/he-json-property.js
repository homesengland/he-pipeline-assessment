var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, h, Event, Prop, State } from '@stencil/core';
import { SyntaxNames } from '../../../constants/constants';
import { getUniversalUniqueId } from '../../../utils/utils';
let HEJsonProperty = 
//Copy of Elsa Switch Case
//Copied to allow us control over how the expression editor is displayed.
class HEJsonProperty {
    componentWillLoad() {
        const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Json;
        this.currentValue = this.propertyModel.expressions[defaultSyntax] || undefined;
    }
    getEditorHeight(options) {
        const editorHeightName = options.editorHeight || 'Large';
        switch (editorHeightName) {
            case 'Large':
                return '20em';
            case 'Default':
            default:
                return '15em';
        }
    }
    onMonacoValueChanged(e) {
        this.propertyModel.expressions[SyntaxNames.Json] = this.currentValue = e.value;
    }
    onDefaultSyntaxValueChanged(e) {
        this.currentValue = e.detail;
    }
    componentWillRender() {
        this.keyId = getUniversalUniqueId();
    }
    render() {
        const propertyDescriptor = this.propertyDescriptor;
        const propertyModel = this.propertyModel;
        const options = propertyDescriptor.options || {};
        const editorHeight = this.getEditorHeight(options);
        const context = options.context;
        let value = this.currentValue;
        if (value == undefined) {
            const defaultValue = this.propertyDescriptor.defaultValue;
            value = defaultValue ? defaultValue.toString() : undefined;
        }
        return (h("he-property-editor", { key: `property-editor-${this.keyId}`, activityModel: this.activityModel, propertyDescriptor: propertyDescriptor, propertyModel: propertyModel, onDefaultSyntaxValueChanged: e => this.onDefaultSyntaxValueChanged(e), "editor-height": editorHeight, context: context },
            h("elsa-monaco", { value: value, language: "json", "editor-height": editorHeight, onValueChanged: e => this.onMonacoValueChanged(e.detail) })));
    }
};
__decorate([
    Prop()
], HEJsonProperty.prototype, "activityModel", void 0);
__decorate([
    Prop()
], HEJsonProperty.prototype, "propertyDescriptor", void 0);
__decorate([
    Prop()
], HEJsonProperty.prototype, "propertyModel", void 0);
__decorate([
    Prop()
], HEJsonProperty.prototype, "keyId", void 0);
__decorate([
    Event()
], HEJsonProperty.prototype, "expressionChanged", void 0);
__decorate([
    State()
], HEJsonProperty.prototype, "currentValue", void 0);
HEJsonProperty = __decorate([
    Component({
        tag: 'he-json-property',
        shadow: false,
    })
    //Copy of Elsa Switch Case
    //Copied to allow us control over how the expression editor is displayed.
], HEJsonProperty);
export { HEJsonProperty };
