import { r as registerInstance, h, k as Host } from './index-ea213ee1.js';
import { i as initializeMonacoWorker } from './elsa-monaco-utils-62e58fac.js';
import { r as resources } from './localizations-41cde9b2.js';
import { l as loadTranslations } from './i18n-loader-aa6cec69.js';
import { b as propertyDisplayManager } from './index-e19c34cd.js';
import { F as FormContext, t as textInput } from './forms-0aa787e1.js';
import { s as state } from './secret.store-8ddb62d1.js';
import { S as SecretEventTypes } from './secret.events-665f57c3.js';
import { a as state$1 } from './store-52e2ea41.js';
import { S as SyntaxNames } from './index-0f68dbd6.js';
import { c as createHttpClient } from './elsa-client-17ed10a4.js';
import { c as createElsaSecretsClient } from './credential-manager.client-ef17058e.js';
import { e as eventBus } from './event-bus-5d6f3774.js';
import './utils-db96334c.js';
import './collection-ba09a015.js';
import './_commonjsHelpers-6cb8dacb.js';
import './cronstrue-37d55fa1.js';
import './axios-middleware.esm-fcda64d5.js';

let _elsaOauth2Client = null;
const createElsaOauth2Client = async function (serverUrl) {
  if (!!_elsaOauth2Client)
    return _elsaOauth2Client;
  const httpClient = await createHttpClient(serverUrl);
  _elsaOauth2Client = {
    oauth2Api: {
      getUrl: async (secretId) => {
        const response = await httpClient.get(`v1/oauth2/url/${secretId}`);
        return response.data;
      }
    }
  };
  return _elsaOauth2Client;
};

