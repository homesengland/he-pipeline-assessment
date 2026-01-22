import { r as registerInstance, h } from './index-1542df5c.js';
import './index-892f713d.js';
import { i as injectHistory } from './index-53e05b34.js';
import { l as loadTranslations } from './i18n-loader-aa6cec69.js';
import { G as GetIntlMessage } from './intl-message-2593bae2.js';
import { T as Tunnel } from './dashboard-beb9b1e8.js';
import { c as parseQuery } from './utils-db96334c.js';
import { b as createElsaClient } from './elsa-client-8304c78c.js';
import './event-bus-5d6f3774.js';
import './index-1654a48d.js';
import './events-d0aab14a.js';
import './fetch-client-f0dc2a52.js';
import './_commonjsHelpers-6cb8dacb.js';
import './cronstrue-37d55fa1.js';
import './active-router-16dd3465.js';
import './index-2db7bf78.js';
import './match-path-02f6df12.js';
import './location-utils-6419c2b3.js';
import './index-842ad20c.js';
import './collection-ba09a015.js';

const resources = {
  'en': {
    'default': {
      'Name': 'Name',
      'Instances': 'Instances',
      'LatestVersion': 'Latest Version',
      'PublishedVersion': 'Published Version',
      'Edit': 'Edit',
      'Delete': 'Delete',
      'DeleteConfirmationModel': {
        'Title': 'Delete Workflow Definition',
        'Message': 'Are you sure you wish to permanently delete this workflow, including all of its workflow instances?'
      }
    }
  },
  'zh-CN': {
    'default': {
      'Name': '名称',
      'Instances': '实例',
      'LatestVersion': '最新版本',
      'PublishedVersion': '发布版本',
      'Edit': '编辑',
      'Delete': '删除',
      'DeleteConfirmationModel': {
        'Title': '删除工作流程定义',
        'Message': '你确定要永久删除这个工作流程，包括它的所有工作流程实例？'
      }
    }
  },
  'nl-NL': {
    'default': {
      'Name': 'Naam',
      'Instances': 'Instanties',
      'LatestVersion': 'Laatste Versie',
      'PublishedVersion': 'Gepubliceerde Versie',
      'Edit': 'Bewerken',
      'Delete': 'Verwijderen',
      'DeleteConfirmationModel': {
        'Title': 'Verwijder Workflow Definitie',
        'Message': 'Weet je zeker dat je deze workflow permanent wilt verwijderen, inclusief alle bijbehorende workflow instanties?'
      }
    }
  },
  'fa-IR': {
    'default': {
      'Name': 'نام',
      'Instances': 'جریانها',
      'LatestVersion': 'جدیدترین ویرایش',
      'PublishedVersion': 'ویرایش منتشر شده',
      'Edit': 'ویرایش',
      'Delete': 'حذف',
      'DeleteConfirmationModel': {
        'Title': 'حذف تعریف فرآیند',
        'Message': 'آیا از حذف این فرآیند و همه جریانهای آن اطمینان دارید?'
      }
    }
  },
  'de-DE': {
    'default': {
      'Name': 'Name',
      'Instances': 'Instanzen',
      'LatestVersion': 'Neuste Version',
      'PublishedVersion': 'Veröffentlichte Version',
      'Edit': 'Bearbeiten',
      'Delete': 'Entfernen',
      'DeleteConfirmationModel': {
        'Title': 'Ablaufdefinition entfernen',
        'Message': 'Sind Sie sicher, dass sie die Definition und die dazugehörigen Instanzen unwiderruflich löschen wollen?'
      }
    }
  },
};

