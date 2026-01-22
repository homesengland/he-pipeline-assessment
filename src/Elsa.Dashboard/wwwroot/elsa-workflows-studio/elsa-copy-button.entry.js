import { r as registerInstance, h, l as Host } from './index-1542df5c.js';
import './index-892f713d.js';
import './index-1654a48d.js';
import { e as eventBus } from './event-bus-5d6f3774.js';
import { E as EventTypes } from './events-d0aab14a.js';
import './elsa-client-8304c78c.js';
import './fetch-client-f0dc2a52.js';
import './collection-ba09a015.js';
import './_commonjsHelpers-6cb8dacb.js';
import './utils-db96334c.js';
import './cronstrue-37d55fa1.js';

const ElsaCopyButton = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.onCopyClick = async (e) => {
      e.stopPropagation();
      await navigator.clipboard.writeText(this.value);
      await eventBus.emit(EventTypes.ClipboardCopied, this, undefined, 'Data copied to clipboard.');
    };
    this.value = undefined;
  }
  render() {
    return (h(Host, { class: "elsa-align-text-top elsa-inline-block" }, h("button", { type: "button", class: "hover:elsa-text-blue-500 focus:elsa-text-green-600 elsa-pl-0.5", onClick: this.onCopyClick }, h("svg", { xmlns: "http://www.w3.org/2000/svg", class: "elsa-h-4 elsa-w-4", fill: "none", viewBox: "0 0 24 24", stroke: "currentColor" }, h("path", { "stroke-linecap": "round", "stroke-linejoin": "round", "stroke-width": "2", d: "M8 5H6a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2v-1M8 5a2 2 0 002 2h2a2 2 0 002-2M8 5a2 2 0 012-2h2a2 2 0 012 2m0 0h2a2 2 0 012 2v3m2 4H10m0 0l3-3m-3 3l3 3" })))));
  }
};

export { ElsaCopyButton as elsa_copy_button };
