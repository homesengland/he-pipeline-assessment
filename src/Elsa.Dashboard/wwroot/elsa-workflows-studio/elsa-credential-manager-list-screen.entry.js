import { r as registerInstance, h } from './index-CL6j2ec2.js';
import './credential-manager-plugin-CzGEoBmR.js';
import { T as Tunnel } from './dashboard-DaK-DIvX.js';
import { c as createElsaSecretsClient } from './credential-manager.client-DYzSPspV.js';
import { S as SecretEventTypes } from './secret.events-CkUpytGo.js';
import { e as eventBus } from './event-bus-axQqcjdg.js';
import './index-D7wXd6HU.js';
import './events-CpKc8CLe.js';
import './index-fZDMH_YE.js';
import './elsa-client-q6ob5JPZ.js';
import './fetch-client-1OcjQcrw.js';
import './collection-B4sYCr2r.js';
import './_commonjsHelpers-Cf5sKic0.js';
import './utils-C0M_5Llz.js';
import './cronstrue-BvVNjwUa.js';
import './store-B_H_ZDGs.js';
import './index-C-8L13GY.js';

const CredentialManagerListScreen = class {
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
        return (h("div", { key: 'd0a3f7f61d7666b1176a553981ae7f42666552d0' }, h("div", { key: '76c2a39b175dbce7400e5d34154597cb85a469da', class: "elsa-align-middle elsa-inline-block elsa-min-w-full elsa-border-b elsa-border-gray-200" }, h("table", { key: 'a4d77e0ae3e3a0e05d92e8d86b3f1f294ba7d206', class: "elsa-min-w-full" }, h("thead", { key: '4e0fdd660a959a4b04d4d07a7b7dcfe2dff84102' }, h("tr", { key: '9e97ac5b85932aa82ddbf9d313fe2eeff094b296', class: "elsa-border-t elsa-border-gray-200" }, h("th", { key: '004703005dcfac9866d2d2a7f4ee895fc1f61f25', class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, h("span", { key: '8489228b4a187b78a1eb786378c501bd8ac4b253', class: "lg:elsa-pl-2" }, "Name")), h("th", { key: 'f4f75ceb74a93ace2e5881d23a3eebff8704fe61', class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, "Type"), h("th", { key: 'd51b9a6f5cfb6a048364769d3550dff9c37cbc6b', class: "elsa-pr-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-right elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }))), h("tbody", { key: 'f8d4803c8811c0bbdde70087ae39b1c191c079a9', class: "elsa-bg-white elsa-divide-y elsa-divide-gray-100" }, secrets === null || secrets === void 0 ? void 0 : secrets.map(item => {
            const editIcon = (h("svg", { class: "elsa-h-5 elsa-w-5 elsa-text-gray-500", width: "24", height: "24", viewBox: "0 0 24 24", xmlns: "http://www.w3.org/2000/svg", fill: "none", stroke: "currentColor", "stroke-width": "2", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { d: "M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7" }), h("path", { d: "M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z" })));
            const deleteIcon = (h("svg", { class: "elsa-h-5 elsa-w-5 elsa-text-gray-500", width: "24", height: "24", viewBox: "0 0 24 24", "stroke-width": "2", stroke: "currentColor", fill: "none", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { stroke: "none", d: "M0 0h24v24H0z" }), h("line", { x1: "4", y1: "7", x2: "20", y2: "7" }), h("line", { x1: "10", y1: "11", x2: "10", y2: "17" }), h("line", { x1: "14", y1: "11", x2: "14", y2: "17" }), h("path", { d: "M5 7l1 12a2 2 0 0 0 2 2h8a2 2 0 0 0 2 -2l1 -12" }), h("path", { d: "M9 7v-3a1 1 0 0 1 1 -1h4a1 1 0 0 1 1 1v3" })));
            return (h("tr", null, h("td", { class: "elsa-px-6 elsa-py-3 elsa-whitespace-no-wrap elsa-text-sm elsa-leading-5 elsa-font-medium elsa-text-gray-900" }, h("div", { class: "elsa-flex elsa-items-center elsa-space-x-3 lg:elsa-pl-2" }, item.name)), h("td", { class: "elsa-px-6 elsa-py-3 elsa-text-sm elsa-leading-5 elsa-text-gray-500 elsa-font-medium" }, h("div", { class: "elsa-flex elsa-items-center elsa-space-x-3 lg:elsa-pl-2" }, item.type)), h("td", { class: "elsa-pr-6" }, h("elsa-context-menu", { history: this.history, menuItems: [
                    { text: 'Edit', clickHandler: e => this.onSecretEdit(e, item), icon: editIcon },
                    { text: 'Delete', clickHandler: e => this.onDeleteClick(e, item), icon: deleteIcon }
                ] }))));
        })))), h("elsa-confirm-dialog", { key: '3da6b9a800d42c4d1bb0fd77b06b5fa7298b28d5', ref: el => this.confirmDialog = el })));
    }
};
Tunnel.injectProps(CredentialManagerListScreen, ['serverUrl', 'culture', 'basePath']);

export { CredentialManagerListScreen as elsa_credential_manager_list_screen };
//# sourceMappingURL=elsa-credential-manager-list-screen.entry.esm.js.map