let ElsaSecretEditorModal = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.renderProps = {};
    this.updateCounter = 0; // Used to force update, when a property changes value
    this.timestamp = new Date();
    this.t = (key) => this.i18next.t(key);
    this.onSubmit = async (e) => {
      e.preventDefault();
      const form = e.target;
      const formData = new FormData(form);
      this.updateSecret(formData, this.secretModel);
      const client = await createElsaSecretsClient(this.serverUrl);
      await client.secretsApi.save(this.secretModel);
      await this.hide(true);
      await eventBus.emit(SecretEventTypes.SecretUpdated, this, this.secretModel);
    };
    this.onSecretsLoaded = async (secrets) => {
      if (!this.secretModel) {
        return;
      }
      var secret = secrets.find(x => x.id === this.secretModel.id);
      if (secret) {
        this.secretModel = secret;
      }
    };
    this.onShowSecretEditor = async (secret, animate) => {
      this.secretModel = JSON.parse(JSON.stringify(secret));
      this.secretDescriptor = state.secretsDescriptors.find(x => x.type == secret.type);
      this.formContext = new FormContext(this.secretModel, newValue => this.secretModel = newValue);
      this.timestamp = new Date();
      this.renderProps = {};
      await this.show(animate);
    };
    this.show = async (animate) => await this.dialog.show(animate);
    this.hide = async (animate) => await this.dialog.hide(animate);
    this.onKeyDown = async (event) => {
      if (event.ctrlKey && event.key === 'Enter') {
        this.dialog.querySelector('button[type="submit"]').click();
      }
    };
  }
  connectedCallback() {
    eventBus.on(SecretEventTypes.SecretsEditor.Show, this.onShowSecretEditor);
    eventBus.on(SecretEventTypes.SecretsLoaded, this.onSecretsLoaded);
  }
  disconnectedCallback() {
    eventBus.detach(SecretEventTypes.SecretsEditor.Show, this.onShowSecretEditor);
    eventBus.detach(SecretEventTypes.SecretsLoaded, this.onSecretsLoaded);
  }
  async componentWillLoad() {
    var _a;
    const monacoLibPath = (_a = this.monacoLibPath) !== null && _a !== void 0 ? _a : state$1.monacoLibPath;
    await initializeMonacoWorker(monacoLibPath);
    this.i18next = await loadTranslations(this.culture, resources);
  }
  updateSecret(formData, secret) {
    const secretDescriptor = this.secretDescriptor;
    const inputProperties = secretDescriptor.inputProperties;
    for (const property of inputProperties)
      propertyDisplayManager.update(secret, property, formData);
  }
  get isAuthorizeButtonVisible() {
    var _a;
    if (!this.secretModel) {
      return false;
    }
    const grantType = (_a = this.secretModel.properties.find(x => x.name === 'GrantType')) === null || _a === void 0 ? void 0 : _a.expressions[SyntaxNames.Literal];
    const isTokenMissing = this.secretModel.properties.findIndex(x => x.name === 'Token') === -1;
    return grantType === 'authorization_code' && isTokenMissing;
  }
  get isAuthorizeButtonDisabled() {
    var _a, _b, _c, _d;
    const authUrl = (_a = this.secretModel.properties.find(x => x.name === 'AuthorizationUrl')) === null || _a === void 0 ? void 0 : _a.expressions[SyntaxNames.Literal];
    const tokenUrl = (_b = this.secretModel.properties.find(x => x.name === 'AccessTokenUrl')) === null || _b === void 0 ? void 0 : _b.expressions[SyntaxNames.Literal];
    const clientId = (_c = this.secretModel.properties.find(x => x.name === 'ClientId')) === null || _c === void 0 ? void 0 : _c.expressions[SyntaxNames.Literal];
    const clientSecret = (_d = this.secretModel.properties.find(x => x.name === 'ClientSecret')) === null || _d === void 0 ? void 0 : _d.expressions[SyntaxNames.Literal];
    return !authUrl || !tokenUrl || !clientId || !clientSecret;
  }
  async componentWillRender() {
    const secretDescriptor = this.secretDescriptor || {
      displayName: '',
      type: '',
      outcomes: [],
      category: '',
      browsable: false,
      inputProperties: [],
      description: '',
      customAttributes: {}
    };
    const defaultProperties = secretDescriptor.inputProperties.filter(x => !x.category || x.category.length == 0);
    const secretModel = this.secretModel || {
      type: '',
      id: '',
      name: '',
      properties: [],
    };
    const t = this.t;
    this.renderProps = {
      secretDescriptor: secretDescriptor,
      secretModel,
      defaultProperties,
    };
  }
  async onCancelClick() {
    await this.hide(true);
  }
  async onGetConsentClick(e) {
    e.preventDefault();
    const client = await createElsaSecretsClient(this.serverUrl);
    const secret = await client.secretsApi.save(this.secretModel);
    this.secretModel = secret;
    const oauthClient = await createElsaOauth2Client(this.serverUrl);
    const url = await oauthClient.oauth2Api.getUrl(secret.id);
    window.open(url, '_blank', 'location=yes,height=600,width=600,scrollbars=yes,status=yes');
  }
  render() {
    const renderProps = this.renderProps;
    const secretDescriptor = renderProps.secretDescriptor;
    const secretModel = this.secretModel;
    const t = this.t;
    return (h(Host, { class: "elsa-block" }, h("elsa-modal-dialog", { ref: el => this.dialog = el }, h("div", { slot: "content", class: "elsa-py-8 elsa-pb-0" }, h("form", { onSubmit: e => this.onSubmit(e), key: this.timestamp.getTime().toString(), onKeyDown: this.onKeyDown, class: 'activity-editor-form' }, h("div", { class: "elsa-flex elsa-px-8" }, h("div", { class: "elsa-space-y-8 elsa-divide-y elsa-divide-gray-200 elsa-w-full" }, h("div", null, h("div", null, h("h3", { class: "elsa-text-lg elsa-leading-6 elsa-font-medium elsa-text-gray-900" }, secretDescriptor.type), h("p", { class: "elsa-mt-1 elsa-text-sm elsa-text-gray-500" }, secretDescriptor.description)), h("div", { class: "elsa-mt-8" }, this.renderProperties(secretModel), this.isAuthorizeButtonVisible &&
      h("button", { disabled: this.isAuthorizeButtonDisabled, onClick: e => this.onGetConsentClick(e), class: "elsa-mt-6 elsa-w-full elsa-inline-flex elsa-justify-center elsa-rounded-md elsa-border elsa-border-gray-300 elsa-shadow-sm elsa-px-4 elsa-py-2 elsa-bg-white elsa-text-base elsa-font-medium elsa-text-gray-700 hover:elsa-bg-gray-50 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 sm:elsa-w-auto sm:elsa-text-sm disabled:elsa-opacity-50" }, "Authorize"))))), h("div", { class: "elsa-pt-5" }, h("div", { class: "elsa-bg-gray-50 elsa-px-4 elsa-py-3 sm:elsa-px-6 sm:elsa-flex sm:elsa-flex-row-reverse" }, h("button", { type: "submit", class: "elsa-ml-3 elsa-inline-flex elsa-justify-center elsa-py-2 elsa-px-4 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500" }, 'Save'), h("button", { type: "button", onClick: () => this.onCancelClick(), class: "elsa-w-full elsa-inline-flex elsa-justify-center elsa-rounded-md elsa-border elsa-border-gray-300 elsa-shadow-sm elsa-px-4 elsa-py-2 elsa-bg-white elsa-text-base elsa-font-medium elsa-text-gray-700 hover:elsa-bg-gray-50 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 sm:elsa-mt-0 sm:elsa-ml-3 sm:elsa-w-auto sm:elsa-text-sm" }, 'Cancel'))))), h("div", { slot: "buttons" }))));
  }
  renderProperties(secretModel) {
    const propertyDescriptors = this.renderProps.defaultProperties;
    const formContext = this.formContext;
    if (propertyDescriptors.length == 0)
      return undefined;
    const key = `secret-settings:${secretModel.id}`;
    const t = this.t;
    return (h("div", null, h("div", { class: "elsa-w-full" }, textInput(formContext, 'name', 'Name', secretModel.name, 'Secret\'s name', 'secretName'), textInput(formContext, 'type', 'Type', secretModel.type, 'Secret\'s type', 'secretType', true)), h("div", { class: "elsa-mt-6" }, h("div", { key: key, class: `elsa-grid elsa-grid-cols-1 elsa-gap-y-6 elsa-gap-x-4 sm:elsa-grid-cols-6` }, propertyDescriptors.map(property => this.renderPropertyEditor(secretModel, property))))));
  }
  renderPropertyEditor(secret, property) {
    const key = `secret-property-input:${secret.id}:${property.name}`;
    const display = propertyDisplayManager.display(secret, property);
    const id = `${property.name}Control`;
    return h("elsa-control", { key: key, id: id, class: "sm:elsa-col-span-6", content: display, onChange: () => this.updateCounter++ });
  }
};

export { ElsaSecretEditorModal as elsa_secret_editor_modal };
