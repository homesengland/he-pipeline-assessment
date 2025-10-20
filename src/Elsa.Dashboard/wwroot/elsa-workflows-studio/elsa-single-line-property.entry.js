import { r as registerInstance, h } from './index-CL6j2ec2.js';
import { S as SyntaxNames } from './index-D7wXd6HU.js';
import './events-CpKc8CLe.js';

const ElsaSingleLineProperty = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
    }
    onChange(e) {
        const input = e.currentTarget;
        const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
        this.propertyModel.expressions[defaultSyntax] = this.currentValue = input.value;
    }
    componentWillLoad() {
        const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
        this.currentValue = this.propertyModel.expressions[defaultSyntax] || undefined;
    }
    onDefaultSyntaxValueChanged(e) {
        this.currentValue = e.detail;
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
        return (h("elsa-property-editor", { key: '3d47f53448b7bf024f970624035ff10e78ada4b4', activityModel: this.activityModel, propertyDescriptor: propertyDescriptor, propertyModel: propertyModel, onDefaultSyntaxValueChanged: e => this.onDefaultSyntaxValueChanged(e), "editor-height": "5em", "single-line": true }, h("input", { key: '58444eb2b5ae956da7fc31636101e7dbb96d5c76', type: "text", id: fieldId, name: fieldName, value: value, onChange: e => this.onChange(e), class: "disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300", disabled: isReadOnly })));
    }
};

export { ElsaSingleLineProperty as elsa_single_line_property };
//# sourceMappingURL=elsa-single-line-property.entry.esm.js.map
