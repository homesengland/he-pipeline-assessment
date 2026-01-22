import { r as registerInstance, h } from './index-1542df5c.js';
import { l as loadTranslations } from './i18n-loader-aa6cec69.js';

const resources = {
  'en': {
    default: {
      'Title': 'Workflow Instances'
    }
  },
  'zh-CN': {
    default: {
      'Title': '工作流实例'
    }
  },
  'nl-NL': {
    default: {
      'Title': 'Workflow Instanties'
    }
  },
  'fa-IR': {
    default: {
      'Title': 'فرآیندهای اجرا شده'
    }
  },
  'de-DE': {
    default: {
      'Title': 'Ablaufinstanzen'
    }
  }
};

const ElsaStudioWorkflowInstancesList = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.culture = undefined;
  }
  async componentWillLoad() {
    this.i18next = await loadTranslations(this.culture, resources);
  }
  render() {
    const t = x => this.i18next.t(x);
    return (h("div", null, h("div", { class: "elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white" }, h("div", { class: "elsa-flex-1 elsa-min-w-0" }, h("h1", { class: "elsa-text-lg elsa-font-medium elsa-leading-6 elsa-text-gray-900 sm:elsa-truncate" }, t('Title')))), h("elsa-workflow-instance-list-screen", null)));
  }
};

export { ElsaStudioWorkflowInstancesList as elsa_studio_workflow_instances_list };
