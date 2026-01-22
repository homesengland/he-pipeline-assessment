import { r as registerInstance, h, l as Host } from './index-1542df5c.js';

const ElsaTabContent = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.tab = undefined;
    this.active = undefined;
  }
  render() {
    return (h(Host, null, h("div", { class: `${this.active ? '' : 'elsa-hidden'} elsa-overflow-y-auto elsa-h-full` }, h("slot", null))));
  }
};

export { ElsaTabContent as elsa_tab_content };
