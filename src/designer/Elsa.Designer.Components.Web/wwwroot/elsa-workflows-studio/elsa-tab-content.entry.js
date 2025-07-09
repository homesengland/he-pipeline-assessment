import { r as registerInstance, h, k as Host } from './index-ea213ee1.js';

let ElsaTabContent = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
  }
  render() {
    return (h(Host, null, h("div", { class: `${this.active ? '' : 'elsa-hidden'} elsa-overflow-y-auto elsa-h-full` }, h("slot", null))));
  }
};

export { ElsaTabContent as elsa_tab_content };
