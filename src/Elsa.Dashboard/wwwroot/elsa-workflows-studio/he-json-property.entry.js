import { r as registerInstance, e as createEvent, h } from './index-1542df5c.js';
import { S as SyntaxNames } from './constants-6ea82f24.js';
import { g as getUniversalUniqueId } from './utils-89b7e981.js';
import './index-912d1a21.js';

const HEJsonProperty = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.expressionChanged = createEvent(this, "expressionChanged", 7);
    this.activityModel = undefined;
    this.propertyDescriptor = undefined;
    this.propertyModel = undefined;
    this.keyId = undefined;
    this.currentValue = undefined;
  }
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
    return (h("he-property-editor", { key: `property-editor-${this.keyId}`, activityModel: this.activityModel, propertyDescriptor: propertyDescriptor, propertyModel: propertyModel, onDefaultSyntaxValueChanged: e => this.onDefaultSyntaxValueChanged(e), "editor-height": editorHeight, context: context }, h("elsa-monaco", { value: value, language: "json", "editor-height": editorHeight, onValueChanged: e => this.onMonacoValueChanged(e.detail) })));
  }
};

export { HEJsonProperty as he_json_property };
