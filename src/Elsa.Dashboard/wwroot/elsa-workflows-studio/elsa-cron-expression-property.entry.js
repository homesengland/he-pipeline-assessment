import { r as registerInstance, h } from './index-CL6j2ec2.js';
import { c as cronstrue } from './cronstrue-BvVNjwUa.js';
import { S as SyntaxNames } from './index-D7wXd6HU.js';
import './_commonjsHelpers-Cf5sKic0.js';
import './events-CpKc8CLe.js';

const ElsaCronExpressionProperty = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
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
        return (h("elsa-property-editor", { key: 'a913595faab45eda65b3a6a7155522454dc68bff', activityModel: this.activityModel, propertyDescriptor: propertyDescriptor, propertyModel: propertyModel, onDefaultSyntaxValueChanged: e => this.onDefaultSyntaxValueChanged(e), "editor-height": "5em", "single-line": true }, h("div", { key: '6d07d6eb07a55ea07bf43067a4ff2b0802febfca' }, h("input", { key: '65c7cb94ca73b3d01083f9f09b60dfd97f8da53d', type: "text", id: fieldId, name: fieldName, value: value, onChange: e => this.onChange(e), class: "disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300", disabled: isReadOnly }), h("p", { key: '860e11f3dd77ce124f93622f04319123fa661258', class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, this.valueDescription))));
    }
};

export { ElsaCronExpressionProperty as elsa_cron_expression_property };
//# sourceMappingURL=elsa-cron-expression-property.entry.esm.js.map
