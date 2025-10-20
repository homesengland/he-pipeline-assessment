import { r as registerInstance, h } from './index-CL6j2ec2.js';
import { S as SyntaxNames } from './index-D7wXd6HU.js';
import './events-CpKc8CLe.js';

const ElsaCheckBoxProperty = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
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
    }
    onDefaultSyntaxValueChanged(e) {
        this.isChecked = (e.detail || '').toLowerCase() == 'true';
    }
    render() {
        const propertyDescriptor = this.propertyDescriptor;
        const propertyModel = this.propertyModel;
        const propertyName = propertyDescriptor.name;
        const fieldId = propertyName;
        const fieldName = propertyName;
        const fieldLabel = propertyDescriptor.label || propertyName;
        let isChecked = this.isChecked;
        return (h("elsa-property-editor", { key: '421f14d3298f5a43ffec9affcf2780ff9138edb2', activityModel: this.activityModel, propertyDescriptor: propertyDescriptor, propertyModel: propertyModel, onDefaultSyntaxValueChanged: e => this.onDefaultSyntaxValueChanged(e), "single-line": true, showLabel: false }, h("div", { key: 'a94c8cba1513990ddc76f89de1d09729f4c97950', class: "elsa-max-w-lg" }, h("div", { key: '3ae65dc8d55da032e55c6db000dd3ff38227e2b2', class: "elsa-relative elsa-flex elsa-items-start" }, h("div", { key: '47d8fb88d82080c9d7eac0678f38b4e096812144', class: "elsa-flex elsa-items-center elsa-h-5" }, h("input", { key: '91e85fd6d58365199b03ed8c0a07bafdf9478170', id: fieldId, name: fieldName, type: "checkbox", checked: isChecked, value: 'true', onChange: e => this.onCheckChanged(e), class: "focus:elsa-ring-blue-500 elsa-h-4 elsa-w-4 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" })), h("div", { key: 'bb895d8ae53bb395271a7b819bd3b9fd3ac2758b', class: "elsa-ml-3 elsa-text-sm" }, h("label", { key: '779d9284b7e0cb4a39d56cbdbf88a70b4f6aab65', htmlFor: fieldId, class: "elsa-font-medium elsa-text-gray-700" }, fieldLabel))))));
    }
};

export { ElsaCheckBoxProperty as elsa_checkbox_property };
//# sourceMappingURL=elsa-checkbox-property.entry.esm.js.map
