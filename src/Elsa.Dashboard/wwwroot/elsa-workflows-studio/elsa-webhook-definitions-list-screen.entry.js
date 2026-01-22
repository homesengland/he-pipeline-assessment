import { r as registerInstance, h } from './index-1542df5c.js';
import { c as collection } from './collection-ba09a015.js';
import { c as createElsaWebhooksClient } from './elsa-client-38cb59dc.js';
import { T as Tunnel } from './dashboard-beb9b1e8.js';
import './_commonjsHelpers-6cb8dacb.js';
import './fetch-client-f0dc2a52.js';
import './event-bus-5d6f3774.js';
import './events-d0aab14a.js';
import './index-2db7bf78.js';

const ElsaWebhookDefinitionsListScreen = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.history = undefined;
    this.serverUrl = undefined;
    this.basePath = undefined;
    this.culture = undefined;
    this.webhookDefinitions = { items: [], page: 1, pageSize: 50, totalCount: 0 };
  }
  async componentWillLoad() {
    await this.loadWebhookDefinitions();
  }
  async onDeleteClick(e, webhookDefinition) {
    const result = await this.confirmDialog.show('Delete Webhook Definition', 'Are you sure you wish to permanently delete this webhook?');
    if (!result)
      return;
    const elsaClient = await createElsaWebhooksClient(this.serverUrl);
    await elsaClient.webhookDefinitionsApi.delete(webhookDefinition.id);
    await this.loadWebhookDefinitions();
  }
  async loadWebhookDefinitions() {
    const elsaClient = await createElsaWebhooksClient(this.serverUrl);
    const page = 0;
    const pageSize = 50;
    this.webhookDefinitions = await elsaClient.webhookDefinitionsApi.list(page, pageSize);
  }
  render() {
    const webhookDefinitions = this.webhookDefinitions;
    const list = collection.orderBy(webhookDefinitions, 'name');
    const basePath = this.basePath;
    return (h("div", null, h("div", { class: "elsa-align-middle elsa-inline-block elsa-min-w-full elsa-border-b elsa-border-gray-200" }, h("table", { class: "elsa-min-w-full" }, h("thead", null, h("tr", { class: "elsa-border-t elsa-border-gray-200" }, h("th", { class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, h("span", { class: "lg:elsa-pl-2" }, "Name")), h("th", { class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, "Path"), h("th", { class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, "Payload Type Name"), h("th", { class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, "Enabled"), h("th", { class: "elsa-pr-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-right elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }))), h("tbody", { class: "elsa-bg-white elsa-divide-y elsa-divide-gray-100" }, collection.map(list, item => {
      const webhookDefinition = item;
      let webhookDisplayName = webhookDefinition.name;
      if (!webhookDisplayName || webhookDisplayName.trim().length == 0)
        webhookDisplayName = webhookDefinition.name;
      if (!webhookDisplayName || webhookDisplayName.trim().length == 0)
        webhookDisplayName = 'Untitled';
      const editUrl = `${basePath}/webhook-definitions/${webhookDefinition.id}`;
      const editIcon = (h("svg", { class: "elsa-h-5 elsa-w-5 elsa-text-gray-500", width: "24", height: "24", viewBox: "0 0 24 24", xmlns: "http://www.w3.org/2000/svg", fill: "none", stroke: "currentColor", "stroke-width": "2", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { d: "M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7" }), h("path", { d: "M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z" })));
      const deleteIcon = (h("svg", { class: "elsa-h-5 elsa-w-5 elsa-text-gray-500", width: "24", height: "24", viewBox: "0 0 24 24", "stroke-width": "2", stroke: "currentColor", fill: "none", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { stroke: "none", d: "M0 0h24v24H0z" }), h("line", { x1: "4", y1: "7", x2: "20", y2: "7" }), h("line", { x1: "10", y1: "11", x2: "10", y2: "17" }), h("line", { x1: "14", y1: "11", x2: "14", y2: "17" }), h("path", { d: "M5 7l1 12a2 2 0 0 0 2 2h8a2 2 0 0 0 2 -2l1 -12" }), h("path", { d: "M9 7v-3a1 1 0 0 1 1 -1h4a1 1 0 0 1 1 1v3" })));
      return (h("tr", null, h("td", { class: "elsa-px-6 elsa-py-3 elsa-whitespace-no-wrap elsa-text-sm elsa-leading-5 elsa-font-medium elsa-text-gray-900" }, h("div", { class: "elsa-flex elsa-items-center elsa-space-x-3 lg:elsa-pl-2" }, h("elsa-nav-link", { url: editUrl, anchorClass: "elsa-truncate hover:elsa-text-gray-600" }, h("span", null, webhookDisplayName)))), h("td", { class: "elsa-px-6 elsa-py-3 elsa-text-sm elsa-leading-5 elsa-text-gray-500 elsa-font-medium" }, h("div", { class: "elsa-flex elsa-items-center elsa-space-x-3 lg:elsa-pl-2" }, webhookDefinition.path)), h("td", { class: "elsa-px-6 elsa-py-3 elsa-text-sm elsa-leading-5 elsa-text-gray-500 elsa-font-medium" }, h("div", { class: "elsa-flex elsa-items-center elsa-space-x-3 lg:elsa-pl-2" }, webhookDefinition.payloadTypeName)), h("td", { class: "elsa-px-6 elsa-py-3 elsa-text-sm elsa-leading-5 elsa-text-gray-500 elsa-font-medium" }, h("div", { class: "elsa-flex elsa-items-center elsa-space-x-3 lg:elsa-pl-2" }, true == webhookDefinition.isEnabled ? 'Yes' : 'No')), h("td", { class: "elsa-pr-6" }, h("elsa-context-menu", { history: this.history, menuItems: [
          { text: 'Edit', anchorUrl: editUrl, icon: editIcon },
          { text: 'Delete', clickHandler: e => this.onDeleteClick(e, webhookDefinition), icon: deleteIcon }
        ] }))));
    })))), h("elsa-confirm-dialog", { ref: el => this.confirmDialog = el })));
  }
};
Tunnel.injectProps(ElsaWebhookDefinitionsListScreen, ['serverUrl', 'culture', 'basePath']);

export { ElsaWebhookDefinitionsListScreen as elsa_webhook_definitions_list_screen };
