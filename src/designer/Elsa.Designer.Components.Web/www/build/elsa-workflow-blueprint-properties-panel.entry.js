import { r as registerInstance, h, k as Host } from './index-ea213ee1.js';
import { l as loadTranslations } from './i18n-loader-aa6cec69.js';
import './index-e19c34cd.js';
import { T as Tunnel } from './dashboard-c739a7dd.js';
import { b as createElsaClient } from './elsa-client-17ed10a4.js';
import './event-bus-5d6f3774.js';
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

let ElsaWorkflowBlueprintPropertiesPanel = class {
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
    return (h(Host, null, h("dl", { class: "elsa-border-b elsa-border-gray-200 elsa-divide-y elsa-divide-gray-200" }, h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, t('Name')), h("dd", { class: "elsa-text-gray-900" }, name)), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, t('Id')), h("dd", { class: "elsa-text-gray-900 elsa-break-all" }, workflowBlueprint.id || '-')), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, t('Version')), h("dd", { class: "elsa-text-gray-900" }, workflowBlueprint.version)), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, t('PublishedVersion')), h("dd", { class: "elsa-text-gray-900" }, this.publishedVersion || '-')), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, t('Status')), h("dd", { class: `${isPublished ? 'elsa-text-green-600' : 'elsa-text-yellow-700'}` }, isPublished ? t('Published') : t('Draft'))))));
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
