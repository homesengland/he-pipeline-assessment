import { r as registerInstance, h } from './index-1542df5c.js';
import { S as SyntaxNames } from './index-1654a48d.js';
import { g as getSelectListItems } from './select-list-items-5261654d.js';
import { T as Tunnel } from './workflow-editor-eaa88887.js';
import './events-d0aab14a.js';
import './index-892f713d.js';
import './elsa-client-8304c78c.js';
import './fetch-client-f0dc2a52.js';
import './event-bus-5d6f3774.js';
import './collection-ba09a015.js';
import './_commonjsHelpers-6cb8dacb.js';
import './utils-db96334c.js';
import './cronstrue-37d55fa1.js';
import './index-2db7bf78.js';

const ElsaRadioListProperty = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.selectList = { items: [], isFlagsEnum: false };
    this.activityModel = undefined;
    this.propertyDescriptor = undefined;
    this.propertyModel = undefined;
    this.serverUrl = undefined;
    this.currentValue = undefined;
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
    return (h("elsa-property-editor", { activityModel: this.activityModel, propertyDescriptor: propertyDescriptor, propertyModel: propertyModel, onDefaultSyntaxValueChanged: e => this.onDefaultSyntaxValueChanged(e), "single-line": true }, h("div", { class: "elsa-max-w-lg elsa-space-y-3 elsa-my-4" }, items.map((item, index) => {
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
