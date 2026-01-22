import { r as registerInstance, h, l as Host } from './index-1542df5c.js';

const ElsaTabHeader = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.tab = undefined;
    this.active = undefined;
  }
  render() {
    const className = this.active ? 'elsa-border-blue-500 elsa-text-blue-600' : 'elsa-border-transparent elsa-text-gray-500 hover:elsa-text-gray-700 hover:elsa-border-gray-300';
    return (h(Host, null, h("div", { class: `${className} elsa-cursor-pointer elsa-whitespace-nowrap elsa-py-4 elsa-px-1 elsa-border-b-2 elsa-font-medium elsa-text-sm` }, h("slot", null))));
  }
};

export { ElsaTabHeader as elsa_tab_header };
