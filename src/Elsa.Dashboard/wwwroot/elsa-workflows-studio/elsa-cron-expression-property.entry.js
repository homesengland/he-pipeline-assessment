import { r as registerInstance, h } from './index-1542df5c.js';
import { c as cronstrue } from './cronstrue-37d55fa1.js';
import { S as SyntaxNames } from './index-1654a48d.js';
import './_commonjsHelpers-6cb8dacb.js';
import './events-d0aab14a.js';

const ElsaCronExpressionProperty = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.activityModel = undefined;
    this.propertyDescriptor = undefined;
    this.propertyModel = undefined;
    this.currentValue = undefined;
    this.valueDescription = undefined;
  }
  onChange(e) {
    const input = e.currentTarget;
    const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
    this.propertyModel.expressions[defaultSyntax] = this.currentValue = input.value;
    this.updateDescription();
  }
  componentWillLoad() {
    const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
    this.currentValue = this.propertyModel.expressions[defaultSyntax] || undefined;
    this.updateDescription();
  }
  onDefaultSyntaxValueChanged(e) {
    this.currentValue = e.detail;
  }
  updateDescription() {
    this.valueDescription = cronstrue.toString(this.currentValue, { throwExceptionOnParseError: false });
  }
  render() {
    const propertyDescriptor = this.propertyDescriptor;
    const propertyModel = this.propertyModel;
    const propertyName = propertyDescriptor.name;
    const isReadOnly = propertyDescriptor.isReadOnly;
    const fieldId = propertyName;
    const fieldName = propertyName;
    let value = this.currentValue;
    if (value == undefined) {
      const defaultValue = this.propertyDescriptor.defaultValue;
      value = defaultValue ? defaultValue.toString() : undefined;
    }
    if (isReadOnly) {
      const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
      this.propertyModel.expressions[defaultSyntax] = value;
    }
    return (h("elsa-property-editor", { activityModel: this.activityModel, propertyDescriptor: propertyDescriptor, propertyModel: propertyModel, onDefaultSyntaxValueChanged: e => this.onDefaultSyntaxValueChanged(e), "editor-height": "5em", "single-line": true }, h("div", null, h("input", { type: "text", id: fieldId, name: fieldName, value: value, onChange: e => this.onChange(e), class: "disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300", disabled: isReadOnly }), h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, this.valueDescription))));
  }
};

export { ElsaCronExpressionProperty as elsa_cron_expression_property };
