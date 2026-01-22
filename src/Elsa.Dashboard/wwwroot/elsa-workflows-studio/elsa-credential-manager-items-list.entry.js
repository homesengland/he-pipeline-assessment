import { r as registerInstance, h } from './index-1542df5c.js';
import './index-842ad20c.js';
import { r as resources } from './localizations-41cde9b2.js';
import { l as loadTranslations } from './i18n-loader-aa6cec69.js';
import { T as Tunnel } from './dashboard-beb9b1e8.js';
import './index-892f713d.js';
import { S as SecretEventTypes } from './secret.events-665f57c3.js';
import { a as state } from './store-8fc2fe8a.js';
import { e as eventBus } from './event-bus-5d6f3774.js';
import './index-2db7bf78.js';
import './elsa-client-8304c78c.js';
import './fetch-client-f0dc2a52.js';
import './events-d0aab14a.js';
import './collection-ba09a015.js';
import './_commonjsHelpers-6cb8dacb.js';
import './index-1654a48d.js';
import './utils-db96334c.js';
import './cronstrue-37d55fa1.js';
import './index-0d4e8807.js';

const CredentialManagerItemsList = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.monacoLibPath = undefined;
    this.culture = undefined;
    this.basePath = undefined;
    this.serverUrl = undefined;
  }
  async componentWillLoad() {
    this.i18next = await loadTranslations(this.culture, resources);
  }
  async onNewClick() {
    await eventBus.emit(SecretEventTypes.ShowSecretsPicker);
  }
  renderListScreen() {
    return h("elsa-credential-manager-list-screen", null);
  }
  renderSecretPickerModal() {
    return h("elsa-secrets-picker-modal", null);
  }
  renderSecretPickerEditor() {
    var _a;
    const monacoLibPath = (_a = this.monacoLibPath) !== null && _a !== void 0 ? _a : state.monacoLibPath;
    return h("elsa-secret-editor-modal", { culture: this.culture, "monaco-lib-path": monacoLibPath, serverUrl: this.serverUrl });
  }
  render() {
    return (h("div", null, h("div", { class: "elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white" }, h("div", { class: "elsa-flex-1 elsa-min-w-0" }, h("h1", { class: "elsa-text-lg elsa-font-medium elsa-leading-6 elsa-text-gray-900 sm:elsa-truncate" })), h("button", { type: "button", onClick: () => this.onNewClick(), class: "elsa-mt-3 elsa-w-full elsa-inline-flex elsa-justify-center elsa-rounded-md elsa-border elsa-border-gray-300 elsa-shadow-sm elsa-px-4 elsa-py-2 elsa-bg-white elsa-text-base elsa-font-medium elsa-text-gray-700 hover:elsa-bg-gray-50 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 sm:elsa-mt-0 sm:elsa-ml-3 sm:elsa-w-auto sm:elsa-text-sm" }, "Add New")), this.renderListScreen(), this.renderSecretPickerModal(), this.renderSecretPickerEditor()));
  }
};
Tunnel.injectProps(CredentialManagerItemsList, ['culture', 'basePath', 'serverUrl', 'monacoLibPath']);

export { CredentialManagerItemsList as elsa_credential_manager_items_list };
