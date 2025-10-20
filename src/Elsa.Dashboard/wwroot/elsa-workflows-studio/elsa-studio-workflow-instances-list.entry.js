import { r as registerInstance, h } from './index-CL6j2ec2.js';
import { l as loadTranslations } from './i18n-loader-DJQycacf.js';

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
    }
    async componentWillLoad() {
        this.i18next = await loadTranslations(this.culture, resources);
    }
    render() {
        const t = x => this.i18next.t(x);
        return (h("div", { key: 'a75cc6358476d7421655b26998b44e5d407a20eb' }, h("div", { key: '5de6226cb82fc7f15a4451bf5568dbfd1d115eae', class: "elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white" }, h("div", { key: '17247d6afe00ec9b825b5c0e275c12598d7660a9', class: "elsa-flex-1 elsa-min-w-0" }, h("h1", { key: 'c0d3f4ff1bf8ad5809d9d30c1ade4d4805db41ec', class: "elsa-text-lg elsa-font-medium elsa-leading-6 elsa-text-gray-900 sm:elsa-truncate" }, t('Title')))), h("elsa-workflow-instance-list-screen", { key: 'e2da24ff24f3fbc648ac26b22dd8f2475541b29e' })));
    }
};

export { ElsaStudioWorkflowInstancesList as elsa_studio_workflow_instances_list };
//# sourceMappingURL=elsa-studio-workflow-instances-list.entry.esm.js.map
