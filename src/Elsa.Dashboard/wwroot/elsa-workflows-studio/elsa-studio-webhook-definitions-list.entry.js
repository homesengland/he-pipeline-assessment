import { r as registerInstance, h } from './index-CL6j2ec2.js';
import './index-CBYiEsWN.js';
import { l as loadTranslations } from './i18n-loader-DJQycacf.js';
import { G as GetIntlMessage } from './intl-message-C60V_pHc.js';
import { T as Tunnel } from './dashboard-DaK-DIvX.js';
import './_commonjsHelpers-Cf5sKic0.js';
import './index-C-8L13GY.js';

const resources = {
    'en': {
        default: {
            'Title': 'Webhook Definitions',
            'CreateButton': 'Create Webhook'
        }
    },
    'zh-CN': {
        default: {
            'Title': 'Webhook的定义',
            'CreateButton': '创建 Webhook'
        }
    },
    'nl-NL': {
        default: {
            'Title': 'Webhook Definitie',
            'CreateButton': 'Maak Webhook'
        }
    },
    'fa-IR': {
        default: {
            'Title': 'هاWebhook مشخصات',
            'CreateButton': 'ایجاد Webhook'
        }
    },
    'de-Ide': {
        default: {
            'Title': 'Webhook Definitionen',
            'CreateButton': 'Webhook erstellen'
        }
    },
};

const ElsaStudioWebhookDefinitionsList = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
    }
    async componentWillLoad() {
        this.i18next = await loadTranslations(this.culture, resources);
    }
    render() {
        const basePath = this.basePath;
        const IntlMessage = GetIntlMessage(this.i18next);
        return (h("div", { key: 'f0066c028875f9baa93a77376b66a2ee97ad861d' }, h("div", { key: 'e738c93ca1597aa3c37b5d49fb253dd16bc93f7c', class: "elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white" }, h("div", { key: 'd6aa00000e54b838d7e121fc1d7448b1c863492f', class: "elsa-flex-1 elsa-min-w-0" }, h("h1", { key: '9cc1adce9db47ef908c0079313476723311da45b', class: "elsa-text-lg elsa-font-medium elsa-leading-6 elsa-text-gray-900 sm:elsa-truncate" }, h(IntlMessage, { key: '559ba2c08a5ef944242b91573ce0e7f9042a714a', label: "Title" }))), h("div", { key: '5a66d4434ca52e7848409408ad92d8f23a1754ce', class: "elsa-mt-4 elsa-flex sm:elsa-mt-0 sm:elsa-ml-4" }, h("elsa-nav-link", { key: '5a176e24923eaa141bfd95c23e90c442af9f9dfa', url: `${basePath}/webhook-definitions/new`, anchorClass: "elsa-order-0 elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 sm:elsa-order-1 sm:elsa-ml-3" }, h(IntlMessage, { key: 'ceb7fef322172e137bc958c2f3d38d9988c0952c', label: "CreateButton" })))), h("elsa-webhook-definitions-list-screen", { key: '3242ad6e938aab0970657598ac826a99d5ba70f4' })));
    }
};
Tunnel.injectProps(ElsaStudioWebhookDefinitionsList, ['culture', 'basePath']);

export { ElsaStudioWebhookDefinitionsList as elsa_studio_webhook_definitions_list };
//# sourceMappingURL=elsa-studio-webhook-definitions-list.entry.esm.js.map
