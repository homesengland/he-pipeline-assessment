import { r as registerInstance, e as createEvent, h } from './index-1542df5c.js';
import { S as SyntaxNames } from './constants-6ea82f24.js';
import { g as getUniversalUniqueId, p as parseJson } from './utils-89b7e981.js';
import './index-912d1a21.js';

const HEMultiTextProperty = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.expressionChanged = createEvent(this, "expressionChanged", 7);
    this.selectList = { items: [], isFlagsEnum: false };
    this.activityModel = undefined;
    this.propertyDescriptor = undefined;
    this.propertyModel = undefined;
    this.keyId = undefined;
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

export { HEMultiTextProperty as he_multi_text_property };
