import { r as registerInstance, h } from './index-ea213ee1.js';
import { l as loadTranslations } from './i18n-loader-aa6cec69.js';
import { T as Tunnel } from './dashboard-c739a7dd.js';
import './index-2db7bf78.js';

const resources = {
  'en': {
    default: {
      'Title': 'Workflow Registry',
      'CreateButton': 'Create Workflow'
    }
  },
  'zh-CN': {
    default: {
      'Title': '工作流程注册表',
      'CreateButton': '创建工作流程'
    }
  },
  'nl-NL': {
    default: {
      'Title': 'Workflow Register',
      'CreateButton': 'Nieuwe Workflow'
    }
  },
  'fa-IR': {
    default: {
      'Title': 'لیست فرآیندها',
      'CreateButton': 'ایجاد فرآیند'
    }
  },
  'de-DE': {
    default: {
      'Title': 'Ablaufverzeichnis',
      'CreateButton': 'Ablauf erstellen'
    }
  },
};

let ElsaStudioWorkflowRegistry = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
  }
  async componentWillLoad() {
    this.i18next = await loadTranslations(this.culture, resources);
  }
  render() {
    const basePath = this.basePath;
    const t = x => this.i18next.t(x);
    return (h("div", null, h("div", { class: "elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white" }, h("div", { class: "elsa-flex-1 elsa-min-w-0" }, h("h1", { class: "elsa-text-lg elsa-font-medium elsa-leading-6 elsa-text-gray-900 sm:elsa-truncate" }, t('Title'))), h("div", { class: "elsa-mt-4 elsa-flex sm:elsa-mt-0 sm:elsa-ml-4" }, h("stencil-route-link", { url: `${basePath}/workflow-definitions/new`, class: "elsa-order-0 elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 sm:elsa-order-1 sm:elsa-ml-3" }, t('CreateButton')))), h("elsa-workflow-registry-list-screen", null)));
  }
};
Tunnel.injectProps(ElsaStudioWorkflowRegistry, ['culture', 'basePath']);

export { ElsaStudioWorkflowRegistry as elsa_studio_workflow_registry };
