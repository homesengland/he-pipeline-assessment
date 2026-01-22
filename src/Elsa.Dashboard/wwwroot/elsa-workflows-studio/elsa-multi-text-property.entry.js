import { r as registerInstance, e as createEvent, h } from './index-1542df5c.js';
import { S as SyntaxNames } from './index-1654a48d.js';
import { p as parseJson } from './utils-db96334c.js';
import { T as Tunnel } from './workflow-editor-eaa88887.js';
import { g as getSelectListItems } from './select-list-items-5261654d.js';
import './events-d0aab14a.js';
import './collection-ba09a015.js';
import './_commonjsHelpers-6cb8dacb.js';
import './index-2db7bf78.js';
import './index-892f713d.js';
import './elsa-client-8304c78c.js';
import './fetch-client-f0dc2a52.js';
import './event-bus-5d6f3774.js';
import './cronstrue-37d55fa1.js';

const ElsaMultiTextProperty = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.valueChange = createEvent(this, "valueChange", 7);
    this.selectList = { items: [], isFlagsEnum: false };
    this.activityModel = undefined;
    this.propertyDescriptor = undefined;
    this.propertyModel = undefined;
    this.serverUrl = undefined;
    this.currentValue = undefined;
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
