var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, h, Event, Prop, State } from '@stencil/core';
import { SyntaxNames } from '../../../constants/constants';
import { getUniversalUniqueId } from '../../../utils/utils';
let HECheckboxProperty = 
//Copy of Elsa Checkbox Property
//Copied to allow us control over how the expression editor is displayed.
class HECheckboxProperty {
    async componentWillLoad() {
        var _a;
        this.isChecked = (this.propertyModel.expressions[SyntaxNames.Literal] || ((_a = this.propertyDescriptor.defaultValue) === null || _a === void 0 ? void 0 : _a.toString()) || '').toLowerCase() == 'true';
    }
    onCheckChanged(e) {
        const checkbox = e.target;
        this.isChecked = checkbox.checked;
        const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
        this.propertyModel.expressions[defaultSyntax] = this.isChecked.toString();
        this.expressionChanged.emit(JSON.stringify(this.propertyModel));
    }
    onDefaultSyntaxValueChanged(e) {
        this.isChecked = (e.detail || '').toLowerCase() == 'true';
    }
    componentWillRender() {
        var _a;
        this.isChecked = (this.propertyModel.expressions[SyntaxNames.Literal] || ((_a = this.propertyDescriptor.defaultValue) === null || _a === void 0 ? void 0 : _a.toString()) || '').toLowerCase() == 'true';
        this.keyId = getUniversalUniqueId();
    }
    render() {
        const propertyDescriptor = this.propertyDescriptor;
        const propertyModel = this.propertyModel;
        const propertyName = propertyDescriptor.name;
        const fieldId = propertyName;
        const fieldName = propertyName;
        const fieldLabel = propertyDescriptor.label || propertyName;
        let isChecked = this.isChecked;
        return (h("he-property-editor", { key: `property-editor-${fieldId}-${this.keyId}`, activityModel: this.activityModel, propertyDescriptor: propertyDescriptor, propertyModel: propertyModel, onDefaultSyntaxValueChanged: e => this.onDefaultSyntaxValueChanged(e), "single-line": true, showLabel: false },
            h("div", { class: "elsa-max-w-lg" },
                h("div", { class: "elsa-relative elsa-flex elsa-items-start" },
                    h("div", { class: "elsa-flex elsa-items-center elsa-h-5" },
                        h("input", { id: fieldId, name: fieldName, type: "checkbox", checked: isChecked, value: 'true', onChange: e => this.onCheckChanged(e), class: "focus:elsa-ring-blue-500 elsa-h-4 elsa-w-4 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" })),
                    h("div", { class: "elsa-ml-3 elsa-text-sm" },
                        h("label", { htmlFor: fieldId, class: "elsa-font-medium elsa-text-gray-700" }, fieldLabel))))));
    }
};
__decorate([
    Prop()
], HECheckboxProperty.prototype, "activityModel", void 0);
__decorate([
    Prop()
], HECheckboxProperty.prototype, "propertyDescriptor", void 0);
__decorate([
    Prop()
], HECheckboxProperty.prototype, "propertyModel", void 0);
__decorate([
    Prop()
], HECheckboxProperty.prototype, "keyId", void 0);
__decorate([
    Event()
], HECheckboxProperty.prototype, "expressionChanged", void 0);
__decorate([
    State()
], HECheckboxProperty.prototype, "isChecked", void 0);
HECheckboxProperty = __decorate([
    Component({
        tag: 'he-checkbox-property',
        shadow: false,
    })
    //Copy of Elsa Checkbox Property
    //Copied to allow us control over how the expression editor is displayed.
], HECheckboxProperty);
export { HECheckboxProperty };
