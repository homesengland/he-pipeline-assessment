import { r as registerInstance, e as createEvent, h, l as Host } from './index-1542df5c.js';
import { T as Tunnel } from './dashboard-beb9b1e8.js';
import './index-892f713d.js';
import { h as hooks } from './moment-fe70f3d6.js';
import { b as createElsaClient } from './elsa-client-8304c78c.js';
import './index-2db7bf78.js';
import './event-bus-5d6f3774.js';
import './index-1654a48d.js';
import './events-d0aab14a.js';
import './fetch-client-f0dc2a52.js';
import './utils-db96334c.js';
import './collection-ba09a015.js';
import './_commonjsHelpers-6cb8dacb.js';
import './cronstrue-37d55fa1.js';

const ElsaVersionHistoryPanel = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.versionSelected = createEvent(this, "versionSelected", 7);
    this.deleteVersionClicked = createEvent(this, "deleteVersionClicked", 7);
    this.revertVersionClicked = createEvent(this, "revertVersionClicked", 7);
    this.loadVersions = async () => {
      const client = await createElsaClient(this.serverUrl);
      const workflowDefinitionId = this.workflowDefinition.definitionId;
      this.versions = await client.workflowDefinitionsApi.getVersionHistory(workflowDefinitionId);
    };
    this.onViewVersionClick = (e, version) => {
      e.preventDefault();
      this.versionSelected.emit(version);
    };
    this.onDeleteVersionClick = async (e, version) => {
      e.preventDefault();
      const result = await this.confirmDialog.show('Delete Version', 'Are you sure you wish to permanently delete this version? This operation cannot be undone.');
      if (!result)
        return;
      this.deleteVersionClicked.emit(version);
    };
    this.onRevertVersionClick = (e, version) => {
      e.preventDefault();
      this.revertVersionClicked.emit(version);
    };
    this.workflowDefinition = undefined;
    this.serverUrl = undefined;
    this.versions = [];
  }
  async onWorkflowDefinitionChanged(value) {
    await this.loadVersions();
  }
  async componentWillLoad() {
    await this.loadVersions();
  }
  render() {
    const versions = this.versions;
    const selectedVersion = this.workflowDefinition.version;
    return (h(Host, null, h("dl", { class: "elsa-border-gray-200 elsa-divide-y elsa-divide-gray-200" }, h("div", { class: "elsa-text-sm elsa-font-medium" }, h("div", { class: "elsa-mt-2 elsa-flex elsa-flex-col" }, h("div", { class: "" }, h("div", { class: "elsa-inline-block elsa-min-w-full elsa-py-2 elsa-align-middle" }, h("div", { class: "elsa-shadow-sm elsa-ring-1 elsa-ring-black elsa-ring-opacity-5 md:elsa-rounded-lg" }, h("table", { class: "elsa-min-w-full elsa-divide-y elsa-divide-gray-300" }, h("thead", { class: "elsa-bg-gray-50" }, h("tr", null, h("th", { scope: "col", class: "elsa-px-3 elsa-py-3.5 elsa-text-left elsa-text-sm elsa-font-semibold elsa-text-gray-900" }), h("th", { scope: "col", class: "elsa-py-3.5 elsa-pl-2 elsa-pr-3 elsa-text-left elsa-text-sm elsa-font-semibold elsa-text-gray-900" }, "Version"), h("th", { scope: "col", class: "elsa-px-3 elsa-py-3.5 elsa-text-left elsa-text-sm elsa-font-semibold elsa-text-gray-900" }, "Created"), h("th", { scope: "col", class: "elsa-relative elsa-py-3.5 elsa-pl-3 elsa-pr-4 sm:elsa-pr-6 lg:elsa-pr-8" }, h("span", { class: "elsa-sr-only" }, "View")), h("th", { scope: "col", class: "elsa-relative elsa-py-3.5 elsa-pr-4 sm:elsa-pr-6 lg:elsa-pr-8" }))), h("tbody", { class: "elsa-divide-y elsa-divide-gray-200 elsa-bg-white" }, versions.map(v => {
      const createdAt = hooks(v.createdAt);
      const isSelected = selectedVersion == v.version;
      const rowCssClass = isSelected ? 'elsa-bg-gray-100' : undefined;
      const canDeleteOrRevert = !v.isLatest && !v.isPublished;
      const published = v.isPublished ? (h("div", { title: "Published" }, h("svg", { class: "elsa-h-6 elsa-w-6 elsa-text-green-500", fill: "none", viewBox: "0 0 24 24", stroke: "currentColor" }, h("path", { "stroke-linecap": "round", "stroke-linejoin": "round", "stroke-width": "2", d: "M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" })))) : undefined;
      const deleteIcon = (h("svg", { class: "elsa-h-5 elsa-w-5 elsa-text-gray-500", width: "24", height: "24", viewBox: "0 0 24 24", "stroke-width": "2", stroke: "currentColor", fill: "none", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { stroke: "none", d: "M0 0h24v24H0z" }), h("line", { x1: "4", y1: "7", x2: "20", y2: "7" }), h("line", { x1: "10", y1: "11", x2: "10", y2: "17" }), h("line", { x1: "14", y1: "11", x2: "14", y2: "17" }), h("path", { d: "M5 7l1 12a2 2 0 0 0 2 2h8a2 2 0 0 0 2 -2l1 -12" }), h("path", { d: "M9 7v-3a1 1 0 0 1 1 -1h4a1 1 0 0 1 1 1v3" })));
      const revertIcon = (h("svg", { class: "elsa-h-6 elsa-w-6 elsa-text-gray-500", width: "24", height: "24", viewBox: "0 0 24 24", "stroke-width": "2", stroke: "currentColor", fill: "none", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { stroke: "none", d: "M0 0h24v24H0z" }), h("path", { d: "M9 11l-4 4l4 4m-4 -4h11a4 4 0 0 0 0 -8h-1" })));
      const viewIcon = (h("svg", { class: "elsa-h-6 w-6 elsa-text-gray-500", fill: "none", viewBox: "0 0 24 24", stroke: "currentColor" }, h("path", { "stroke-linecap": "round", "stroke-linejoin": "round", "stroke-width": "2", d: "M15 12a3 3 0 11-6 0 3 3 0 016 0z" }), h("path", { "stroke-linecap": "round", "stroke-linejoin": "round", "stroke-width": "2", d: "M2.458 12C3.732 7.943 7.523 5 12 5c4.478 0 8.268 2.943 9.542 7-1.274 4.057-5.064 7-9.542 7-4.477 0-8.268-2.943-9.542-7z" })));
      let contextMenuItems = [{
          text: 'View',
          icon: viewIcon,
          clickHandler: e => this.onViewVersionClick(e, v)
        }, {
          text: 'Delete',
          icon: deleteIcon,
          clickHandler: e => this.onDeleteVersionClick(e, v)
        },
        {
          text: 'Revert',
          icon: revertIcon,
          clickHandler: e => this.onRevertVersionClick(e, v)
        }];
      return (h("tr", { class: rowCssClass }, h("td", { class: "elsa-whitespace-nowrap elsa-px-3 elsa-py-4 elsa-text-sm elsa-text-gray-500" }, published), h("td", { class: "elsa-whitespace-nowrap elsa-py-4 elsa-pl-2 elsa-pr-3 elsa-text-sm elsa-font-medium elsa-text-gray-900" }, v.version), h("td", { class: "elsa-whitespace-nowrap elsa-px-3 elsa-py-4 elsa-text-sm elsa-text-gray-500" }, createdAt.format('DD-MM-YYYY HH:mm:ss')), h("td", { class: "elsa-relative elsa-whitespace-nowrap elsa-py-4 elsa-pl-3 elsa-pr-4 elsa-text-right elsa-text-sm elsa-font-medium sm:elsa-pr-6 lg:elsa-pr-8" }, h("button", { onClick: e => this.onViewVersionClick(e, v), type: "button", disabled: isSelected, class: "elsa-inline-flex elsa-items-center elsa-rounded-md elsa-border elsa-border-gray-300 elsa-bg-white elsa-px-3 elsa-py-2 elsa-text-sm elsa-font-medium elsa-leading-4 elsa-text-gray-700 elsa-shadow-sm hover:elsa-bg-gray-50 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-blue-500 focus:elsa-ring-offset-2 disabled:elsa-cursor-not-allowed disabled:elsa-opacity-30" }, "View", h("span", { class: "elsa-sr-only" }, ", ", v.version))), h("td", null, v.isPublished || v.isPublished ? undefined : h("elsa-context-menu", { menuItems: contextMenuItems }))));
    }))))))))), h("elsa-confirm-dialog", { ref: el => this.confirmDialog = el })));
  }
  static get watchers() { return {
    "workflowDefinition": ["onWorkflowDefinitionChanged"]
  }; }
};
Tunnel.injectProps(ElsaVersionHistoryPanel, ['serverUrl']);

export { ElsaVersionHistoryPanel as elsa_version_history_panel };
