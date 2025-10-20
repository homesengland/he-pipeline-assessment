import { r as registerInstance, a as createEvent, h } from './index-CL6j2ec2.js';
import { S as SyntaxNames } from './index-D7wXd6HU.js';
import './events-CpKc8CLe.js';

const elsaPropertyEditorCss = ":host{display:block}";

const ElsaPropertyEditor = class {
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
        return h("div", { key: '280b45a9df1965e94d2ffb36f8c58ce9f906101c' }, h("elsa-multi-expression-editor", { key: 'adf842aca00d6029b547ef88b5a2b6ea89d4ca82', onSyntaxChanged: e => this.onSyntaxChanged(e), onExpressionChanged: e => this.onExpressionChanged(e), fieldName: fieldName, label: label, syntax: propertyModel.syntax, defaultSyntax: propertyDescriptor.defaultSyntax, isReadOnly: propertyDescriptor.isReadOnly, expressions: propertyModel.expressions, supportedSyntaxes: propertyDescriptor.supportedSyntaxes, "editor-height": this.editorHeight, context: context }, h("slot", { key: 'be62053d298553c6184deeb53c65c0a8a2eb6e8b' })), fieldHint ? h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, fieldHint) : undefined);
    }
};
ElsaPropertyEditor.style = elsaPropertyEditorCss;

export { ElsaPropertyEditor as elsa_property_editor };
//# sourceMappingURL=elsa-property-editor.entry.esm.js.map
