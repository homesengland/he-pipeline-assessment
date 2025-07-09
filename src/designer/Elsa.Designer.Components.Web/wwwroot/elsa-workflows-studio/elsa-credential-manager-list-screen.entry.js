import { r as registerInstance, h } from './index-ea213ee1.js';
import './credential-manager-plugin-ce89a6e2.js';
import { T as Tunnel } from './dashboard-c739a7dd.js';
import { c as createElsaSecretsClient } from './credential-manager.client-e6b9a7aa.js';
import { S as SecretEventTypes } from './secret.events-665f57c3.js';
import { e as eventBus } from './event-bus-6625fc04.js';
import './index-82ee3393.js';
import './active-router-8e08ab72.js';
import './index-2db7bf78.js';
import './match-path-760e1797.js';
import './location-utils-fea12957.js';
import './index-0f68dbd6.js';
import './index-c5018c3a.js';
import './elsa-client-ecb85def.js';
import './axios-middleware.esm-fcda64d5.js';
import './collection-ba09a015.js';
import './_commonjsHelpers-6cb8dacb.js';
import './utils-db96334c.js';
import './cronstrue-37d55fa1.js';
import './store-52e2ea41.js';

let CredentialManagerListScreen = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.secrets = [];
    this.listenToMessages = (e) => {
      if (e.origin !== window.origin) {
        return;
      }
      if (e.data == "refreshSecrets") {
        this.loadSecrets();
      }
    };
    this.onSecretPicked = async (args) => {
      const secretDescriptor = args;
      const secretModel = this.newSecret(secretDescriptor);
      await this.showSecretEditorInternal(secretModel, false);
    };
  }
  async componentWillLoad() {
    await this.loadSecrets();
  }
  async connectedCallback() {
    eventBus.on(SecretEventTypes.SecretPicked, this.onSecretPicked);
    eventBus.on(SecretEventTypes.SecretUpdated, () => this.loadSecrets());
    window.addEventListener("message", this.listenToMessages);
  }
  async disconnectedCallback() {
    eventBus.detach(SecretEventTypes.SecretPicked, this.onSecretPicked);
    eventBus.detach(SecretEventTypes.SecretUpdated, () => this.loadSecrets());
    window.removeEventListener("message", this.listenToMessages);
  }
  async onSecretEdit(e, secret) {
    const properties = secret.properties;
    const secretModel = {
      id: secret.id,
      displayName: secret.displayName,
      name: secret.name,
      type: secret.type,
      properties: this.mapProperties(properties)
    };
    await this.showSecretEditorInternal(secretModel, true);
  }
  mapProperties(properties) {
    return properties.map(prop => {
      return {
        expressions: {
          Literal: prop.expressions.Literal
        },
        name: prop.name
      };
    });
  }
  async showSecretEditorInternal(secret, animate) {
    await eventBus.emit(SecretEventTypes.SecretsEditor.Show, this, secret, animate);
  }
  newSecret(secretDescriptor) {
    const secret = {
      type: secretDescriptor.type,
      displayName: secretDescriptor.displayName,
      name: secretDescriptor.displayName,
      properties: [],
    };
    for (const property of secretDescriptor.inputProperties) {
      secret.properties[property.name] = {
        syntax: '',
        expression: '',
      };
    }
    return secret;
  }
  async onDeleteClick(e, webhookDefinition) {
    const result = await this.confirmDialog.show('Delete Secret', 'Are you sure you wish to permanently delete this secret?');
    if (!result)
      return;
    const elsaClient = await createElsaSecretsClient(this.serverUrl);
    await elsaClient.secretsApi.delete(webhookDefinition.id);
    await this.loadSecrets();
  }
  async loadSecrets() {
    const elsaClient = await createElsaSecretsClient(this.serverUrl);
    this.secrets = await elsaClient.secretsApi.list();
    await eventBus.emit(SecretEventTypes.SecretsLoaded, this, this.secrets);
  }
  render() {
    const secrets = this.secrets;
    return (h("div", null, h("div", { class: "elsa-align-middle elsa-inline-block elsa-min-w-full elsa-border-b elsa-border-gray-200" }, h("table", { class: "elsa-min-w-full" }, h("thead", null, h("tr", { class: "elsa-border-t elsa-border-gray-200" }, h("th", { class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, h("span", { class: "lg:elsa-pl-2" }, "Name")), h("th", { class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, "Type"), h("th", { class: "elsa-pr-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-right elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }))), h("tbody", { class: "elsa-bg-white elsa-divide-y elsa-divide-gray-100" }, secrets === null || secrets === void 0 ? void 0 : secrets.map(item => {
      const editIcon = (h("svg", { class: "elsa-h-5 elsa-w-5 elsa-text-gray-500", width: "24", height: "24", viewBox: "0 0 24 24", xmlns: "http://www.w3.org/2000/svg", fill: "none", stroke: "currentColor", "stroke-width": "2", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { d: "M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7" }), h("path", { d: "M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z" })));
      const deleteIcon = (h("svg", { class: "elsa-h-5 elsa-w-5 elsa-text-gray-500", width: "24", height: "24", viewBox: "0 0 24 24", "stroke-width": "2", stroke: "currentColor", fill: "none", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { stroke: "none", d: "M0 0h24v24H0z" }), h("line", { x1: "4", y1: "7", x2: "20", y2: "7" }), h("line", { x1: "10", y1: "11", x2: "10", y2: "17" }), h("line", { x1: "14", y1: "11", x2: "14", y2: "17" }), h("path", { d: "M5 7l1 12a2 2 0 0 0 2 2h8a2 2 0 0 0 2 -2l1 -12" }), h("path", { d: "M9 7v-3a1 1 0 0 1 1 -1h4a1 1 0 0 1 1 1v3" })));
      return (h("tr", null, h("td", { class: "elsa-px-6 elsa-py-3 elsa-whitespace-no-wrap elsa-text-sm elsa-leading-5 elsa-font-medium elsa-text-gray-900" }, h("div", { class: "elsa-flex elsa-items-center elsa-space-x-3 lg:elsa-pl-2" }, item.name)), h("td", { class: "elsa-px-6 elsa-py-3 elsa-text-sm elsa-leading-5 elsa-text-gray-500 elsa-font-medium" }, h("div", { class: "elsa-flex elsa-items-center elsa-space-x-3 lg:elsa-pl-2" }, item.type)), h("td", { class: "elsa-pr-6" }, h("elsa-context-menu", { history: this.history, menuItems: [
          { text: 'Edit', clickHandler: e => this.onSecretEdit(e, item), icon: editIcon },
          { text: 'Delete', clickHandler: e => this.onDeleteClick(e, item), icon: deleteIcon }
        ] }))));
    })))), h("elsa-confirm-dialog", { ref: el => this.confirmDialog = el })));
  }
};
Tunnel.injectProps(CredentialManagerListScreen, ['serverUrl', 'culture', 'basePath']);

export { CredentialManagerListScreen as elsa_credential_manager_list_screen };
