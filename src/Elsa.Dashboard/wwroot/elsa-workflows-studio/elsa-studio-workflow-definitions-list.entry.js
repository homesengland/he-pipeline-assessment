import { r as registerInstance, h } from './index-CL6j2ec2.js';
import './index-fZDMH_YE.js';
import './index-CBYiEsWN.js';
import { G as GetIntlMessage } from './intl-message-C60V_pHc.js';
import { l as loadTranslations } from './i18n-loader-DJQycacf.js';
import { T as Tunnel } from './dashboard-DaK-DIvX.js';
import { t as toggle } from './index-jup-zNrU.js';
import { b as createElsaClient } from './elsa-client-q6ob5JPZ.js';
import './event-bus-axQqcjdg.js';
import './index-D7wXd6HU.js';
import './events-CpKc8CLe.js';
import './utils-C0M_5Llz.js';
import './collection-B4sYCr2r.js';
import './_commonjsHelpers-Cf5sKic0.js';
import './cronstrue-BvVNjwUa.js';
import './index-C-8L13GY.js';
import './fetch-client-1OcjQcrw.js';

const resources = {
    'en': {
        default: {
            'Title': 'Workflow Definitions',
            'CreateButton': 'Create Workflow',
            'BackupButton': 'Backup Workflows',
            'RestoreButton': 'Restore Workflows'
        }
    },
    'zh-CN': {
        default: {
            'Title': '工作流程定义',
            'CreateButton': '创建工作流程'
        }
    },
    'nl-NL': {
        default: {
            'Title': 'Workflow Definities',
            'CreateButton': 'Nieuwe Workflow'
        }
    },
    'fa-IR': {
        default: {
            'Title': 'مشخصات فرآیندها',
            'CreateButton': 'ایجاد فرآیند'
        }
    },
    'de-DE': {
        default: {
            'Title': 'Ablaufdefinitionen',
            'CreateButton': 'Ablauf erstellen'
        }
    },
};

