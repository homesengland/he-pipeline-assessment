import { r as registerInstance, h, j as Host } from './index-CL6j2ec2.js';
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
            'Untitled': 'Untitled',
            'Id': 'Id',
            'Version': 'Latest Version',
            'PublishedVersion': 'Published Version',
            'Status': 'Status',
            'Published': 'Published',
            'Draft': 'Draft',
            'Properties': '{{name}} Properties'
        }
    },
    'zh-CN': {
        default: {
            'Name': '名称',
            'Untitled': '无题',
            'Id': 'Id',
            'Version': '最新版本',
            'PublishedVersion': '发布版本',
            'Status': '状态',
            'Published': '已发布',
            'Draft': '草稿',
            'Properties': '{{name}} 属性'
        }
    },
    'nl-NL': {
        default: {
            'Name': 'Naam',
            'Untitled': 'Untitled',
            'Id': 'Id',
            'Version': 'Laatste Versie',
            'PublishedVersion': 'Gepubliceerde Versie',
            'Status': 'Status',
            'Published': 'Published',
            'Draft': 'Draft',
            'Properties': '{{name}} Properties'
        }
    },
    'fa-IR': {
        default: {
            'Name': 'عنوان',
            'Untitled': 'بی نام',
            'Id': 'شناسه',
            'Version': 'جدیدترین ویرایش',
            'PublishedVersion': 'ویرایش منتشر شده',
            'Status': 'وضعیت',
            'Published': 'منتشر شده',
            'Draft': 'پیش نویس',
            'Properties': '{{name}} ویژگی های'
        }
    },
    'de-DE': {
        default: {
            'Name': 'Name',
            'Untitled': 'Unbenannt',
            'Id': 'Id',
            'Version': 'Neuste Version',
            'PublishedVersion': 'Veröffentlichte Version',
            'Status': 'Status',
            'Published': 'Veröffentlicht',
            'Draft': 'Entwurf',
            'Properties': '{{name}} Eigenschaften'
        }
    },
};

const ElsaWorkflowBlueprintPropertiesPanel = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
    }
    async workflowIdChangedHandler(newWorkflowId) {
        await this.loadWorkflowBlueprint(newWorkflowId);
        await this.loadPublishedVersion();
    }
    async componentWillLoad() {
        this.i18next = await loadTranslations(this.culture, resources);
        await this.loadWorkflowBlueprint();
        await this.loadPublishedVersion();
    }
    render() {
        const t = (x, params) => this.i18next.t(x, params);
        const { workflowBlueprint } = this;
        const name = workflowBlueprint.name || this.i18next.t("Untitled");
        const { isPublished } = workflowBlueprint;
        return (h(Host, { key: '6f875f375667ea829e7781ea213e21c8679f2bbc' }, h("dl", { key: 'a2a045260f22f97164d4f31288ef7fdfd8a0de6b', class: "elsa-border-b elsa-border-gray-200 elsa-divide-y elsa-divide-gray-200" }, h("div", { key: '8b5051bd8c6e7d191f3b1df5f14d31dc581bc83e', class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { key: '0b48e7615112c54efe3b9b64088286be16a418b7', class: "elsa-text-gray-500" }, t('Name')), h("dd", { key: 'eaf8b4fcb1522e7f39d44d43cb4a728f4d69aa51', class: "elsa-text-gray-900" }, name)), h("div", { key: '5f27e3033384eeb5d8d360fd03497498cf17162a', class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { key: '243efcadf0e070fe5ce775079a659cb5cd695588', class: "elsa-text-gray-500" }, t('Id')), h("dd", { key: 'fd2f88fa88de0c48bad0f539e69110b72d6e7bd2', class: "elsa-text-gray-900 elsa-break-all" }, workflowBlueprint.id || '-')), h("div", { key: 'cfc8f8143f501a02fdea91ef6ecaf810c0018168', class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { key: 'e4d435580db4d1444819c30ae394ad60b984678f', class: "elsa-text-gray-500" }, t('Version')), h("dd", { key: '0799af27428fca45dae860fcc05aee40856eae92', class: "elsa-text-gray-900" }, workflowBlueprint.version)), h("div", { key: 'd58b3b321d8adb36c0307c9972ea8502ea239d05', class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { key: 'ccaa880438b1126399eb36edf48a9a85efb4415a', class: "elsa-text-gray-500" }, t('PublishedVersion')), h("dd", { key: '6a99f323805ac9c19e5f4ab2bf88f0917ed59664', class: "elsa-text-gray-900" }, this.publishedVersion || '-')), h("div", { key: '5e82634f8beba2ac97c93fbaecc8284ac3818ef6', class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { key: '7337dc55a2679c1f9bde4f68d38b439d592b60be', class: "elsa-text-gray-500" }, t('Status')), h("dd", { key: '0ab12279ccfa1a9b855ea4bcfbf7f07d199a1939', class: `${isPublished ? 'elsa-text-green-600' : 'elsa-text-yellow-700'}` }, isPublished ? t('Published') : t('Draft'))))));
    }
    createClient() {
        return createElsaClient(this.serverUrl);
    }
    async loadPublishedVersion() {
        const elsaClient = await this.createClient();
        const { workflowBlueprint } = this;
        if (workflowBlueprint.isPublished) {
            const publishedWorkflowDefinitions = await elsaClient.workflowDefinitionsApi.getMany([workflowBlueprint.id], { isPublished: true });
            const publishedDefinition = workflowBlueprint.isPublished ? workflowBlueprint : publishedWorkflowDefinitions.find(x => x.definitionId == workflowBlueprint.id);
            if (publishedDefinition) {
                this.publishedVersion = publishedDefinition.version;
            }
        }
        else {
            this.publishedVersion = 0;
        }
    }
    async loadWorkflowBlueprint(workflowId = this.workflowId) {
        const elsaClient = await this.createClient();
        this.workflowBlueprint = await elsaClient.workflowRegistryApi.get(workflowId, { isLatest: true });
    }
    static get watchers() { return {
        "workflowId": ["workflowIdChangedHandler"]
    }; }
};
Tunnel.injectProps(ElsaWorkflowBlueprintPropertiesPanel, ['serverUrl', 'culture']);

export { ElsaWorkflowBlueprintPropertiesPanel as elsa_workflow_blueprint_properties_panel };
//# sourceMappingURL=elsa-workflow-blueprint-properties-panel.entry.esm.js.map
