import { r as registerInstance, h, j as Host } from './index-CL6j2ec2.js';
import { i as initializeMonacoWorker } from './elsa-monaco-utils-BNFq3u5d.js';
import { r as resources } from './localizations-uya2Gbnn.js';
import { l as loadTranslations } from './i18n-loader-DJQycacf.js';
import { b as propertyDisplayManager } from './index-fZDMH_YE.js';
import { F as FormContext, t as textInput } from './forms-CaCATBuQ.js';
import { s as state } from './secret.store-Bx5ZyYIY.js';
import { S as SecretEventTypes } from './secret.events-CkUpytGo.js';
import { a as state$1 } from './store-B_H_ZDGs.js';
import { S as SyntaxNames } from './index-D7wXd6HU.js';
import { c as createHttpClient } from './elsa-client-q6ob5JPZ.js';
import { c as createElsaSecretsClient } from './credential-manager.client-DYzSPspV.js';
import { e as eventBus } from './event-bus-axQqcjdg.js';
import './events-CpKc8CLe.js';
import './utils-C0M_5Llz.js';
import './collection-B4sYCr2r.js';
import './_commonjsHelpers-Cf5sKic0.js';
import './cronstrue-BvVNjwUa.js';
import './fetch-client-1OcjQcrw.js';

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

const ElsaSecretEditorModal = class {
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
        return (h(Host, { key: '394baa1f18b800bdf4f500fbf1e42e62e50391b4', class: "elsa-block" }, h("elsa-modal-dialog", { key: 'c07c41c15062cfea79196be0b39a1fb0ae5f4833', ref: el => this.dialog = el }, h("div", { key: 'd43ee9e0381be2ee11324fd6fd164b348c1e5ce4', slot: "content", class: "elsa-py-8 elsa-pb-0" }, h("form", { onSubmit: e => this.onSubmit(e), key: this.timestamp.getTime().toString(), onKeyDown: this.onKeyDown, class: 'activity-editor-form' }, h("div", { key: 'e52971459090095e731e5ed137d41224fe685fd6', class: "elsa-flex elsa-px-8" }, h("div", { key: '0b4c8f235cbe99551e543844fded0bbde0655b2d', class: "elsa-space-y-8 elsa-divide-y elsa-divide-gray-200 elsa-w-full" }, h("div", { key: '02511183f47a0c3b337f4b9c2cb92533fe0132ee' }, h("div", { key: '12cfc29ff0d53a068668b00bd25989771354d258' }, h("h3", { key: '8ae7dd491dabd2f66841b2729ceb0bf066c8ffc0', class: "elsa-text-lg elsa-leading-6 elsa-font-medium elsa-text-gray-900" }, secretDescriptor.type), h("p", { key: 'f36dbcd7564084f9ec681fe64bc68ab71ced9331', class: "elsa-mt-1 elsa-text-sm elsa-text-gray-500" }, secretDescriptor.description)), h("div", { key: 'c7bb8597897fcd153200d3499911fad2e7733086', class: "elsa-mt-8" }, this.renderProperties(secretModel), this.isAuthorizeButtonVisible &&
            h("button", { key: '0af84a3d06c028a6c22e346c960827fd6120465b', disabled: this.isAuthorizeButtonDisabled, onClick: e => this.onGetConsentClick(e), class: "elsa-mt-6 elsa-w-full elsa-inline-flex elsa-justify-center elsa-rounded-md elsa-border elsa-border-gray-300 elsa-shadow-sm elsa-px-4 elsa-py-2 elsa-bg-white elsa-text-base elsa-font-medium elsa-text-gray-700 hover:elsa-bg-gray-50 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 sm:elsa-w-auto sm:elsa-text-sm disabled:elsa-opacity-50" }, "Authorize"))))), h("div", { key: '77363f733a3aabf4c878f4e5060d85f2023db25f', class: "elsa-pt-5" }, h("div", { key: 'c63bd0f14628626daf669696b4f782d6126f013e', class: "elsa-bg-gray-50 elsa-px-4 elsa-py-3 sm:elsa-px-6 sm:elsa-flex sm:elsa-flex-row-reverse" }, h("button", { key: '4fdd0efab747b143fd8c5c5df23f59c5b4f4fcbc', type: "submit", class: "elsa-ml-3 elsa-inline-flex elsa-justify-center elsa-py-2 elsa-px-4 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500" }, 'Save'), h("button", { key: 'a786dfa60e7ccc2e5e257fc444fd49c9bcdfb154', type: "button", onClick: () => this.onCancelClick(), class: "elsa-w-full elsa-inline-flex elsa-justify-center elsa-rounded-md elsa-border elsa-border-gray-300 elsa-shadow-sm elsa-px-4 elsa-py-2 elsa-bg-white elsa-text-base elsa-font-medium elsa-text-gray-700 hover:elsa-bg-gray-50 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 sm:elsa-mt-0 sm:elsa-ml-3 sm:elsa-w-auto sm:elsa-text-sm" }, 'Cancel'))))), h("div", { key: 'd695fd281d10f618b746872228dd0ee24618d19a', slot: "buttons" }))));
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
//# sourceMappingURL=elsa-secret-editor-modal.entry.esm.js.map
