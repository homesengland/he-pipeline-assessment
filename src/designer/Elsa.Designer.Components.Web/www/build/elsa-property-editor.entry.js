import { r as registerInstance, e as createEvent, h } from './index-ea213ee1.js';
import { S as SyntaxNames } from './index-0f68dbd6.js';

const elsaPropertyEditorCss = "";

let ElsaPropertyEditor = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.defaultSyntaxValueChanged = createEvent(this, "defaultSyntaxValueChanged", 7);
    this.editorHeight = '10em';
    this.singleLineMode = false;
    this.showLabel = true;
  }
  onSyntaxChanged(e) {
    this.propertyModel.syntax = e.detail;
  }
  onExpressionChanged(e) {
    const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
    const syntax = this.propertyModel.syntax || defaultSyntax;
    this.propertyModel.expressions[syntax] = e.detail;
    if (syntax != defaultSyntax)
      return;
    this.defaultSyntaxValueChanged.emit(e.detail);
  }
  render() {
    const propertyDescriptor = this.propertyDescriptor;
    const propertyModel = this.propertyModel;
    const fieldHint = propertyDescriptor.hint;
    const fieldName = propertyDescriptor.name;
    const label = this.showLabel ? propertyDescriptor.label : null;
    const context = {
      propertyName: propertyDescriptor.name,
      activityTypeName: this.activityModel.type
    };
    return h("div", null, h("elsa-multi-expression-editor", { onSyntaxChanged: e => this.onSyntaxChanged(e), onExpressionChanged: e => this.onExpressionChanged(e), fieldName: fieldName, label: label, syntax: propertyModel.syntax, defaultSyntax: propertyDescriptor.defaultSyntax, isReadOnly: propertyDescriptor.isReadOnly, expressions: propertyModel.expressions, supportedSyntaxes: propertyDescriptor.supportedSyntaxes, "editor-height": this.editorHeight, context: context }, h("slot", null)), fieldHint ? h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, fieldHint) : undefined);
  }
};
ElsaPropertyEditor.style = elsaPropertyEditorCss;

export { ElsaPropertyEditor as elsa_property_editor };
