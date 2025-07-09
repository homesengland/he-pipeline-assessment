import { r as registerInstance, h } from './index-ea213ee1.js';
import './index-c5018c3a.js';
import './index-842ad20c.js';
import { G as GetIntlMessage } from './intl-message-63e92c76.js';
import { l as loadTranslations } from './i18n-loader-aa6cec69.js';
import { T as Tunnel } from './dashboard-c739a7dd.js';
import { t as toggle } from './index-886428b8.js';
import { b as createElsaClient } from './elsa-client-ecb85def.js';
import './event-bus-6625fc04.js';
import './index-0f68dbd6.js';
import './utils-db96334c.js';
import './collection-ba09a015.js';
import './_commonjsHelpers-6cb8dacb.js';
import './cronstrue-37d55fa1.js';
import './index-2db7bf78.js';
import './axios-middleware.esm-fcda64d5.js';

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

let ElsaStudioWorkflowDefinitionsList = class {
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
    return (h("div", null, h("div", { class: "elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white" }, h("div", { class: "elsa-flex-1 elsa-min-w-0" }, h("h1", { class: "elsa-text-lg elsa-font-medium elsa-leading-6 elsa-text-gray-900 sm:elsa-truncate" }, h(IntlMessage, { label: "Title" }))), h("div", { class: "elsa-mt-4 elsa-flex sm:elsa-mt-0 sm:elsa-ml-4" }, h("span", { class: "elsa-relative elsa-z-20 elsa-inline-flex elsa-shadow-sm elsa-rounded-md" }, h("stencil-route-link", { url: `${basePath}/workflow-definitions/new`, class: "elsa-relative elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-l-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 focus:elsa-z-10" }, h(IntlMessage, { label: "CreateButton" })), h("span", { class: "-elsa-ml-px elsa-relative elsa-block" }, h("button", { onClick: () => this.toggleMenu(), id: "option-menu", type: "button", class: "elsa-relative elsa-inline-flex elsa-items-center elsa-px-2 elsa-py-2 elsa-rounded-r-md elsa-border elsa-border-transparent elsa-bg-blue-600 elsa-text-sm elsa-font-medium elsa-text-white hover:elsa-bg-blue-700 focus:elsa-z-10 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-blue-500 focus:elsa-border-blue-500" }, h("span", { class: "elsa-sr-only" }, "Open options"), h("svg", { class: "elsa-h-5 elsa-w-5", "x-description": "Heroicon name: solid/chevron-down", xmlns: "http://www.w3.org/2000/svg", viewBox: "0 0 20 20", fill: "currentColor", "aria-hidden": "true" }, h("path", { "fill-rule": "evenodd", d: "M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z", "clip-rule": "evenodd" }))), h("div", { ref: el => this.menu = el, "data-transition-enter": "elsa-transition elsa-ease-out elsa-duration-100", "data-transition-enter-start": "elsa-transform elsa-opacity-0 elsa-scale-95", "data-transition-enter-end": "elsa-transform elsa-opacity-100 elsa-scale-100", "data-transition-leave": "elsa-transition elsa-ease-in elsa-duration-75", "data-transition-leave-start": "elsa-transform elsa-opacity-100 elsa-scale-100", "data-transition-leave-end": "elsa-transform elsa-opacity-0 elsa-scale-95", class: "hidden origin-top-left elsa-absolute elsa-right-0 elsa-top-10 elsa-mb-2 -elsa-mr-1 elsa-w-56 elsa-rounded-md elsa-shadow-lg elsa-bg-white elsa-ring-1 elsa-ring-black elsa-ring-opacity-5" }, h("div", { class: "elsa-divide-y elsa-divide-gray-100 focus:elsa-outline-none", role: "menu", "aria-orientation": "vertical", "aria-labelledby": "option-menu" }, h("div", { class: "elsa-py-1", role: "none" }, h("a", { href: "#", onClick: (e) => this.restoreWorkflows(e), class: "elsa-block elsa-px-4 elsa-py-2 elsa-text-sm elsa-text-gray-700 hover:elsa-bg-gray-100 hover:elsa-text-gray-900", role: "menuitem" }, h(IntlMessage, { label: "RestoreButton" })), h("a", { href: `${basePath}/v1/workflow-definitions/backup`, onClick: (e) => this.toggleMenu(e), class: "elsa-block elsa-px-4 elsa-py-2 elsa-text-sm elsa-text-gray-700 hover:elsa-bg-gray-100 hover:elsa-text-gray-900", role: "menuitem" }, h(IntlMessage, { label: "BackupButton" }))))))))), h("elsa-workflow-definitions-list-screen", { ref: el => this.workflowDefinitionsListScreen = el }), h("input", { type: "file", class: "hidden", onChange: (e) => this.onFileInputChange(e), ref: el => this.fileInput = el, accept: ".zip" })));
  }
};
Tunnel.injectProps(ElsaStudioWorkflowDefinitionsList, ['serverUrl', 'culture', 'basePath']);

export { ElsaStudioWorkflowDefinitionsList as elsa_studio_workflow_definitions_list };
