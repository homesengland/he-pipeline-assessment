import { r as registerInstance, e as createEvent, h } from './index-1542df5c.js';
import { S as SyntaxNames } from './constants-6ea82f24.js';
import { g as getUniversalUniqueId } from './utils-89b7e981.js';
import './index-912d1a21.js';

const HECheckboxProperty = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.expressionChanged = createEvent(this, "expressionChanged", 7);
    this.activityModel = undefined;
    this.propertyDescriptor = undefined;
    this.propertyModel = undefined;
    this.keyId = undefined;
    this.isChecked = undefined;
  }
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
    return (h("he-property-editor", { key: `property-editor-${fieldId}-${this.keyId}`, activityModel: this.activityModel, propertyDescriptor: propertyDescriptor, propertyModel: propertyModel, onDefaultSyntaxValueChanged: e => this.onDefaultSyntaxValueChanged(e), "single-line": true, showLabel: false }, h("div", { class: "elsa-max-w-lg" }, h("div", { class: "elsa-relative elsa-flex elsa-items-start" }, h("div", { class: "elsa-flex elsa-items-center elsa-h-5" }, h("input", { id: fieldId, name: fieldName, type: "checkbox", checked: isChecked, value: 'true', onChange: e => this.onCheckChanged(e), class: "focus:elsa-ring-blue-500 elsa-h-4 elsa-w-4 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" })), h("div", { class: "elsa-ml-3 elsa-text-sm" }, h("label", { htmlFor: fieldId, class: "elsa-font-medium elsa-text-gray-700" }, fieldLabel))))));
  }
};

export { HECheckboxProperty as he_checkbox_property };
