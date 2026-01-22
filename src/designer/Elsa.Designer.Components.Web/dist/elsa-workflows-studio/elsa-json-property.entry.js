import { r as registerInstance, h } from './index-ea213ee1.js';
import { S as SyntaxNames } from './index-0f68dbd6.js';

let ElsaJsonProperty = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
  }
  componentWillLoad() {
    const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Json;
    this.currentValue = this.propertyModel.expressions[defaultSyntax] || undefined;
  }
  getEditorHeight(options) {
    const editorHeightName = (options === null || options === void 0 ? void 0 : options.editorHeight) || 'Large';
    switch (editorHeightName) {
      case 'Large':
        return '20em';
    }
    return '15em';
  }
  onMonacoValueChanged(e) {
    this.propertyModel.expressions[SyntaxNames.Json] = this.currentValue = e.value;
  }
  onDefaultSyntaxValueChanged(e) {
    this.currentValue = e.detail;
  }
  render() {
    const propertyDescriptor = this.propertyDescriptor;
    const propertyModel = this.propertyModel;
    const options = propertyDescriptor.options;
    const editorHeight = this.getEditorHeight(options);
    const context = options === null || options === void 0 ? void 0 : options.context;
    let value = this.currentValue;
    if (value == undefined) {
      const defaultValue = this.propertyDescriptor.defaultValue;
      value = defaultValue ? defaultValue.toString() : undefined;
    }
    return (h("elsa-property-editor", { activityModel: this.activityModel, propertyDescriptor: propertyDescriptor, propertyModel: propertyModel, onDefaultSyntaxValueChanged: e => this.onDefaultSyntaxValueChanged(e), "editor-height": editorHeight, context: context }, h("elsa-monaco", { value: value, language: "json", "editor-height": editorHeight, onValueChanged: e => this.onMonacoValueChanged(e.detail) })));
  }
};

export { ElsaJsonProperty as elsa_json_property };
