import { r as registerInstance, h } from './index-CL6j2ec2.js';
import './index-CBYiEsWN.js';
import { r as resources } from './localizations-uya2Gbnn.js';
import { l as loadTranslations } from './i18n-loader-DJQycacf.js';
import { T as Tunnel } from './dashboard-DaK-DIvX.js';
import './index-fZDMH_YE.js';
import { S as SecretEventTypes } from './secret.events-CkUpytGo.js';
import { a as state } from './store-B_H_ZDGs.js';
import { e as eventBus } from './event-bus-axQqcjdg.js';
import './_commonjsHelpers-Cf5sKic0.js';
import './index-C-8L13GY.js';
import './elsa-client-q6ob5JPZ.js';
import './fetch-client-1OcjQcrw.js';
import './events-CpKc8CLe.js';
import './collection-B4sYCr2r.js';
import './index-D7wXd6HU.js';
import './utils-C0M_5Llz.js';
import './cronstrue-BvVNjwUa.js';

const CredentialManagerItemsList = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
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
        return (h("div", { key: '5efec1b973aa9e7297397b817883d73c0237e908' }, h("div", { key: '0642fcb1feef779df0197e8867bbf77c52df16f5', class: "elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white" }, h("div", { key: 'f9de83b6dc47a03f1d90d24e4c7deb7096f19f89', class: "elsa-flex-1 elsa-min-w-0" }, h("h1", { key: 'd2b7849e6155870777d3df46a30ab3e1dc1b2c97', class: "elsa-text-lg elsa-font-medium elsa-leading-6 elsa-text-gray-900 sm:elsa-truncate" })), h("button", { key: 'a20d1e88f53f748facc024af046c60e6a87dc149', type: "button", onClick: () => this.onNewClick(), class: "elsa-mt-3 elsa-w-full elsa-inline-flex elsa-justify-center elsa-rounded-md elsa-border elsa-border-gray-300 elsa-shadow-sm elsa-px-4 elsa-py-2 elsa-bg-white elsa-text-base elsa-font-medium elsa-text-gray-700 hover:elsa-bg-gray-50 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 sm:elsa-mt-0 sm:elsa-ml-3 sm:elsa-w-auto sm:elsa-text-sm" }, "Add New")), this.renderListScreen(), this.renderSecretPickerModal(), this.renderSecretPickerEditor()));
    }
};
Tunnel.injectProps(CredentialManagerItemsList, ['culture', 'basePath', 'serverUrl', 'monacoLibPath']);

export { CredentialManagerItemsList as elsa_credential_manager_items_list };
//# sourceMappingURL=elsa-credential-manager-items-list.entry.esm.js.map
