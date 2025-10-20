import { r as registerInstance, h } from './index-CL6j2ec2.js';
import { S as SyntaxNames } from './index-D7wXd6HU.js';
import { T as Tunnel } from './workflow-editor-pBAZ9Py8.js';
import { i as iconProvider, I as IconName, a as IconColor } from './icon-provider-BX6jwQM3.js';
import './events-CpKc8CLe.js';
import './index-C-8L13GY.js';

const ElsaDictionaryProperty = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
        this.jsonToDictionary = (json) => {
            if (!json)
                return [['', '']];
            const parsedValue = JSON.parse(json);
            return Object.keys(parsedValue).map(key => [key, parsedValue[key]]);
        };
        this.dictionaryToJson = (dictionary) => {
            const filteredDictionary = this.removeInvalidKeys(dictionary);
            if (filteredDictionary.length === 0)
                return null;
            return JSON.stringify(Object.fromEntries(filteredDictionary));
        };
        this.removeInvalidKeys = (dictionary) => {
            const filteredDictionary = [];
            dictionary.forEach(x => {
                const key = x[0].trim();
                if (key !== '' && !filteredDictionary.some(y => y[0].trim() === key))
                    filteredDictionary.push(x);
            });
            return filteredDictionary;
        };
        this.onRowAdded = () => {
            //changing contents of array won't trigger state change,
            //need to update the reference by creating new array
            this.currentValue = [...this.currentValue, ['', '']];
        };
        this.onRowDeleted = (index) => {
            const newValue = this.currentValue.filter((x, i) => i !== index);
            if (newValue.length === 0)
                newValue.push(['', '']);
            this.currentValue = newValue;
            this.propertyModel.expressions[SyntaxNames.Json] = this.dictionaryToJson(newValue);
        };
    }
    async componentWillLoad() {
        this.currentValue = this.jsonToDictionary(this.propertyModel.expressions[SyntaxNames.Json] || null);
        if (this.currentValue.length === 0)
            this.currentValue = [['', '']];
    }
    onDefaultSyntaxValueChanged(e) {
        this.currentValue = this.jsonToDictionary(e.detail);
    }
    onKeyChanged(e, index) {
        const input = e.currentTarget;
        this.currentValue[index][0] = input.value;
        this.propertyModel.expressions[SyntaxNames.Json] = this.dictionaryToJson(this.currentValue);
    }
    onValueChanged(e, index) {
        const input = e.currentTarget;
        this.currentValue[index][1] = input.value;
        this.propertyModel.expressions[SyntaxNames.Json] = this.dictionaryToJson(this.currentValue);
    }
    render() {
        const propertyDescriptor = this.propertyDescriptor;
        const propertyModel = this.propertyModel;
        const fieldId = propertyDescriptor.name;
        const items = this.currentValue;
        return (h("elsa-property-editor", { key: 'cac0ccf17bb4c58866a7e00f0393fa7632ea54d8', propertyDescriptor: propertyDescriptor, propertyModel: propertyModel, activityModel: this.activityModel, onDefaultSyntaxValueChanged: e => this.onDefaultSyntaxValueChanged(e), "single-line": true }, items.map((item, index) => {
            const keyInputId = `${fieldId}_${index}_key}`;
            const valueInputId = `${fieldId}_${index}_value}`;
            const [key, value] = item;
            const isLast = index === (items.length - 1);
            return (h("div", { class: "elsa-flex elsa-flex-row elsa-justify-between elsa-mb-2" }, h("input", { id: keyInputId, type: "text", value: key, onChange: (e) => this.onKeyChanged(e, index), placeholder: "Name", class: "disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-border-gray-300 sm:elsa-text-sm elsa-rounded-md elsa-w-5/12" }), h("input", { id: valueInputId, type: "text", value: value, onChange: (e) => this.onValueChanged(e, index), placeholder: "Value", class: "disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-border-gray-300 sm:elsa-text-sm elsa-rounded-md elsa-w-5/12" }), h("div", { class: "elsa-flex elsa-flex-row elsa-justify-between elsa-w-24" }, h("button", { type: "button", onClick: () => this.onRowDeleted(index) }, iconProvider.getIcon(IconName.TrashBinOutline, { color: IconColor.Gray, hoverColor: IconColor.Red })), isLast && h("button", { type: "button", onClick: this.onRowAdded }, iconProvider.getIcon(IconName.Plus, { color: IconColor.Gray, hoverColor: IconColor.Green })))));
        })));
    }
};
Tunnel.injectProps(ElsaDictionaryProperty, ['serverUrl']);

export { ElsaDictionaryProperty as elsa_dictionary_property };
//# sourceMappingURL=elsa-dictionary-property.entry.esm.js.map
