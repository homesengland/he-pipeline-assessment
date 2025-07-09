import { r as registerInstance, h } from './index-ea213ee1.js';
import { S as SyntaxNames } from './index-0f68dbd6.js';
import { T as Tunnel } from './workflow-editor-3a84ee12.js';
import { g as getSelectListItems } from './select-list-items-37e28b87.js';
import { a as awaitElement } from './utils-db96334c.js';
import './index-2db7bf78.js';
import './index-c5018c3a.js';
import './elsa-client-ecb85def.js';
import './axios-middleware.esm-fcda64d5.js';
import './collection-ba09a015.js';
import './_commonjsHelpers-6cb8dacb.js';
import './event-bus-6625fc04.js';
import './cronstrue-37d55fa1.js';

let ElsaDropdownProperty = class {
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
    return (h("elsa-property-editor", { activityModel: this.activityModel, propertyDescriptor: propertyDescriptor, propertyModel: propertyModel, onDefaultSyntaxValueChanged: e => this.onDefaultSyntaxValueChanged(e), "single-line": true }, h("select", { id: fieldId, name: fieldName, onChange: e => this.onChange(e), class: "elsa-mt-1 elsa-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md" }, items.map(item => {
      const optionIsObject = typeof (item) == 'object';
      const value = optionIsObject ? item.value : item.toString();
      const text = optionIsObject ? item.text : item.toString();
      return h("option", { value: value, selected: value === currentValue }, text);
    }))));
  }
};
Tunnel.injectProps(ElsaDropdownProperty, ['serverUrl']);

export { ElsaDropdownProperty as elsa_dropdown_property };
