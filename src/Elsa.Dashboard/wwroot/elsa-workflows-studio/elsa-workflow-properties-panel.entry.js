import { r as registerInstance, h, j as Host } from './index-CL6j2ec2.js';
import { l as leave, e as enter } from './index-jup-zNrU.js';
import { l as loadTranslations } from './i18n-loader-DJQycacf.js';
import './index-fZDMH_YE.js';
import { T as Tunnel } from './dashboard-DaK-DIvX.js';
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
            'Name': 'Name',
            'DisplayName': 'Display Name',
            'Untitled': 'Untitled',
            'Id': 'Id',
            'Version': 'Latest Version',
            'PublishedVersion': 'Published Version',
            'Status': 'Status',
            'Published': 'Published',
            'Draft': 'Draft'
        }
    },
    'zh-CN': {
        default: {
            'Name': '名称',
            'DisplayName': '展示名称',
            'Untitled': '无题',
            'Id': 'Id',
            'Version': '最新版本',
            'PublishedVersion': '发布版本',
            'Status': '状态',
            'Published': '已发布',
            'Draft': '草稿'
        }
    },
    'nl-NL': {
        default: {
            'Name': 'Naam',
            'DisplayName': 'Weergavenaam',
            'Untitled': 'Untitled',
            'Id': 'Id',
            'Version': 'Laatste Versie',
            'PublishedVersion': 'Gepubliceerde Versie',
            'Status': 'Status',
            'Published': 'Published',
            'Draft': 'Draft'
        }
    },
    'fa-IR': {
        default: {
            'Name': 'نام',
            'DisplayName': 'عنوان نمایشی',
            'Untitled': 'بی نام',
            'Id': 'شناسه',
            'Version': 'جدیدترین ویرایش',
            'PublishedVersion': 'ویرایش منتشر شده',
            'Status': 'وضعیت',
            'Published': 'منتشر شده',
            'Draft': 'پیش نویس'
        }
    },
    'de-DE': {
        default: {
            'Name': 'Name',
            'DisplayName': 'Anzeigename',
            'Untitled': 'Unbenannt',
            'Id': 'Id',
            'Version': 'Neuste Version',
            'PublishedVersion': 'Veröffentlichte Version',
            'Status': 'Status',
            'Published': 'Veröffentlicht',
            'Draft': 'Entwurf'
        }
    },
};

const ElsaWorkflowPropertiesPanel = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
        this.toggle = () => {
            if (this.expanded) {
                leave(this.el).then(() => this.expanded = false);
            }
            else {
                this.expanded = true;
                enter(this.el);
            }
        };
    }
    async workflowDefinitionChangedHandler(newWorkflow, oldWorkflow) {
        if (newWorkflow.version !== oldWorkflow.version || newWorkflow.isPublished !== oldWorkflow.isPublished || newWorkflow.isLatest !== oldWorkflow.isLatest)
            await this.loadPublishedVersion();
    }
    async componentWillLoad() {
        this.i18next = await loadTranslations(this.culture, resources);
        await this.loadPublishedVersion();
    }
    render() {
        const t = (x, params) => this.i18next.t(x, params);
        const { workflowDefinition } = this;
        const name = workflowDefinition.name || this.i18next.t("Untitled");
        const { isPublished } = workflowDefinition;
        return (h(Host, { key: 'a7a16491518910b49b7d42d3877da277256f8c46' }, h("dl", { key: '6da165280a513c5f123a5f57e92323cea1235276', class: "elsa-border-b elsa-border-gray-200 elsa-divide-y elsa-divide-gray-200" }, h("div", { key: '47bb6821a3d56bfe38c4b2cc88070ef283987baf', class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { key: '11a8f64477e0c5ecf28cd4f2911154cc9979efdc', class: "elsa-text-gray-500" }, t('Name')), h("dd", { key: 'bf41b4da35f784db1cd99946c1a53c1ba2b6576c', class: "elsa-text-gray-900" }, name)), h("div", { key: '8e53e110cc7d555de8a9ee1f330300e496ebf85b', class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { key: '18a6ba79cbfcb6413ee0be568e87b2f4ecee5d8e', class: "elsa-text-gray-500" }, t('DisplayName')), h("dd", { key: '0ce1be03f426a8c6b5247093ca54fe0c70263970', class: "elsa-text-gray-900" }, workflowDefinition.displayName || '-')), h("div", { key: 'c504123e5681a9fc347608718dedbe168ed7b5b5', class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { key: '0e2110bf48bb7d5d784821869a8f6d540069bc4c', class: "elsa-text-gray-500" }, t('Id')), h("dd", { key: 'edf3cde6f72698ad2a86c61b68d9a3c97c40fd89', class: "elsa-text-gray-900 elsa-break-all" }, workflowDefinition.definitionId || '-')), h("div", { key: 'eb57a1e8c946c32592354490a8a53b9112f01c67', class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { key: '7b4c6301126664957842b6a23fb8e8adfb222579', class: "elsa-text-gray-500" }, t('Version')), h("dd", { key: '46b416ac28d8c9ba23814760e7a4e9503abc012d', class: "elsa-text-gray-900" }, workflowDefinition.version)), h("div", { key: 'a1baf23f0d49a2134c5c19d67b6bb1e648036059', class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { key: '9a08a59a0b0b1156d6e150c749a66a2ee13383ee', class: "elsa-text-gray-500" }, t('PublishedVersion')), h("dd", { key: '47e15920ddabcf675dbb83124a077625c650aa97', class: "elsa-text-gray-900" }, this.publishedVersion || '-')), h("div", { key: 'b3798aaea223f9c18c1138e800d5fb5fb67b8dfe', class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { key: 'fa7ea75e0d7cb3472603ae60f5cb2b217806aba6', class: "elsa-text-gray-500" }, t('Status')), h("dd", { key: '0e11ae52cd5039015946774fb1358e8a9ffa60cb', class: `${isPublished ? 'elsa-text-green-600' : 'elsa-text-yellow-700'}` }, isPublished ? t('Published') : t('Draft'))))));
    }
    createClient() {
        return createElsaClient(this.serverUrl);
    }
    async loadPublishedVersion() {
        const elsaClient = await this.createClient();
        const { workflowDefinition } = this;
        const publishedWorkflowDefinitions = await elsaClient.workflowDefinitionsApi.getMany([workflowDefinition.definitionId], { isPublished: true });
        const publishedDefinition = workflowDefinition.isPublished ? workflowDefinition : publishedWorkflowDefinitions.find(x => x.definitionId == workflowDefinition.definitionId);
        if (publishedDefinition) {
            this.publishedVersion = publishedDefinition.version;
        }
    }
    static get watchers() { return {
        "workflowDefinition": ["workflowDefinitionChangedHandler"]
    }; }
};
Tunnel.injectProps(ElsaWorkflowPropertiesPanel, ['serverUrl', 'culture']);

export { ElsaWorkflowPropertiesPanel as elsa_workflow_properties_panel };
//# sourceMappingURL=elsa-workflow-properties-panel.entry.esm.js.map
