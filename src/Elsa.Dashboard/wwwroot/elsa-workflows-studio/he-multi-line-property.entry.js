import { r as registerInstance, e as createEvent, h } from './index-1542df5c.js';
import { S as SyntaxNames } from './constants-6ea82f24.js';
import { g as getUniversalUniqueId } from './utils-89b7e981.js';
import './index-912d1a21.js';

const HEMultiLineProperty = class {
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
    const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
    this.currentValue = this.propertyModel.expressions[defaultSyntax] || undefined;
  }
  getEditorHeight(options) {
    const editorHeightName = options.editorHeight || 'Default';
    switch (editorHeightName) {
      case 'Large':
        return { propertyEditor: '20em', textArea: 6 };
      case 'Default':
      default:
        return { propertyEditor: '15em', textArea: 3 };
    }
  }
  onChange(e) {
    const input = e.currentTarget;
    this.propertyModel.expressions['Literal'] = this.currentValue = input.value;
  }
  onDefaultSyntaxValueChanged(e) {
    this.currentValue = e.detail;
  }
  componentWillRender() {
    const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
    this.currentValue = this.propertyModel.expressions[defaultSyntax] || undefined;
    this.keyId = getUniversalUniqueId();
  }
  render() {
    const propertyDescriptor = this.propertyDescriptor;
    const propertyModel = this.propertyModel;
    const propertyName = propertyDescriptor.name;
    const options = propertyDescriptor.options || {};
    const editorHeight = this.getEditorHeight(options);
    const context = options.context;
    const fieldId = propertyName;
    const fieldName = propertyName;
    let value = this.currentValue;
    if (value == undefined) {
      const defaultValue = this.propertyDescriptor.defaultValue;
      value = defaultValue ? defaultValue.toString() : undefined;
    }
    return (h("he-property-editor", { key: `property-editor-${this.keyId}`, activityModel: this.activityModel, propertyDescriptor: propertyDescriptor, propertyModel: propertyModel, onDefaultSyntaxValueChanged: e => this.onDefaultSyntaxValueChanged(e), "editor-height": editorHeight.propertyEditor, context: context }, h("textarea", { id: fieldId, name: fieldName, value: value, onChange: e => this.onChange(e), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300", rows: editorHeight.textArea })));
  }
};

export { HEMultiLineProperty as he_multi_line_property };
