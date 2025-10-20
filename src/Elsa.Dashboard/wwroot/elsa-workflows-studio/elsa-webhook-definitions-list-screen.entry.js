import { r as registerInstance, h } from './index-CL6j2ec2.js';
import { c as collectionExports } from './collection-B4sYCr2r.js';
import { c as createElsaWebhooksClient } from './elsa-client-DrY-Mv8P.js';
import { T as Tunnel } from './dashboard-DaK-DIvX.js';
import './_commonjsHelpers-Cf5sKic0.js';
import './fetch-client-1OcjQcrw.js';
import './event-bus-axQqcjdg.js';
import './events-CpKc8CLe.js';
import './index-C-8L13GY.js';

const ElsaWebhookDefinitionsListScreen = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
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
        const list = collectionExports.orderBy(webhookDefinitions, 'name');
        const basePath = this.basePath;
        return (h("div", { key: '4e3e1bf4a1e6d9eb2b9463b8178a99d33265920b' }, h("div", { key: '1438489b41413e9c55317ea7f74c802cf0fc6cb1', class: "elsa-align-middle elsa-inline-block elsa-min-w-full elsa-border-b elsa-border-gray-200" }, h("table", { key: '03ceab40d28befa8695e878cc4ad85959aa05f8a', class: "elsa-min-w-full" }, h("thead", { key: '81d8c2fe73653071da9c9f8a82ab7d06ac18080b' }, h("tr", { key: 'f335190d209bf9911d9d2e8617c9d132abca7279', class: "elsa-border-t elsa-border-gray-200" }, h("th", { key: 'fa419872ee5d9dbc1831a9f4551d0a590545a701', class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, h("span", { key: '8d1c2c824df5d402381cccbc5d8746e98d47d73f', class: "lg:elsa-pl-2" }, "Name")), h("th", { key: '295d63abe66dfd587a7c050dd26d76736c7fc2d2', class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, "Path"), h("th", { key: 'bcfd3edb005f78329e89062c8244e92cf524bf06', class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, "Payload Type Name"), h("th", { key: 'e80d6605e2ccaa8dc9222a873b388e6d0ff98220', class: "elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }, "Enabled"), h("th", { key: '43b5e7b2228f743fd7e78a3a91b99de13283bd4a', class: "elsa-pr-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-right elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-uppercase elsa-tracking-wider" }))), h("tbody", { key: '171b35d2483a41ab61249d4db6e72c42d7cf3f56', class: "elsa-bg-white elsa-divide-y elsa-divide-gray-100" }, collectionExports.map(list, item => {
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
        })))), h("elsa-confirm-dialog", { key: '19bc092b79ab737e61982d95e87a893b8a9a10c5', ref: el => this.confirmDialog = el })));
    }
};
Tunnel.injectProps(ElsaWebhookDefinitionsListScreen, ['serverUrl', 'culture', 'basePath']);

export { ElsaWebhookDefinitionsListScreen as elsa_webhook_definitions_list_screen };
//# sourceMappingURL=elsa-webhook-definitions-list-screen.entry.esm.js.map
