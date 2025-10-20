import { r as registerInstance, h, j as Host } from './index-CL6j2ec2.js';

const ElsaTabHeader = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
    }
    render() {
        const className = this.active ? 'elsa-border-blue-500 elsa-text-blue-600' : 'elsa-border-transparent elsa-text-gray-500 hover:elsa-text-gray-700 hover:elsa-border-gray-300';
        return (h(Host, { key: '471b83376dd55dc57bc84b427d19a47a5cc0ccae' }, h("div", { key: '2565b0dd7d3682f41d3400c5b92a4a3d986d3bbc', class: `${className} elsa-cursor-pointer elsa-whitespace-nowrap elsa-py-4 elsa-px-1 elsa-border-b-2 elsa-font-medium elsa-text-sm` }, h("slot", { key: '20a6a4d253ec6071766b17f4416ebf4295d9eb3f' }))));
    }
};

export { ElsaTabHeader as elsa_tab_header };
//# sourceMappingURL=elsa-tab-header.entry.esm.js.map