const ElsaWorkflowDefinitionsListScreen = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.t = (key, options) => this.i18next.t(key, options);
    this.onPaged = async (e) => {
      this.currentPage = e.detail.page;
      await this.loadWorkflowDefinitions();
    };
    this.history = undefined;
    this.serverUrl = undefined;
    this.culture = undefined;
    this.basePath = undefined;
    this.workflowDefinitions = { items: [], page: 1, pageSize: 50, totalCount: 0 };
    this.currentPage = 0;
    this.currentPageSize = ElsaWorkflowDefinitionsListScreen.DEFAULT_PAGE_SIZE;
    this.currentSearchTerm = undefined;
  }
  connectedCallback() {
    if (!!this.history)
      this.clearRouteChangedListeners = this.history.listen(e => this.routeChanged(e));
  }
  disconnectedCallback() {
    if (!!this.clearRouteChangedListeners)
      this.clearRouteChangedListeners();
  }
  async componentWillLoad() {
    this.i18next = await loadTranslations(this.culture, resources);
    if (!!this.history)
      this.applyQueryString(this.history.location.search);
    await this.loadWorkflowDefinitions();
  }
  async onSearch(e) {
    e.preventDefault();
    const form = e.currentTarget;
    const formData = new FormData(form);
    const searchTerm = formData.get('searchTerm');
    this.currentSearchTerm = searchTerm.toString();
    await this.loadWorkflowDefinitions();
  }
  applyQueryString(queryString) {
    const query = parseQuery(queryString);
    this.currentPage = !!query.page ? parseInt(query.page) : 0;
    this.currentPage = isNaN(this.currentPage) ? ElsaWorkflowDefinitionsListScreen.START_PAGE : this.currentPage;
    this.currentPageSize = !!query.pageSize ? parseInt(query.pageSize) : ElsaWorkflowDefinitionsListScreen.DEFAULT_PAGE_SIZE;
    this.currentPageSize = isNaN(this.currentPageSize) ? ElsaWorkflowDefinitionsListScreen.DEFAULT_PAGE_SIZE : this.currentPageSize;
    this.currentPageSize = Math.max(Math.min(this.currentPageSize, ElsaWorkflowDefinitionsListScreen.MAX_PAGE_SIZE), ElsaWorkflowDefinitionsListScreen.MIN_PAGE_SIZE);
  }
  async onPublishClick(e, workflowDefinition) {
    const elsaClient = await this.createClient();
    await elsaClient.workflowDefinitionsApi.publish(workflowDefinition.definitionId);
    await this.loadWorkflowDefinitions();
  }
  async onUnPublishClick(e, workflowDefinition) {
    const elsaClient = await this.createClient();
    await elsaClient.workflowDefinitionsApi.retract(workflowDefinition.definitionId);
    await this.loadWorkflowDefinitions();
  }
  async onDeleteClick(e, workflowDefinition) {
    const t = x => this.i18next.t(x);
    const result = await this.confirmDialog.show(t('DeleteConfirmationModel.Title'), t('DeleteConfirmationModel.Message'));
    if (!result)
      return;
    const elsaClient = await this.createClient();
    await elsaClient.workflowDefinitionsApi.delete(workflowDefinition.definitionId, { allVersions: true });
    await this.loadWorkflowDefinitions();
  }
  async routeChanged(e) {
    if (!e.pathname.toLowerCase().endsWith('workflow-definitions'))
      return;
    this.applyQueryString(e.search);
    await this.loadWorkflowDefinitions();
  }
  async loadWorkflowDefinitions() {
    const elsaClient = await this.createClient();
    const page = this.currentPage;
    const pageSize = this.currentPageSize;
    const latestOrPublishedVersionOptions = { isLatestOrPublished: true };
    this.workflowDefinitions = await elsaClient.workflowDefinitionsApi.list(page, pageSize, latestOrPublishedVersionOptions, this.currentSearchTerm);
  }
  createClient() {
    return createElsaClient(this.serverUrl);
  }
  render() {
    const allDefinitions = this.workflowDefinitions.items;
    const latestDefinitions = allDefinitions.filter(x => x.isLatest);
    const publishedDefinitions = allDefinitions.filter(x => x.isPublished);
    const totalCount = this.workflowDefinitions.totalCount;
    const i18next = this.i18next;
    const IntlMessage = GetIntlMessage(i18next);
    const basePath = this.basePath;
    const t = this.t;
    return (h("div", null, h("div", { class: "elsa-relative elsa-z-10 elsa-flex-shrink-0 elsa-flex elsa-h-16 elsa-bg-white elsa-border-b elsa-border-gray-200" }, h("div", { class: "elsa-flex-1 elsa-px-4 elsa-flex elsa-justify-between sm:elsa-px-6 lg:elsa-px-8" }, h("div", { class: "elsa-flex-1 elsa-flex" }, h("form", { class: "elsa-w-full elsa-flex md:ml-0", onSubmit: e => this.onSearch(e) }, h("label", { htmlFor: "search_field", class: "elsa-sr-only" }, "Search"), h("div", { class: "elsa-relative elsa-w-full elsa-text-gray-400 focus-within:elsa-text-gray-600" }, h("div", { class: "elsa-absolute elsa-inset-y-0 elsa-left-0 elsa-flex elsa-items-center elsa-pointer-events-none" }, h("svg", { class: "elsa-h-5 elsa-w-5", fill: "currentColor", viewBox: "0 0 20 20" }, h("path", { "fill-rule": "evenodd", "clip-rule": "evenodd", d: "M8 4a4 4 0 100 8 4 4 0 000-8zM2 8a6 6 0 1110.89 3.476l4.817 4.817a1 1 0 01-1.414 1.414l-4.816-4.816A6 6 0 012 8z" }))), h("input", { name: "searchTerm", class: "elsa-block elsa-w-full elsa-h-full elsa-pl-8 elsa-pr-3 elsa-py-2 elsa-rounded-md elsa-text-gray-900 elsa-placeholder-gray-500 focus:elsa-placeholder-gray-400 sm:elsa-text-sm elsa-border-0 focus:elsa-outline-none focus:elsa-ring-0", placeholder: 'Search', type: "search" })))))), h("div", { class: "elsa-align-middle elsa-inline-block elsa-min-w-full elsa-border-b elsa-border-gray-200" }, h("table", { class: "elsa-min-w-full" }, h("thead", null, h("tr", { class: "elsa-border-t elsa-border-gray-200" }, h("th", { class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, h("span", { class: "lg:elsa-pl-2" }, h(IntlMessage, { label: "Name" }))), h("th", { class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, h(IntlMessage, { label: "Instances" })), h("th", { class: "hidden md:elsa-table-cell elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-right elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, h(IntlMessage, { label: "LatestVersion" })), h("th", { class: "hidden md:elsa-table-cell elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-right elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, h(IntlMessage, { label: "PublishedVersion" })), h("th", { class: "elsa-pr-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-right elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }))), h("tbody", { class: "elsa-bg-white elsa-divide-y elsa-divide-gray-100" }, latestDefinitions.map(workflowDefinition => {
      const latestVersionNumber = workflowDefinition.version;
      const { isPublished } = workflowDefinition;
      const publishedVersion = isPublished ? workflowDefinition : publishedDefinitions.find(x => x.definitionId == workflowDefinition.definitionId);
      const publishedVersionNumber = !!publishedVersion ? publishedVersion.version : '-';
      let workflowDisplayName = workflowDefinition.displayName;
      if (!workflowDisplayName || workflowDisplayName.trim().length == 0)
        workflowDisplayName = workflowDefinition.name;
      if (!workflowDisplayName || workflowDisplayName.trim().length == 0)
        workflowDisplayName = 'Untitled';
      const editUrl = `${basePath}/workflow-definitions/${workflowDefinition.definitionId}`;
      const instancesUrl = `${basePath}/workflow-instances?workflow=${workflowDefinition.definitionId}`;
      const editIcon = (h("svg", { class: "elsa-h-5 elsa-w-5 elsa-text-gray-500", width: "24", height: "24", viewBox: "0 0 24 24", xmlns: "http://www.w3.org/2000/svg", fill: "none", stroke: "currentColor", "stroke-width": "2", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { d: "M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7" }), h("path", { d: "M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z" })));
      const deleteIcon = (h("svg", { class: "elsa-h-5 elsa-w-5 elsa-text-gray-500", width: "24", height: "24", viewBox: "0 0 24 24", "stroke-width": "2", stroke: "currentColor", fill: "none", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { stroke: "none", d: "M0 0h24v24H0z" }), h("line", { x1: "4", y1: "7", x2: "20", y2: "7" }), h("line", { x1: "10", y1: "11", x2: "10", y2: "17" }), h("line", { x1: "14", y1: "11", x2: "14", y2: "17" }), h("path", { d: "M5 7l1 12a2 2 0 0 0 2 2h8a2 2 0 0 0 2 -2l1 -12" }), h("path", { d: "M9 7v-3a1 1 0 0 1 1 -1h4a1 1 0 0 1 1 1v3" })));
      const publishIcon = (h("svg", { xmlns: "http://www.w3.org/2000/svg", class: "elsa-h-5 elsa-w-5 elsa-text-gray-500", fill: "none", viewBox: "0 0 24 24", stroke: "currentColor" }, h("path", { "stroke-linecap": "round", "stroke-linejoin": "round", "stroke-width": "2", d: "M7 16a4 4 0 01-.88-7.903A5 5 0 1115.9 6L16 6a5 5 0 011 9.9M15 13l-3-3m0 0l-3 3m3-3v12" })));
      const unPublishIcon = (h("svg", { xmlns: "http://www.w3.org/2000/svg", class: "elsa-h-5 elsa-w-5 elsa-text-gray-500", fill: "none", viewBox: "0 0 24 24", stroke: "currentColor" }, h("path", { "stroke-linecap": "round", "stroke-linejoin": "round", "stroke-width": "2", d: "M18.364 18.364A9 9 0 005.636 5.636m12.728 12.728A9 9 0 015.636 5.636m12.728 12.728L5.636 5.636" })));
      return (h("tr", null, h("td", { class: "elsa-px-6 elsa-py-3 elsa-whitespace-no-wrap elsa-text-sm elsa-leading-5 elsa-font-medium elsa-text-gray-900" }, h("div", { class: "elsa-flex elsa-items-center elsa-space-x-3 lg:elsa-pl-2" }, h("elsa-nav-link", { url: editUrl, anchorClass: "elsa-truncate hover:elsa-text-gray-600" }, h("span", null, workflowDisplayName)))), h("td", { class: "elsa-px-6 elsa-py-3 elsa-text-sm elsa-leading-5 elsa-text-gray-500 elsa-font-medium" }, h("div", { class: "elsa-flex elsa-items-center elsa-space-x-3 lg:elsa-pl-2" }, h("elsa-nav-link", { url: instancesUrl, anchorClass: "elsa-truncate hover:elsa-text-gray-600" }, h(IntlMessage, { label: "Instances" })))), h("td", { class: "hidden md:elsa-table-cell elsa-px-6 elsa-py-3 elsa-whitespace-no-wrap elsa-text-sm elsa-leading-5 elsa-text-gray-500 elsa-text-right" }, latestVersionNumber), h("td", { class: "hidden md:elsa-table-cell elsa-px-6 elsa-py-3 elsa-whitespace-no-wrap elsa-text-sm elsa-leading-5 elsa-text-gray-500 elsa-text-right" }, publishedVersionNumber), h("td", { class: "elsa-pr-6" }, h("elsa-context-menu", { history: this.history, menuItems: [
          { text: i18next.t('Edit'), anchorUrl: editUrl, icon: editIcon },
          isPublished ? { text: i18next.t('Unpublish'), clickHandler: e => this.onUnPublishClick(e, workflowDefinition), icon: unPublishIcon } : {
            text: i18next.t('Publish'),
            clickHandler: e => this.onPublishClick(e, workflowDefinition),
            icon: publishIcon
          },
          { text: i18next.t('Delete'), clickHandler: e => this.onDeleteClick(e, workflowDefinition), icon: deleteIcon }
        ] }))));
    }))), h("elsa-pager", { page: this.currentPage, pageSize: this.currentPageSize, totalCount: totalCount, history: this.history, onPaged: this.onPaged, culture: this.culture })), h("elsa-confirm-dialog", { ref: el => this.confirmDialog = el, culture: this.culture })));
  }
};
ElsaWorkflowDefinitionsListScreen.DEFAULT_PAGE_SIZE = 15;
ElsaWorkflowDefinitionsListScreen.MIN_PAGE_SIZE = 5;
ElsaWorkflowDefinitionsListScreen.MAX_PAGE_SIZE = 100;
ElsaWorkflowDefinitionsListScreen.START_PAGE = 0;
Tunnel.injectProps(ElsaWorkflowDefinitionsListScreen, ['serverUrl', 'culture', 'basePath']);
injectHistory(ElsaWorkflowDefinitionsListScreen);

export { ElsaWorkflowDefinitionsListScreen as elsa_workflow_definitions_list_screen };
