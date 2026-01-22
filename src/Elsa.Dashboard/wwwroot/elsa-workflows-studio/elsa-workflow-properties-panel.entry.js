import { r as registerInstance, h, l as Host } from './index-1542df5c.js';
import { l as leave, e as enter } from './index-886428b8.js';
import { l as loadTranslations } from './i18n-loader-aa6cec69.js';
import './index-892f713d.js';
import { T as Tunnel } from './dashboard-beb9b1e8.js';
import { b as createElsaClient } from './elsa-client-8304c78c.js';
import './event-bus-5d6f3774.js';
import './index-1654a48d.js';
import './events-d0aab14a.js';
import './fetch-client-f0dc2a52.js';
import './utils-db96334c.js';
import './collection-ba09a015.js';
import './_commonjsHelpers-6cb8dacb.js';
import './cronstrue-37d55fa1.js';
import './index-2db7bf78.js';

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
    this.workflowDefinition = undefined;
    this.culture = undefined;
    this.serverUrl = undefined;
    this.publishedVersion = undefined;
    this.expanded = undefined;
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
    return (h(Host, null, h("dl", { class: "elsa-border-b elsa-border-gray-200 elsa-divide-y elsa-divide-gray-200" }, h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, t('Name')), h("dd", { class: "elsa-text-gray-900" }, name)), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, t('DisplayName')), h("dd", { class: "elsa-text-gray-900" }, workflowDefinition.displayName || '-')), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, t('Id')), h("dd", { class: "elsa-text-gray-900 elsa-break-all" }, workflowDefinition.definitionId || '-')), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, t('Version')), h("dd", { class: "elsa-text-gray-900" }, workflowDefinition.version)), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, t('PublishedVersion')), h("dd", { class: "elsa-text-gray-900" }, this.publishedVersion || '-')), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, t('Status')), h("dd", { class: `${isPublished ? 'elsa-text-green-600' : 'elsa-text-yellow-700'}` }, isPublished ? t('Published') : t('Draft'))))));
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
