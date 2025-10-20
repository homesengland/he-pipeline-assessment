import { r as registerInstance, h, j as Host } from './index-CL6j2ec2.js';
import { l as leave, e as enter } from './index-jup-zNrU.js';
import './index-fZDMH_YE.js';
import './index-D7wXd6HU.js';
import { e as eventBus } from './event-bus-axQqcjdg.js';
import { E as EventTypes } from './events-CpKc8CLe.js';
import './elsa-client-q6ob5JPZ.js';
import './fetch-client-1OcjQcrw.js';
import './collection-B4sYCr2r.js';
import './_commonjsHelpers-Cf5sKic0.js';
import './utils-C0M_5Llz.js';
import './cronstrue-BvVNjwUa.js';

const ElsaFlyoutPanel = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
        this.expandButtonPosition = 1;
        this.autoExpand = false;
        this.hidden = false;
        this.silent = false;
        this.updateCounter = 0; // This is required, so that the component would update tab click events when tabs change from outside
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
    async componentDidLoad() {
        this.expanded = this.autoExpand;
        this.updateTabs();
    }
    async updateTabs() {
        this.headerTabs = Array.from(this.el.querySelectorAll('elsa-tab-header'));
        this.headerTabs.forEach(element => {
            element.onclick = () => {
                this.selectTab(element.tab);
                if (!this.silent) {
                    eventBus.emit(EventTypes.FlyoutPanelTabSelected, this, element.tab);
                }
            };
        });
        this.contentTabs = Array.from(this.el.querySelectorAll('elsa-tab-content'));
        if (this.headerTabs.length > 0) {
            this.currentTab = this.headerTabs[0].tab;
            await this.selectTab(this.currentTab);
        }
    }
    async componentDidRender() {
        this.updateTabs();
    }
    render() {
        const { hidden, expanded, expandButtonPosition } = this;
        const expandPositionClass = `elsa-right-${16 * (expandButtonPosition - 1) + 12}`;
        const hideOpenToggle = hidden || expanded;
        const hideContents = hidden || !expanded;
        return (h(Host, { key: '624b37c5fabbaa430c4c5ed298643f272250643c' }, h("button", { key: 'd670de0cef04ec76903939df1fda83dd825238dc', type: "button", onClick: this.toggle, class: `${hideOpenToggle ? "elsa-hidden" : expandPositionClass} workflow-settings-button elsa-fixed elsa-top-20 elsa-inline-flex elsa-items-center elsa-p-2 elsa-rounded-full elsa-border elsa-border-transparent elsa-bg-white shadow elsa-text-gray-400 hover:elsa-text-blue-500 focus:elsa-text-blue-500 hover:elsa-ring-2 hover:elsa-ring-offset-2 hover:elsa-ring-blue-500 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-z-10` }, h("svg", { key: '0f8183d27312764780df25a5740633bc5ffc922e', xmlns: "http://www.w3.org/2000/svg", class: "elsa-h-8 elsa-w-8", fill: "none", viewBox: "0 0 24 24", stroke: "currentColor" }, h("path", { key: '5c9aedc137c5328ecdc2a1959b56af2f1fa5480b', "stroke-linecap": "round", "stroke-linejoin": "round", "stroke-width": "2", d: "M11 19l-7-7 7-7m8 14l-7-7 7-7" }))), h("section", { key: '134390e42446696b296542725ff76bef1adaa1d9', class: `${hideContents ? 'elsa-hidden' : ''} elsa-fixed elsa-top-4 elsa-right-0 elsa-bottom-0 elsa-overflow-hidden`, "aria-labelledby": "slide-over-title", role: "dialog", "aria-modal": "true" }, h("div", { key: 'fab8107edede11f2ef20840882838687eedae694', class: "elsa-absolute elsa-inset-0 elsa-overflow-hidden" }, h("div", { key: 'b500bfc758b87d296d46b108d9b6b61c857d2ba0', class: "elsa-absolute elsa-inset-0", "aria-hidden": "true" }), h("div", { key: '17f02260c1b953c2a23d8120d672c78a08262f4e', class: "elsa-fixed elsa-top-20 elsa-inset-y-0 elsa-right-2 elsa-bottom-2 max-elsa-w-full elsa-flex" }, h("div", { key: '1d19adf77ece863afc54f2613e1491682df381bc', ref: el => this.el = el, "data-transition-enter": "elsa-transform elsa-transition elsa-ease-in-out elsa-duration-300 sm:elsa-duration-700", "data-transition-enter-start": "elsa-translate-x-full", "data-transition-enter-end": "elsa-translate-x-0", "data-transition-leave": "elsa-transform elsa-transition elsa-ease-in-out elsa-duration-300 sm:elsa-duration-700", "data-transition-leave-start": "elsa-translate-x-0", "data-transition-leave-end": "elsa-translate-x-full", class: "elsa-w-screen elsa-max-w-lg elsa-h-full " }, h("button", { key: 'f454bd7b6cf5bb13dbfc0e5031de4870c04ac211', type: "button", onClick: this.toggle, class: `${this.autoExpand ? 'elsa-hidden' : ''} workflow-settings-button elsa-absolute elsa-left-2 elsa-inline-flex elsa-items-center elsa-p-2 elsa-rounded-full elsa-border elsa-border-transparent elsa-bg-white shadow elsa-text-gray-400 hover:elsa-text-blue-500 focus:elsa-text-blue-500 hover:elsa-ring-2 hover:elsa-ring-offset-2 hover:elsa-ring-blue-500 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-z-10` }, h("svg", { key: '3d13993a59f3417f5cf1c158457696add47b7aae', xmlns: "http://www.w3.org/2000/svg", class: "elsa-h-8 elsa-w-8", fill: "none", viewBox: "0 0 24 24", stroke: "currentColor" }, h("path", { key: '2d3491127de5010f111d4ed1e05c963a6f74c13d', "stroke-linecap": "round", "stroke-linejoin": "round", "stroke-width": "2", d: "M13 5l7 7-7 7M5 5l7 7-7 7" }))), h("div", { key: '3009ccb2828853991bb48147e3aa8b52b3228bb9', class: "elsa-h-full elsa-flex elsa-flex-col elsa-py-6 elsa-bg-white elsa-shadow-xl elsa-bg-white" }, h("div", { key: '4b07f91b63c6597c8778f79bcdf17ea40872d2c0', class: "elsa-h-full elsa-mt-8 elsa-p-6 elsa-flex elsa-flex-col" }, h("div", { key: '66e9a101f8b5fc27d9c97a7c06f20d1c2d9e756a', class: "elsa-border-b elsa-border-gray-200" }, h("nav", { key: 'd027dc4bc794317e3b94d1731b3b0cc0f17c3460', class: "-elsa-mb-px elsa-flex elsa-space-x-8", "aria-label": "Tabs" }, h("slot", { key: '51e68c8cabbc93f81520752840c3ed0a3ae5cde0', name: "header" }))), h("section", { key: '13b8f3de1c3e2d0a15f0f583fd18af7e99bc9f00', class: "elsa-overflow-auto elsa-h-full" }, h("slot", { key: '3143e8ebc2ad2f74c2fc52c0de3c9a1649998f5f', name: "content" }))))))))));
    }
    async selectTab(tab, expand = false) {
        this.headerTabs.forEach(element => {
            element.active = element.tab === tab;
        });
        this.contentTabs.forEach(element => {
            element.active = element.tab === tab;
        });
        if (expand && !this.expanded) {
            this.expanded = true;
            enter(this.el);
        }
    }
};

export { ElsaFlyoutPanel as elsa_flyout_panel };
//# sourceMappingURL=elsa-flyout-panel.entry.esm.js.map
