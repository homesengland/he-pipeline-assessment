import { r as registerInstance, e as createEvent, h } from './index-ea213ee1.js';
import { S as SyntaxNames } from './index-0f68dbd6.js';
import { p as parseJson } from './utils-db96334c.js';
import { T as Tunnel } from './workflow-editor-3a84ee12.js';
import { g as getSelectListItems } from './select-list-items-37e28b87.js';
import './collection-ba09a015.js';
import './_commonjsHelpers-6cb8dacb.js';
import './index-2db7bf78.js';
import './index-c5018c3a.js';
import './elsa-client-ecb85def.js';
import './axios-middleware.esm-fcda64d5.js';
import './event-bus-6625fc04.js';
import './cronstrue-37d55fa1.js';

let ElsaMultiTextProperty = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.valueChange = createEvent(this, "valueChange", 7);
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
    this.valueChange.emit(newValue);
    this.currentValue = JSON.stringify(newValues);
    this.propertyModel.expressions[SyntaxNames.Json] = this.currentValue;
  }
  onDefaultSyntaxValueChanged(e) {
    this.currentValue = e.detail;
  }
  createKeyValueOptions(options) {
    if (options === null)
      return options;
    return options.map(option => typeof option === 'string' ? { text: option, value: option } : option);
  }
  async componentWillRender() {
    this.selectList = await getSelectListItems(this.serverUrl, this.propertyDescriptor);
  }
  render() {
    const propertyDescriptor = this.propertyDescriptor;
    const propertyModel = this.propertyModel;
    const propertyName = propertyDescriptor.name;
    const fieldId = propertyName;
    const fieldName = propertyName;
    const values = parseJson(this.currentValue);
    const items = this.selectList.items;
    const useDropdown = !!propertyDescriptor.options && Array.isArray(propertyDescriptor.options) && propertyDescriptor.options.length > 0;
    const propertyOptions = this.createKeyValueOptions(items);
    const elsaInputTags = useDropdown ?
      h("elsa-input-tags-dropdown", { dropdownValues: propertyOptions, values: values, fieldId: fieldId, fieldName: fieldName, onValueChanged: e => this.onValueChanged(e.detail) }) :
      h("elsa-input-tags", { values: values, fieldId: fieldId, fieldName: fieldName, onValueChanged: e => this.onValueChanged(e.detail) });
    return (h("elsa-property-editor", { activityModel: this.activityModel, propertyDescriptor: propertyDescriptor, propertyModel: propertyModel, onDefaultSyntaxValueChanged: e => this.onDefaultSyntaxValueChanged(e), "single-line": true }, elsaInputTags));
  }
};
Tunnel.injectProps(ElsaMultiTextProperty, ['serverUrl']);

export { ElsaMultiTextProperty as elsa_multi_text_property };
