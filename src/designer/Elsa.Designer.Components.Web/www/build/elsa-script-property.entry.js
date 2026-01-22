import { r as registerInstance, h } from './index-ea213ee1.js';
import { E as EventTypes } from './index-0f68dbd6.js';
import './index-e19c34cd.js';
import { T as Tunnel } from './workflow-editor-3a84ee12.js';
import { b as createElsaClient } from './elsa-client-17ed10a4.js';
import { e as eventBus } from './event-bus-5d6f3774.js';
import './utils-db96334c.js';
import './collection-ba09a015.js';
import './_commonjsHelpers-6cb8dacb.js';
import './cronstrue-37d55fa1.js';
import './index-2db7bf78.js';
import './axios-middleware.esm-fcda64d5.js';

let ElsaScriptProperty = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.editorHeight = '6em';
    this.singleLineMode = false;
    this.activityValidatingContext = null;
    this.configureComponentCustomButtonContext = null;
  }
  async componentWillLoad() {
    this.currentValue = this.propertyModel.expressions['Literal'];
    await this.configureComponentCustomButton();
    this.validate(this.currentValue);
  }
  async componentDidLoad() {
    const elsaClient = await createElsaClient(this.serverUrl);
    const context = {
      propertyName: this.propertyDescriptor.name,
      activityTypeName: this.activityModel.type
    };
    const libSource = await elsaClient.scriptingApi.getJavaScriptTypeDefinitions(this.workflowDefinitionId, context);
    const libUri = 'defaultLib:lib.es6.d.ts';
    await this.monacoEditor.addJavaScriptLib(libSource, libUri);
  }
  async configureComponentCustomButton() {
    this.configureComponentCustomButtonContext = {
      component: 'elsa-script-property',
      activityType: this.activityModel.type,
      prop: this.propertyDescriptor.name,
      data: null
    };
    await eventBus.emit(EventTypes.ComponentLoadingCustomButton, this, this.configureComponentCustomButtonContext);
  }
  mapSyntaxToLanguage(syntax) {
    switch (syntax) {
      case 'JavaScript':
        return 'javascript';
      case 'Liquid':
        return 'handlebars';
      case 'Literal':
      default:
        return 'plaintext';
    }
  }
  onComponentCustomButtonClick(e) {
    e.preventDefault();
    const componentCustomButtonClickContext = {
      component: 'elsa-script-property',
      activityType: this.activityModel.type,
      prop: this.propertyDescriptor.name,
      params: null
    };
    eventBus.emit(EventTypes.ComponentCustomButtonClick, this, componentCustomButtonClickContext);
  }
  onMonacoValueChanged(e) {
    this.currentValue = e.value;
    this.validate(this.currentValue);
  }
  validate(value) {
    this.activityValidatingContext = {
      activityType: this.activityModel.type,
      prop: this.propertyDescriptor.name,
      value: value,
      data: null,
      isValidated: false,
      isValid: false
    };
    eventBus.emit(EventTypes.ActivityPluginValidating, this, this.activityValidatingContext);
  }
  render() {
    const propertyDescriptor = this.propertyDescriptor;
    const syntax = this.syntax;
    const monacoLanguage = this.mapSyntaxToLanguage(syntax);
    const propertyName = propertyDescriptor.name;
    const fieldId = propertyName;
    const fieldName = propertyName;
    const fieldLabel = propertyDescriptor.label || propertyName;
    const fieldHint = propertyDescriptor.hint;
    const value = this.currentValue;
    const renderValidationResult = () => {
      if (this.activityValidatingContext == null || !this.activityValidatingContext.isValidated)
        return;
      const isPositiveResult = this.activityValidatingContext.isValid;
      const color = isPositiveResult ? 'green' : 'red';
      return (h("div", { class: "elsa-mt-3" }, h("p", { class: `elsa-mt-1 elsa-text-sm elsa-text-${color}-500` }, this.activityValidatingContext.data)));
    };
    const renderComponentCustomButton = () => {
      if (this.configureComponentCustomButtonContext.data == null)
        return;
      const label = this.configureComponentCustomButtonContext.data.label;
      return (h("div", { class: "elsa-mt-3" }, h("a", { href: "#", onClick: e => this.onComponentCustomButtonClick(e), class: "elsa-relative elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-gray-300 elsa-text-sm elsa-leading-5 elsa-font-medium elsa-rounded-md elsa-text-gray-700 elsa-bg-white hover:elsa-text-gray-500 focus:elsa-outline-none focus:elsa-shadow-outline-blue focus:elsa-border-blue-300 active:elsa-bg-gray-100 active:elsa-text-gray-700 elsa-transition elsa-ease-in-out elsa-duration-150" }, label)));
    };
    return h("div", null, h("div", { class: "elsa-flex" }, h("div", { class: "" }, h("label", { htmlFor: fieldId, class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700" }, fieldLabel)), h("div", { class: "elsa-flex-1" }, h("div", null, h("div", { class: "hidden sm:elsa-block" }, h("nav", { class: "elsa-flex elsa-flex-row-reverse", "aria-label": "Tabs" }, h("span", { class: "elsa-bg-blue-100 elsa-text-blue-700 elsa-px-3 elsa-py-2 elsa-font-medium elsa-text-sm elsa-rounded-md" }, syntax)))))), h("div", { class: "elsa-mt-1" }, h("elsa-monaco", { value: value, language: monacoLanguage, "editor-height": this.editorHeight, "single-line": this.singleLineMode, onValueChanged: e => this.onMonacoValueChanged(e.detail), ref: el => this.monacoEditor = el })), fieldHint ? h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, fieldHint) : undefined, h("input", { type: "hidden", name: fieldName, value: value }), renderValidationResult(), renderComponentCustomButton());
  }
};
Tunnel.injectProps(ElsaScriptProperty, ['serverUrl', 'workflowDefinitionId']);

export { ElsaScriptProperty as elsa_script_property };
