import { r as registerInstance, h } from './index-1542df5c.js';
import { c as collection } from './collection-ba09a015.js';
import './index-892f713d.js';
import './index-1654a48d.js';
import { i as injectHistory } from './index-53e05b34.js';
import { T as Tunnel } from './dashboard-beb9b1e8.js';
import { D as DropdownButtonOrigin } from './models-9655958f.js';
import { c as parseQuery } from './utils-db96334c.js';
import { e as eventBus } from './event-bus-5d6f3774.js';
import { E as EventTypes } from './events-d0aab14a.js';
import { b as createElsaClient } from './elsa-client-8304c78c.js';
import './_commonjsHelpers-6cb8dacb.js';
import './fetch-client-f0dc2a52.js';
import './cronstrue-37d55fa1.js';
import './active-router-16dd3465.js';
import './index-2db7bf78.js';
import './match-path-02f6df12.js';
import './location-utils-6419c2b3.js';

const ElsaWorkflowRegistryListScreen = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.workflowRegistryColumns = {
      data: null
    };
    this.onPaged = async (e) => {
      this.currentPage = e.detail.page;
      await this.loadWorkflowBlueprints();
    };
    this.history = undefined;
    this.serverUrl = undefined;
    this.basePath = undefined;
    this.culture = undefined;
    this.currentProviderName = "ProgrammaticWorkflowProvider";
    this.workflowProviders = [];
    this.workflowBlueprints = { items: [], page: 1, pageSize: 50, totalCount: 0 };
    this.currentPage = 0;
    this.currentPageSize = ElsaWorkflowRegistryListScreen.DEFAULT_PAGE_SIZE;
    this.currentSearchTerm = undefined;
  }
  async componentWillLoad() {
    await this.loadWorkflowProviders();
    if (!!this.history)
      this.applyQueryString(this.history.location.search);
    await this.loadWorkflowBlueprints();
    await eventBus.emit(EventTypes.WorkflowRegistryLoadingColumns, this, this.workflowRegistryColumns);
  }
  connectedCallback() {
    if (!!this.history)
      this.unlistenRouteChanged = this.history.listen(e => this.routeChanged(e));
    eventBus.on(EventTypes.WorkflowUpdated, this.onLoadWorkflowBlueprints);
    eventBus.on(EventTypes.WorkflowRegistryUpdated, this.onLoadWorkflowBlueprints);
  }
  disconnectedCallback() {
    if (!!this.unlistenRouteChanged)
      this.unlistenRouteChanged();
    eventBus.detach(EventTypes.WorkflowUpdated, this.onLoadWorkflowBlueprints);
    eventBus.detach(EventTypes.WorkflowRegistryUpdated, this.onLoadWorkflowBlueprints);
  }
  applyQueryString(queryString) {
    const query = parseQuery(queryString);
    if (!!query.provider)
      this.currentProviderName = query.provider;
    this.currentPage = !!query.page ? parseInt(query.page) : 0;
    this.currentPage = isNaN(this.currentPage) ? ElsaWorkflowRegistryListScreen.START_PAGE : this.currentPage;
    this.currentPageSize = !!query.pageSize ? parseInt(query.pageSize) : ElsaWorkflowRegistryListScreen.DEFAULT_PAGE_SIZE;
    this.currentPageSize = isNaN(this.currentPageSize) ? ElsaWorkflowRegistryListScreen.DEFAULT_PAGE_SIZE : this.currentPageSize;
    this.currentPageSize = Math.max(Math.min(this.currentPageSize, ElsaWorkflowRegistryListScreen.MAX_PAGE_SIZE), ElsaWorkflowRegistryListScreen.MIN_PAGE_SIZE);
  }
  async routeChanged(e) {
    if (!e.pathname.toLowerCase().endsWith('workflow-registry'))
      return;
    this.applyQueryString(e.search);
    await this.loadWorkflowBlueprints();
  }
  async onDisableWorkflowClick(e, workflowBlueprintId) {
    const result = await this.confirmDialog.show('Disable Workflow', 'Are you sure you wish to disable this workflow?');
    if (!result)
      return;
    await this.updateFeature(workflowBlueprintId, 'disabled', 'true');
  }
  async onEnableWorkflowClick(e, workflowBlueprintId) {
    const result = await this.confirmDialog.show('Enable Workflow', 'Are you sure you wish to enable this workflow?');
    if (!result)
      return;
    await this.updateFeature(workflowBlueprintId, 'disabled', 'false');
  }
  async updateFeature(workflowBlueprintId, key, value) {
    const workflowRegistryUpdating = {
      params: [workflowBlueprintId, key, value]
    };
    await eventBus.emit(EventTypes.WorkflowRegistryUpdating, this, workflowRegistryUpdating);
  }
  async onLoadWorkflowBlueprints() {
    await this.loadWorkflowBlueprints();
  }
  async onWorkflowProviderChanged(value) {
    this.currentProviderName = value;
    this.currentPage = 0;
    await this.loadWorkflowBlueprints();
  }
  async loadWorkflowProviders() {
    const elsaClient = await this.createClient();
    this.workflowProviders = await elsaClient.workflowProvidersApi.list();
    this.currentProviderName = this.workflowProviders.length > 0 ? this.workflowProviders[0].name : undefined;
  }
  async loadWorkflowBlueprints() {
    const elsaClient = await this.createClient();
    const page = this.currentPage;
    const pageSize = this.currentPageSize;
    const versionOptions = { isLatest: true };
    const providerName = this.currentProviderName;
    this.workflowBlueprints = await elsaClient.workflowRegistryApi.list(providerName, page, pageSize, versionOptions);
  }
  createClient() {
    return createElsaClient(this.serverUrl);
  }
  render() {
    const workflowBlueprints = this.workflowBlueprints.items;
    const totalCount = this.workflowBlueprints.totalCount;
    const groupings = collection.groupBy(workflowBlueprints, 'id');
    const basePath = this.basePath;
    let headers = this.workflowRegistryColumns.data != null ? this.workflowRegistryColumns.data.headers : [];
    let hasFeatureContextItems = this.workflowRegistryColumns.data != null ? this.workflowRegistryColumns.data.hasContextItems : false;
    const renderFeatureHeader = (item) => {
      return (h("th", { class: "hidden md:elsa-table-cell elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-text-right elsa-uppercase elsa-tracking-wider" }, item[0]));
    };
    const renderFeatureColumn = (item, isWorkflowBlueprintDisabled) => {
      return (h("td", { class: "hidden md:elsa-table-cell elsa-px-6 elsa-py-3 elsa-whitespace-no-wrap elsa-text-sm elsa-leading-5 elsa-text-gray-500 elsa-text-right" }, isWorkflowBlueprintDisabled ? 'No' : 'Yes'));
    };
    const renderContextMenu = (workflowBlueprintId, isWorkflowBlueprintDisabled, history, editUrl, editIcon, enableIcon, disableIcon) => {
      let menuItems = [];
      menuItems = [...menuItems, ...[{ text: 'Edit', anchorUrl: editUrl, icon: editIcon }]];
      if (hasFeatureContextItems) {
        if (isWorkflowBlueprintDisabled)
          menuItems = [...menuItems, ...[{
                text: 'Enable',
                clickHandler: e => this.onEnableWorkflowClick(e, workflowBlueprintId),
                icon: enableIcon
              }]];
        else
          menuItems = [...menuItems, ...[{
                text: 'Disable',
                clickHandler: e => this.onDisableWorkflowClick(e, workflowBlueprintId),
                icon: disableIcon
              }]];
      }
      return (h("td", { class: "elsa-pr-6" }, h("elsa-context-menu", { history: history, menuItems: menuItems })));
    };
    return (h("div", null, h("div", { class: "elsa-p-8 elsa-flex elsa-content-end elsa-justify-right elsa-bg-white elsa-space-x-4" }, h("div", { class: "elsa-flex-shrink-0" }, this.renderWorkflowProviderFilter())), h("div", { class: "elsa-align-middle elsa-inline-block elsa-min-w-full elsa-border-b elsa-border-gray-200" }, h("table", { class: "elsa-min-w-full" }, h("thead", null, h("tr", { class: "elsa-border-t elsa-border-gray-200" }, h("th", { class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-text-left elsa-uppercase elsa-tracking-wider" }, h("span", { class: "lg:elsa-pl-2" }, "Name")), h("th", { class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-text-left elsa-uppercase elsa-tracking-wider" }, "Instances"), h("th", { class: "hidden md:elsa-table-cell elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-text-right elsa-uppercase elsa-tracking-wider" }, "Latest Version"), h("th", { class: "hidden md:elsa-table-cell elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-text-right elsa-uppercase elsa-tracking-wider" }, "Published Version"), headers.map(item => renderFeatureHeader(item)), h("th", { class: "elsa-pr-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-text-left elsa-uppercase elsa-tracking-wider" }))), h("tbody", { class: "elsa-bg-white elsa-divide-y elsa-divide-gray-100" }, collection.map(groupings, group => {
      const versions = collection.orderBy(group, 'version', 'desc');
      const workflowBlueprint = versions[0];
      const latestVersionNumber = workflowBlueprint.version;
      const publishedVersion = versions.find(x => x.isPublished);
      const publishedVersionNumber = !!publishedVersion ? publishedVersion.version : '-';
      let workflowDisplayName = workflowBlueprint.displayName;
      if (!workflowDisplayName || workflowDisplayName.trim().length == 0)
        workflowDisplayName = workflowBlueprint.name;
      if (!workflowDisplayName || workflowDisplayName.trim().length == 0)
        workflowDisplayName = '(Untitled)';
      const editUrl = `${basePath}/workflow-registry/${workflowBlueprint.id}`;
      const instancesUrl = `${basePath}/workflow-instances?workflow=${workflowBlueprint.id}`;
      const editIcon = (h("svg", { class: "elsa-h-5 elsa-w-5 elsa-text-gray-500", width: "24", height: "24", viewBox: "0 0 24 24", xmlns: "http://www.w3.org/2000/svg", fill: "none", stroke: "currentColor", "stroke-width": "2", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { d: "M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7" }), h("path", { d: "M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z" })));
      const enableIcon = (h("svg", { class: "elsa-h-5 elsa-w-5 elsa-text-gray-500", width: "24", height: "24", viewBox: "0 0 24 24", "stroke-width": "2", stroke: "currentColor", fill: "none", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { stroke: "none", d: "M0 0h24v24H0z" }), h("path", { d: "M5 12l5 5l10 -10" })));
      const disableIcon = (h("svg", { class: "elsa-h-5 elsa-w-5 elsa-text-gray-500", width: "24", height: "24", viewBox: "0 0 24 24", "stroke-width": "2", stroke: "currentColor", fill: "none", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { stroke: "none", d: "M0 0h24v24H0z" }), h("circle", { cx: "12", cy: "12", r: "9" }), h("line", { x1: "5.7", y1: "5.7", x2: "18.3", y2: "18.3" })));
      return (h("tr", null, h("td", { class: "elsa-px-6 elsa-py-3 elsa-whitespace-no-wrap elsa-text-sm elsa-leading-5 elsa-font-medium elsa-text-gray-900" }, h("div", { class: "elsa-flex elsa-items-center elsa-space-x-3 lg:elsa-pl-2" }, h("elsa-nav-link", { url: editUrl, anchorClass: "elsa-truncate hover:elsa-text-gray-600" }, h("span", null, workflowDisplayName)))), h("td", { class: "elsa-px-6 elsa-py-3 elsa-text-sm elsa-leading-5 elsa-text-gray-500 elsa-font-medium" }, h("div", { class: "elsa-flex elsa-items-center elsa-space-x-3 lg:elsa-pl-2" }, h("elsa-nav-link", { url: instancesUrl, anchorClass: "elsa-truncate hover:elsa-text-gray-600" }, "Instances"))), h("td", { class: "hidden md:elsa-table-cell elsa-px-6 elsa-py-3 elsa-whitespace-no-wrap elsa-text-sm elsa-leading-5 elsa-text-gray-500 elsa-text-right" }, latestVersionNumber), h("td", { class: "hidden md:elsa-table-cell elsa-px-6 elsa-py-3 elsa-whitespace-no-wrap elsa-text-sm elsa-leading-5 elsa-text-gray-500 elsa-text-right" }, publishedVersionNumber), headers.map(item => renderFeatureColumn(item, workflowBlueprint.isDisabled)), renderContextMenu(workflowBlueprint.id, workflowBlueprint.isDisabled, this.history, editUrl, editIcon, enableIcon, disableIcon)));
    }))), h("elsa-pager", { page: this.currentPage, pageSize: this.currentPageSize, totalCount: totalCount, history: this.history, onPaged: this.onPaged, culture: this.culture })), h("elsa-confirm-dialog", { ref: el => this.confirmDialog = el })));
  }
  renderWorkflowProviderFilter() {
    const items = this.workflowProviders.map(x => ({ text: x.displayName, value: x.name }));
    const selectedProvider = this.workflowProviders.find(x => x.name == this.currentProviderName);
    const selectedProviderText = (selectedProvider === null || selectedProvider === void 0 ? void 0 : selectedProvider.displayName) || '';
    const renderIcon = function () {
      return h("svg", { class: "elsa-mr-3 elsa-h-5 elsa-w-5 elsa-text-gray-400", fill: "none", viewBox: "0 0 24 24", stroke: "currentColor" }, h("path", { stroke: "none", d: "M0 0h24v24H0z" }), h("rect", { x: "4", y: "4", width: "6", height: "6", rx: "1" }), h("rect", { x: "14", y: "4", width: "6", height: "6", rx: "1" }), h("rect", { x: "4", y: "14", width: "6", height: "6", rx: "1" }), h("rect", { x: "14", y: "14", width: "6", height: "6", rx: "1" }));
    };
    return h("elsa-dropdown-button", { text: selectedProviderText, items: items, icon: renderIcon(), origin: DropdownButtonOrigin.TopRight, onItemSelected: e => this.onWorkflowProviderChanged(e.detail.value) });
  }
};
ElsaWorkflowRegistryListScreen.DEFAULT_PAGE_SIZE = 5;
ElsaWorkflowRegistryListScreen.MIN_PAGE_SIZE = 5;
ElsaWorkflowRegistryListScreen.MAX_PAGE_SIZE = 100;
ElsaWorkflowRegistryListScreen.START_PAGE = 0;
Tunnel.injectProps(ElsaWorkflowRegistryListScreen, ['serverUrl', 'culture', 'basePath']);
injectHistory(ElsaWorkflowRegistryListScreen);

export { ElsaWorkflowRegistryListScreen as elsa_workflow_registry_list_screen };