const ElsaStudioWorkflowDefinitionsList = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
        this.restoreWorkflows = async (e) => {
            e.preventDefault();
            this.fileInput.value = null;
            this.fileInput.click();
            toggle(this.menu);
        };
    }
    async componentWillLoad() {
        this.i18next = await loadTranslations(this.culture, resources);
    }
    async onFileInputChange(e) {
        const files = this.fileInput.files;
        if (files.length == 0) {
            return;
        }
        const file = files[0];
        const elsaClient = await createElsaClient(this.serverUrl);
        try {
            await elsaClient.workflowDefinitionsApi.restore(file);
        }
        catch (e) {
            console.error(e);
        }
        await this.workflowDefinitionsListScreen.loadWorkflowDefinitions();
    }
    toggleMenu(e) {
        toggle(this.menu);
    }
    render() {
        const basePath = this.basePath;
        const IntlMessage = GetIntlMessage(this.i18next);
        return (h("div", { key: '16501a17c36b4faefe0f8eefbd724efe95185b7b' }, h("div", { key: '6b5719cff183a00bb66850dbf589d2b9fe45a15d', class: "elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white" }, h("div", { key: '8de2e3854972526fc55683d2a20ba5d6aae5f82f', class: "elsa-flex-1 elsa-min-w-0" }, h("h1", { key: '36fbb9ddd19dc957e929f5f30a58f03b20c31ea0', class: "elsa-text-lg elsa-font-medium elsa-leading-6 elsa-text-gray-900 sm:elsa-truncate" }, h(IntlMessage, { key: 'bfa1e3483630508dc929f8c72646c4fce30c31f5', label: "Title" }))), h("div", { key: 'b27afad19d77ad3ec127d81382b0a0b44fbf8ef7', class: "elsa-mt-4 elsa-flex sm:elsa-mt-0 sm:elsa-ml-4" }, h("span", { key: '03c3179936a65292b61ae144466a4da161ddfa14', class: "elsa-relative elsa-z-20 elsa-inline-flex elsa-shadow-sm elsa-rounded-md" }, h("elsa-nav-link", { key: 'd8e1c2944ef5f618d17c52919bfd67727b3b706b', url: `${basePath}/workflow-definitions/new`, anchorClass: "elsa-relative elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-l-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 focus:elsa-z-10" }, h(IntlMessage, { key: '97592ae1c8929cdb15bc11f5eff86024796424a3', label: "CreateButton" })), h("span", { key: 'd5b1931a8c77472d71c3c6fc3b31c202cbc9f7a3', class: "-elsa-ml-px elsa-relative elsa-block" }, h("button", { key: '54cb98bc3b67d685a4e8303731007170806a5401', onClick: () => this.toggleMenu(), id: "option-menu", type: "button", class: "elsa-relative elsa-inline-flex elsa-items-center elsa-px-2 elsa-py-2 elsa-rounded-r-md elsa-border elsa-border-transparent elsa-bg-blue-600 elsa-text-sm elsa-font-medium elsa-text-white hover:elsa-bg-blue-700 focus:elsa-z-10 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-blue-500 focus:elsa-border-blue-500" }, h("span", { key: 'b0982c15291982f6a4b1bd11e8964cf02079f12d', class: "elsa-sr-only" }, "Open options"), h("svg", { key: 'ade5e9ef9265d5232a7a616c925337c18220801e', class: "elsa-h-5 elsa-w-5", "x-description": "Heroicon name: solid/chevron-down", xmlns: "http://www.w3.org/2000/svg", viewBox: "0 0 20 20", fill: "currentColor", "aria-hidden": "true" }, h("path", { key: 'dd98de2ffd756b79fbeb13cd4bcea4eecc793f9c', "fill-rule": "evenodd", d: "M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z", "clip-rule": "evenodd" }))), h("div", { key: '4258660f41e1c77539780299a71c1ab14855dff4', ref: el => this.menu = el, "data-transition-enter": "elsa-transition elsa-ease-out elsa-duration-100", "data-transition-enter-start": "elsa-transform elsa-opacity-0 elsa-scale-95", "data-transition-enter-end": "elsa-transform elsa-opacity-100 elsa-scale-100", "data-transition-leave": "elsa-transition elsa-ease-in elsa-duration-75", "data-transition-leave-start": "elsa-transform elsa-opacity-100 elsa-scale-100", "data-transition-leave-end": "elsa-transform elsa-opacity-0 elsa-scale-95", class: "hidden origin-top-left elsa-absolute elsa-right-0 elsa-top-10 elsa-mb-2 -elsa-mr-1 elsa-w-56 elsa-rounded-md elsa-shadow-lg elsa-bg-white elsa-ring-1 elsa-ring-black elsa-ring-opacity-5" }, h("div", { key: '7d361b44146781cb7f72a126469ee60a819bbb83', class: "elsa-divide-y elsa-divide-gray-100 focus:elsa-outline-none", role: "menu", "aria-orientation": "vertical", "aria-labelledby": "option-menu" }, h("div", { key: '5a581260253f987ad73fe3d3371e0098a9ddb5d6', class: "elsa-py-1", role: "none" }, h("a", { key: '1a6470e5f529dac91a8b5c8f4e3ee4e064ded307', href: "#", onClick: (e) => this.restoreWorkflows(e), class: "elsa-block elsa-px-4 elsa-py-2 elsa-text-sm elsa-text-gray-700 hover:elsa-bg-gray-100 hover:elsa-text-gray-900", role: "menuitem" }, h(IntlMessage, { key: 'cd917cb4faa8ea8ac9dc4d273e08b484aff14721', label: "RestoreButton" })), h("a", { key: '0686b47186475b252c721f9b73cbd4c79bb3a63e', href: `${basePath}/v1/workflow-definitions/backup`, onClick: (e) => this.toggleMenu(e), class: "elsa-block elsa-px-4 elsa-py-2 elsa-text-sm elsa-text-gray-700 hover:elsa-bg-gray-100 hover:elsa-text-gray-900", role: "menuitem" }, h(IntlMessage, { key: 'ae137cd680f7f46a2736bff2f7ea2212e83ba154', label: "BackupButton" }))))))))), h("elsa-workflow-definitions-list-screen", { key: '28f96a9d878fb101f103ae8839a102dfde96b0bc', ref: el => this.workflowDefinitionsListScreen = el }), h("input", { key: 'b0f0ef2da98d9db6a2bc5b5905191fb779174d09', type: "file", class: "hidden", onChange: (e) => this.onFileInputChange(e), ref: el => this.fileInput = el, accept: ".zip" })));
    }
};
Tunnel.injectProps(ElsaStudioWorkflowDefinitionsList, ['serverUrl', 'culture', 'basePath']);

export { ElsaStudioWorkflowDefinitionsList as elsa_studio_workflow_definitions_list };
//# sourceMappingURL=elsa-studio-workflow-definitions-list.entry.esm.js.map
