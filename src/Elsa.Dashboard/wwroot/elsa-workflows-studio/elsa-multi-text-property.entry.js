import { r as registerInstance, a as createEvent, h } from './index-CL6j2ec2.js';
import { S as SyntaxNames } from './index-D7wXd6HU.js';
import { p as parseJson } from './utils-C0M_5Llz.js';
import { T as Tunnel } from './workflow-editor-pBAZ9Py8.js';
import { g as getSelectListItems } from './select-list-items-qT1HJ7dW.js';
import './events-CpKc8CLe.js';
import './collection-B4sYCr2r.js';
import './_commonjsHelpers-Cf5sKic0.js';
import './index-C-8L13GY.js';
import './index-fZDMH_YE.js';
import './elsa-client-q6ob5JPZ.js';
import './fetch-client-1OcjQcrw.js';
import './event-bus-axQqcjdg.js';
import './cronstrue-BvVNjwUa.js';

const ElsaMultiTextProperty = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
        this.valueChange = createEvent(this, "valueChange", 7);
        this.selectList = { items: [], isFlagsEnum: false };
    }
    async componentWillLoad() {
        this.currentValue = this.propertyModel.expressions[SyntaxNames.Json] || '[]';
    }
    onValueChanged(newValue) {
        const newValues = newValue.map(item => {
            if (typeof item === 'string')
                return item;
            if (typeof item === 'number')
                return item.toString();
            if (typeof item === 'boolean')
                return item.toString();
            return item.value;
        });
        this.valueChange.emit(newValue);
        this.currentValue = JSON.stringify(newValues);
        this.propertyModel.expressions[SyntaxNames.Json] = this.currentValue;
    }
    onDefaultSyntaxValueChanged(e) {
        this.currentValue = e.detail;
    }
    createKeyValueOptions(options) {
        if (options === null)
            return options;
        return options.map(option => typeof option === 'string' ? { text: option, value: option } : option);
    }
    async componentWillRender() {
        this.selectList = await getSelectListItems(this.serverUrl, this.propertyDescriptor);
    }
    render() {
        const propertyDescriptor = this.propertyDescriptor;
        const propertyModel = this.propertyModel;
        const propertyName = propertyDescriptor.name;
        const fieldId = propertyName;
        const fieldName = propertyName;
        const values = parseJson(this.currentValue);
        const items = this.selectList.items;
        const useDropdown = !!propertyDescriptor.options && Array.isArray(propertyDescriptor.options) && propertyDescriptor.options.length > 0;
        const propertyOptions = this.createKeyValueOptions(items);
        const elsaInputTags = useDropdown ?
            h("elsa-input-tags-dropdown", { dropdownValues: propertyOptions, values: values, fieldId: fieldId, fieldName: fieldName, onValueChanged: e => this.onValueChanged(e.detail) }) :
            h("elsa-input-tags", { values: values, fieldId: fieldId, fieldName: fieldName, onValueChanged: e => this.onValueChanged(e.detail) });
        return (h("elsa-property-editor", { key: '9396f94a97a04862f1cf5c647c1994f8467c2698', activityModel: this.activityModel, propertyDescriptor: propertyDescriptor, propertyModel: propertyModel, onDefaultSyntaxValueChanged: e => this.onDefaultSyntaxValueChanged(e), "single-line": true }, elsaInputTags));
    }
};
Tunnel.injectProps(ElsaMultiTextProperty, ['serverUrl']);

export { ElsaMultiTextProperty as elsa_multi_text_property };
//# sourceMappingURL=elsa-multi-text-property.entry.esm.js.map
