import { r as registerInstance, h, j as Host } from './index-CL6j2ec2.js';
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

const ElsaCopyButton = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
        this.onCopyClick = async (e) => {
            e.stopPropagation();
            await navigator.clipboard.writeText(this.value);
            await eventBus.emit(EventTypes.ClipboardCopied, this, undefined, 'Data copied to clipboard.');
        };
    }
    render() {
        return (h(Host, { key: '434550fb550e34ed32d88c16f0ab1ae82ed63b24', class: "elsa-align-text-top elsa-inline-block" }, h("button", { key: 'aa4e9d0b09d649b739e6507f237db82deb3188fb', type: "button", class: "hover:elsa-text-blue-500 focus:elsa-text-green-600 elsa-pl-0.5", onClick: this.onCopyClick }, h("svg", { key: '8e6f8f584f931eee8906ef3b630427c045a8670c', xmlns: "http://www.w3.org/2000/svg", class: "elsa-h-4 elsa-w-4", fill: "none", viewBox: "0 0 24 24", stroke: "currentColor" }, h("path", { key: '938cbefdd64ad148c10001226cc432fdc09708bf', "stroke-linecap": "round", "stroke-linejoin": "round", "stroke-width": "2", d: "M8 5H6a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2v-1M8 5a2 2 0 002 2h2a2 2 0 002-2M8 5a2 2 0 012-2h2a2 2 0 012 2m0 0h2a2 2 0 012 2v3m2 4H10m0 0l3-3m-3 3l3 3" })))));
    }
};

export { ElsaCopyButton as elsa_copy_button };
//# sourceMappingURL=elsa-copy-button.entry.esm.js.map
