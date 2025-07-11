var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, Event, h, Prop, State } from '@stencil/core';
import { SyntaxNames } from '../../../constants/constants';
import { getUniversalUniqueId } from '../../../utils/utils';
import { parseJson } from '../../../utils/utils';
let HEMultiTextProperty = 
//Copy of Elsa Switch Case
//Copied to allow us control over how the expression editor is displayed.
class HEMultiTextProperty {
    constructor() {
        this.selectList = { items: [], isFlagsEnum: false };
    }
    async componentWillLoad() {
        this.currentValue = this.propertyModel.expressions[SyntaxNames.Json] || '[]';
    }
    onValueChanged(newValue) {
        const newValues = newValue.map(item => {
            if (typeof item === 'string')
                return item;
            if (typeof item === 'number')
                return item.toString();
            if (typeof item === 'boolean')
                return item.toString();
            return item.value;
        });
        this.currentValue = JSON.stringify(newValues);
        this.propertyModel.expressions[SyntaxNames.Json] = this.currentValue;
        this.expressionChanged.emit(JSON.stringify(this.propertyModel));
    }
    onDefaultSyntaxValueChanged(e) {
        this.currentValue = e.detail;
    }
    createKeyValueOptions(options) {
        if (options === null)
            return options;
        return options.map(option => typeof option === 'string' ? { text: option, value: option } : option);
    }
    componentWillRender() {
        this.currentValue = this.propertyModel.expressions[SyntaxNames.Json] || '[]';
        this.keyId = getUniversalUniqueId();
    }
    render() {
        const propertyDescriptor = this.propertyDescriptor;
        const propertyModel = this.propertyModel;
        const propertyName = propertyDescriptor.name;
        const fieldId = propertyName;
        const fieldName = propertyName;
        const values = parseJson(this.currentValue);
        /*const items = this.selectList.items as Array<SelectListItem>;*/
        //const useDropdown = !!propertyDescriptor.options && propertyDescriptor.options.length > 0;
        /*const propertyOptions = this.createKeyValueOptions(items);*/
        const elsaInputTags = 
        //useDropdown ?
        //<elsa-input-tags-dropdown dropdownValues={propertyOptions} values={values} fieldId={fieldId} fieldName={fieldName}
        //  onValueChanged={e => this.onValueChanged(e.detail)} /> :
        h("elsa-input-tags", { values: values, fieldId: fieldId, fieldName: fieldName, onValueChanged: e => this.onValueChanged(e.detail) });
        return (h("he-property-editor", { key: `property-editor-${this.keyId}`, activityModel: this.activityModel, propertyDescriptor: propertyDescriptor, propertyModel: propertyModel, onDefaultSyntaxValueChanged: e => this.onDefaultSyntaxValueChanged(e), "single-line": true }, elsaInputTags));
    }
};
__decorate([
    Prop()
], HEMultiTextProperty.prototype, "activityModel", void 0);
__decorate([
    Prop()
], HEMultiTextProperty.prototype, "propertyDescriptor", void 0);
__decorate([
    Prop()
], HEMultiTextProperty.prototype, "propertyModel", void 0);
__decorate([
    Prop()
], HEMultiTextProperty.prototype, "keyId", void 0);
__decorate([
    State()
], HEMultiTextProperty.prototype, "currentValue", void 0);
__decorate([
    Event()
], HEMultiTextProperty.prototype, "expressionChanged", void 0);
HEMultiTextProperty = __decorate([
    Component({
        tag: 'he-multi-text-property',
        shadow: false,
    })
    //Copy of Elsa Switch Case
    //Copied to allow us control over how the expression editor is displayed.
], HEMultiTextProperty);
export { HEMultiTextProperty };
