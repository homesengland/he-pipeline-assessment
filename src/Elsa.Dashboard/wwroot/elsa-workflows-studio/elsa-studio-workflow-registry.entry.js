import { r as registerInstance, h } from './index-CL6j2ec2.js';
import { l as loadTranslations } from './i18n-loader-DJQycacf.js';
import { T as Tunnel } from './dashboard-DaK-DIvX.js';
import './index-C-8L13GY.js';

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

const ElsaStudioWorkflowRegistry = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
    }
    async componentWillLoad() {
        this.i18next = await loadTranslations(this.culture, resources);
    }
    render() {
        const basePath = this.basePath;
        const t = x => this.i18next.t(x);
        return (h("div", { key: 'c82c21650734643602067bcb56bebdfa32305d4a' }, h("div", { key: '1e98756807144ffd6a9977ec20b182e65c067868', class: "elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white" }, h("div", { key: '1e25aa174c177db2d1168e571dc250ba1695dead', class: "elsa-flex-1 elsa-min-w-0" }, h("h1", { key: 'd7d6108cb9058b017f009ca4d36ea819ef1adc58', class: "elsa-text-lg elsa-font-medium elsa-leading-6 elsa-text-gray-900 sm:elsa-truncate" }, t('Title'))), h("div", { key: '1657f978ba000087b1d6a0cc928c0b4b825ccb9c', class: "elsa-mt-4 elsa-flex sm:elsa-mt-0 sm:elsa-ml-4" }, h("elsa-nav-link", { key: '73e6714542d689f372067d320fe93c8e1a1c3e7b', url: `${basePath}/workflow-definitions/new`, anchorClass: "elsa-order-0 elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 sm:elsa-order-1 sm:elsa-ml-3" }, t('CreateButton')))), h("elsa-workflow-registry-list-screen", { key: 'd165ef3249353e0444956a6f76b4fd8ca934c663' })));
    }
};
Tunnel.injectProps(ElsaStudioWorkflowRegistry, ['culture', 'basePath']);

export { ElsaStudioWorkflowRegistry as elsa_studio_workflow_registry };
//# sourceMappingURL=elsa-studio-workflow-registry.entry.esm.js.map
