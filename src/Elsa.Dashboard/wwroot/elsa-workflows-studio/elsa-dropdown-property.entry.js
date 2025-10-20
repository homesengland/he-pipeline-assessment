import { r as registerInstance, h } from './index-CL6j2ec2.js';
import { S as SyntaxNames } from './index-D7wXd6HU.js';
import { T as Tunnel } from './workflow-editor-pBAZ9Py8.js';
import { g as getSelectListItems } from './select-list-items-qT1HJ7dW.js';
import { a as awaitElement } from './utils-C0M_5Llz.js';
import './events-CpKc8CLe.js';
import './index-C-8L13GY.js';
import './index-fZDMH_YE.js';
import './elsa-client-q6ob5JPZ.js';
import './fetch-client-1OcjQcrw.js';
import './event-bus-axQqcjdg.js';
import './collection-B4sYCr2r.js';
import './_commonjsHelpers-Cf5sKic0.js';
import './cronstrue-BvVNjwUa.js';

const ElsaDropdownProperty = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
        this.selectList = { items: [], isFlagsEnum: false };
        this.reloadSelectListFromDeps = async (e) => {
            const depValues = {};
            const options = this.propertyDescriptor.options;
            for (const dependencyPropName of options.context.dependsOnValue) {
                const value = this.activityModel.properties.find((prop) => {
                    return prop.name == dependencyPropName;
                });
                depValues[dependencyPropName] = value.expressions["Literal"];
            }
            // Need to get the latest value of the component that just changed.
            // For this we need to get the value from the event.
            const currentTarget = e.currentTarget;
            depValues[currentTarget.id] = currentTarget.value;
            let newOptions = {
                context: Object.assign(Object.assign({}, options.context), { depValues: depValues }),
                runtimeSelectListProviderType: options.runtimeSelectListProviderType
            };
            this.selectList = await getSelectListItems(this.serverUrl, { options: newOptions });
            let currentSelectList = await awaitElement('#' + this.propertyDescriptor.name);
            const defaultValue = this.propertyDescriptor.defaultValue;
            if (defaultValue) {
                this.currentValue = defaultValue;
            }
            else {
                const firstOption = this.selectList.items[0];
                if (firstOption) {
                    const optionIsObject = typeof (firstOption) == 'object';
                    this.currentValue = optionIsObject ? firstOption.value : firstOption.toString();
                }
            }
            // Dispatch change event so that dependent dropdown elements refresh.
            // Do this after the current component has re-rendered, otherwise the current value will be sent to the backend, which is outdated.
            requestAnimationFrame(() => {
                currentSelectList.dispatchEvent(new Event("change"));
            });
        };
    }
    async componentWillLoad() {
        var _a, _b;
        const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
        this.currentValue = this.propertyModel.expressions[defaultSyntax] || undefined;
        const dependsOnEvent = this.propertyDescriptor.options && "context" in this.propertyDescriptor.options ? (_b = (_a = this.propertyDescriptor.options) === null || _a === void 0 ? void 0 : _a.context) === null || _b === void 0 ? void 0 : _b.dependsOnEvent : undefined;
        // Does this property have a dependency on another property?
        if (!!dependsOnEvent) {
            // Collect current property values of the activity.
            const initialDepsValue = {};
            for (const event of dependsOnEvent) {
                for (const value of this.activityModel.properties) {
                    initialDepsValue[value.name] = value.expressions['Literal'];
                }
                // Listen for change events on the dependency dropdown list.
                const dependentInputElement = await awaitElement('#' + event);
                dependentInputElement.addEventListener('change', this.reloadSelectListFromDeps);
                // Get the current value of the dependency dropdown list.
                initialDepsValue[event] = dependentInputElement.value;
            }
            const existingOptions = this.propertyDescriptor.options;
            // Load the list items from the backend.
            const options = {
                context: Object.assign(Object.assign({}, existingOptions.context), { depValues: initialDepsValue }),
                runtimeSelectListProviderType: existingOptions.runtimeSelectListProviderType
            };
            this.selectList = await getSelectListItems(this.serverUrl, { options: options });
            if (this.currentValue == undefined) {
                const defaultValue = this.propertyDescriptor.defaultValue;
                if (defaultValue) {
                    this.currentValue = defaultValue;
                }
                else {
                    const firstOption = this.selectList.items[0];
                    if (firstOption) {
                        const optionIsObject = typeof (firstOption) == 'object';
                        this.currentValue = optionIsObject ? firstOption.value : firstOption.toString();
                    }
                }
            }
        }
        else {
            this.selectList = await getSelectListItems(this.serverUrl, this.propertyDescriptor);
            if (this.currentValue == undefined) {
                const defaultValue = this.propertyDescriptor.defaultValue;
                if (defaultValue) {
                    this.currentValue = defaultValue;
                }
                else {
                    const firstOption = this.selectList.items[0];
                    if (firstOption) {
                        const optionIsObject = typeof (firstOption) == 'object';
                        this.currentValue = optionIsObject ? firstOption.value : firstOption.toString();
                    }
                }
            }
        }
        if (this.currentValue != undefined) {
            this.propertyModel.expressions[defaultSyntax] = this.currentValue;
        }
    }
    onChange(e) {
        const select = e.target;
        const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
        this.propertyModel.expressions[defaultSyntax] = this.currentValue = select.value;
    }
    onDefaultSyntaxValueChanged(e) {
        this.currentValue = e.detail;
    }
    render() {
        const propertyDescriptor = this.propertyDescriptor;
        const propertyModel = this.propertyModel;
        const propertyName = propertyDescriptor.name;
        const fieldId = propertyName;
        const fieldName = propertyName;
        let currentValue = this.currentValue;
        const { items } = this.selectList;
        if (currentValue == undefined) {
            const defaultValue = this.propertyDescriptor.defaultValue;
            currentValue = defaultValue ? defaultValue.toString() : undefined;
        }
        return (h("elsa-property-editor", { key: 'dbb879ce5c07e5483a57070b6946a36368d27696', activityModel: this.activityModel, propertyDescriptor: propertyDescriptor, propertyModel: propertyModel, onDefaultSyntaxValueChanged: e => this.onDefaultSyntaxValueChanged(e), "single-line": true }, h("select", { key: '6f61d37b83d49a12384f4094989cd61a62a8d593', id: fieldId, name: fieldName, onChange: e => this.onChange(e), class: "elsa-mt-1 elsa-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md" }, items.map(item => {
            const optionIsObject = typeof (item) == 'object';
            const value = optionIsObject ? item.value : item.toString();
            const text = optionIsObject ? item.text : item.toString();
            return h("option", { value: value, selected: value === currentValue }, text);
        }))));
    }
};
Tunnel.injectProps(ElsaDropdownProperty, ['serverUrl']);

export { ElsaDropdownProperty as elsa_dropdown_property };
//# sourceMappingURL=elsa-dropdown-property.entry.esm.js.map
