import { r as registerInstance, h, k as Host } from './index-ea213ee1.js';
import { e as eventBus } from './event-bus-6625fc04.js';
import { E as EventTypes } from './index-5e41ee1e.js';
import { c as createElsaWebhooksClient } from './elsa-client-afda30f0.js';
import { F as FormContext, t as textInput, b as textArea, c as checkBox } from './forms-0aa787e1.js';
import { T as Tunnel } from './dashboard-c739a7dd.js';
import './axios-middleware.esm-fcda64d5.js';
import './index-0f68dbd6.js';
import './index-2db7bf78.js';

let ElsaWebhookDefinitionEditorScreen = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
  }
  async getServerUrl() {
    return this.serverUrl;
  }
  async getWebhookId() {
    return this.webhookDefinitionInternal.id;
  }
  async webhookDefinitionChangedHandler(newValue) {
    this.webhookDefinitionInternal = Object.assign({}, newValue);
    this.formContext = new FormContext(this.webhookDefinitionInternal, newValue => this.webhookDefinitionInternal = newValue);
  }
  async webhookIdChangedHandler(newValue) {
    const webhookId = newValue;
    let webhookDefinition = ElsaWebhookDefinitionEditorScreen.createWebhookDefinition();
    webhookDefinition.id = webhookId;
    const client = createElsaWebhooksClient(this.serverUrl);
    if (webhookId && webhookId.length > 0) {
      try {
        webhookDefinition = await client.webhookDefinitionsApi.getByWebhookId(webhookId);
      }
      catch (_a) {
        console.warn(`The specified webhook definition does not exist. Creating a new one.`);
      }
    }
    else {
      webhookDefinition.isEnabled = true;
    }
    this.updateWebhookDefinition(webhookDefinition);
  }
  async serverUrlChangedHandler(newValue) {
  }
  async componentWillLoad() {
    await this.serverUrlChangedHandler(this.serverUrl);
    await this.webhookDefinitionChangedHandler(this.webhookDefinition);
    await this.webhookIdChangedHandler(this.webhookId);
  }
  async saveWebhook() {
    if (!this.serverUrl || this.serverUrl.length == 0)
      return;
    const client = createElsaWebhooksClient(this.serverUrl);
    let webhookDefinition = this.webhookDefinitionInternal;
    const request = {
      id: webhookDefinition.id || this.webhookId,
      name: webhookDefinition.name,
      path: webhookDefinition.path,
      description: webhookDefinition.description,
      payloadTypeName: webhookDefinition.payloadTypeName,
      isEnabled: webhookDefinition.isEnabled,
    };
    this.saving = true;
    try {
      if (request.id == null)
        webhookDefinition = await client.webhookDefinitionsApi.save(request);
      else
        webhookDefinition = await client.webhookDefinitionsApi.update(request);
      this.saving = false;
      this.saved = true;
      setTimeout(() => this.saved = false, 500);
    }
    catch (e) {
      console.error(e);
      this.saving = false;
      this.saved = false;
      this.networkError = e.message;
      setTimeout(() => this.networkError = null, 10000);
    }
  }
  updateWebhookDefinition(value) {
    this.webhookDefinition = value;
  }
  sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
  }
  navigate(path) {
    if (this.history) {
      this.history.push(path);
      return;
    }
    document.location.pathname = path;
  }
  async onSaveClicked(e) {
    e.preventDefault();
    await this.saveWebhook();
    eventBus.emit(EventTypes.WebhookSaved, this, this.webhookDefinitionInternal);
    const anchor = e.currentTarget;
    this.sleep(1000).then(() => {
      this.navigate(`/webhook-definitions`);
    });
  }
  render() {
    return (h(Host, { class: "elsa-flex elsa-flex-col elsa-w-full", ref: el => this.el = el }, h("form", { onSubmit: e => this.onSaveClicked(e) }, h("div", { class: "elsa-px-8 mb-8" }, h("div", { class: "elsa-border-b elsa-border-gray-200" })), this.renderWebhookFields(), this.renderCanvas())));
  }
  renderWebhookFields() {
    const webhookDefinition = this.webhookDefinitionInternal;
    const formContext = this.formContext;
    return (h("main", { class: "elsa-max-w-7xl elsa-mx-auto elsa-pb-10 lg:elsa-py-12 lg:elsa-px-8" }, h("div", { class: "lg:elsa-grid lg:elsa-grid-cols-12 lg:elsa-gap-x-5" }, h("aside", { class: "elsa-py-6 elsa-px-2 sm:elsa-px-6 lg:elsa-py-0 lg:elsa-px-0 lg:elsa-col-span-2" }), h("div", { class: "elsa-space-y-6 sm:elsa-px-6 lg:elsa-px-0 lg:elsa-col-span-9" }, h("section", { "aria-labelledby": "payment_details_heading" }, h("div", { class: "elsa-shadow sm:elsa-rounded-md sm:elsa-overflow-hidden" }, h("div", { class: "elsa-bg-white elsa-py-6 elsa-px-4 sm:elsa-p-6" }, h("div", null, h("h1", { class: "elsa-text-lg elsa-leading-6 elsa-font-medium elsa-text-gray-900" }, null == webhookDefinition.id ? "Create Webhook Definition" : "Edit Webhook Definition")), h("div", { class: "elsa-mt-6 elsa-grid elsa-grid-cols-4 elsa-gap-6" }, h("div", { class: "elsa-col-span-4" }, textInput(formContext, 'name', 'Name', webhookDefinition.name, 'The name of the webhook.', 'webhookName')), h("div", { class: "elsa-col-span-4" }, textInput(formContext, 'path', 'Path', webhookDefinition.path, 'The path of the webhook.', 'webhookPath')), h("div", { class: "elsa-col-span-4" }, textArea(formContext, 'description', 'Description', webhookDefinition.description, null, 'webhookDescription')), h("div", { class: "elsa-col-span-4" }, textInput(formContext, 'payloadTypeName', 'Payload Type Name', webhookDefinition.payloadTypeName, 'The payload type name of the webhook.', 'webhookPayloadTypeName')), h("div", { class: "elsa-col-span-4" }, checkBox(formContext, 'isEnabled', 'Enabled', webhookDefinition.isEnabled, null)))), h("div", { class: "elsa-px-4 elsa-py-3 elsa-bg-gray-50 elsa-text-right sm:px-6" }, h("button", { type: "submit", class: "elsa-ml-0 elsa-w-full elsa-inline-flex elsa-justify-center elsa-rounded-md elsa-border elsa-border-transparent elsa-shadow-sm elsa-px-4 elsa-py-2 elsa-bg-blue-600 elsa-text-base elsa-font-medium elsa-text-white hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 sm:elsa-ml-3 sm:elsa-w-auto sm:elsa-text-sm" }, "Save"))))))));
  }
  renderCanvas() {
    return (h("div", { class: "elsa-flex-1 elsa-flex elsa-relative" }, h("elsa-webhook-definition-editor-notifications", null), h("div", { class: "elsa-fixed elsa-bottom-10 elsa-right-12" }, h("div", { class: "elsa-flex elsa-items-center elsa-space-x-4" }, this.renderSavingIndicator(), this.renderNetworkError()))));
  }
  renderSavingIndicator() {
    const message = this.saving ? 'Saving...' : this.saved ? 'Saved'
      : null;
    if (!message)
      return undefined;
    return (h("div", null, h("span", { class: "elsa-text-gray-400 elsa-text-sm" }, message)));
  }
  renderNetworkError() {
    if (!this.networkError)
      return undefined;
    return (h("div", null, h("span", { class: "elsa-text-rose-400 elsa-text-sm" }, "An error occurred: ", this.networkError)));
  }
  static createWebhookDefinition() {
    return {
      id: null,
    };
  }
  static get watchers() { return {
    "webhookDefinition": ["webhookDefinitionChangedHandler"],
    "webhookId": ["webhookIdChangedHandler"],
    "serverUrl": ["serverUrlChangedHandler"]
  }; }
};
Tunnel.injectProps(ElsaWebhookDefinitionEditorScreen, ['serverUrl', 'culture']);

export { ElsaWebhookDefinitionEditorScreen as elsa_webhook_definition_editor_screen };
