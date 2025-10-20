import { r as registerInstance, h } from './index-CL6j2ec2.js';
import { S as SyntaxNames } from './index-D7wXd6HU.js';
import { g as getSelectListItems } from './select-list-items-qT1HJ7dW.js';
import { T as Tunnel } from './workflow-editor-pBAZ9Py8.js';
import './events-CpKc8CLe.js';
import './index-fZDMH_YE.js';
import './elsa-client-q6ob5JPZ.js';
import './fetch-client-1OcjQcrw.js';
import './event-bus-axQqcjdg.js';
import './collection-B4sYCr2r.js';
import './_commonjsHelpers-Cf5sKic0.js';
import './utils-C0M_5Llz.js';
import './cronstrue-BvVNjwUa.js';
import './index-C-8L13GY.js';

const ElsaRadioListProperty = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
        this.selectList = { items: [], isFlagsEnum: false };
    }
    async componentWillLoad() {
        const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
        this.currentValue = this.propertyModel.expressions[defaultSyntax] || null;
    }
    onCheckChanged(e) {
        const radio = e.target;
        const checked = radio.checked;
        if (checked)
            this.currentValue = radio.value;
        const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
        this.propertyModel.expressions[defaultSyntax] = this.currentValue;
    }
    onDefaultSyntaxValueChanged(e) {
        this.currentValue = e.detail;
    }
    async componentWillRender() {
        this.selectList = await getSelectListItems(this.serverUrl, this.propertyDescriptor);
    }
    render() {
        const propertyDescriptor = this.propertyDescriptor;
        const propertyModel = this.propertyModel;
        const fieldId = propertyDescriptor.name;
        const selectList = this.selectList;
        const items = selectList.items;
        const currentValue = this.currentValue;
        return (h("elsa-property-editor", { key: '20b0df2458ae6d8a8ff7fa772303b716f5fbcdb6', activityModel: this.activityModel, propertyDescriptor: propertyDescriptor, propertyModel: propertyModel, onDefaultSyntaxValueChanged: e => this.onDefaultSyntaxValueChanged(e), "single-line": true }, h("div", { key: 'adbf99113ad79178910879944c60bbe11dce3c2e', class: "elsa-max-w-lg elsa-space-y-3 elsa-my-4" }, items.map((item, index) => {
            const inputId = `${fieldId}_${index}`;
            const optionIsString = typeof (item) == 'string';
            const value = optionIsString ? item : item.value;
            const text = optionIsString ? item : item.text;
            const isSelected = currentValue == value;
            return (h("div", { class: "elsa-relative elsa-flex elsa-items-start" }, h("div", { class: "elsa-flex elsa-items-center elsa-h-5" }, h("input", { id: inputId, type: "radio", radioGroup: fieldId, checked: isSelected, value: value, onChange: e => this.onCheckChanged(e), class: "elsa-focus:ring-blue-500 elsa-h-4 elsa-w-4 elsa-text-blue-600 elsa-border-gray-300" })), h("div", { class: "elsa-ml-3 elsa-mt-1 elsa-text-sm" }, h("label", { htmlFor: inputId, class: "elsa-font-medium elsa-text-gray-700" }, text))));
        }))));
    }
};
Tunnel.injectProps(ElsaRadioListProperty, ['serverUrl']);

export { ElsaRadioListProperty as elsa_radio_list_property };
//# sourceMappingURL=elsa-radio-list-property.entry.esm.js.map
