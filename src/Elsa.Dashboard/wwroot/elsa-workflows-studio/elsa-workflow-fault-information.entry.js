import { r as registerInstance, h } from './index-CL6j2ec2.js';
import { h as hooks } from './moment-Bh6YS7CH.js';
import { e as clip } from './utils-C0M_5Llz.js';
import './collection-B4sYCr2r.js';
import './_commonjsHelpers-Cf5sKic0.js';

const ElsaWorkflowFaultInformation = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
    }
    render() {
        if (!this.workflowFault)
            return undefined;
        const renderExceptionMessage = (exception) => {
            return (h("div", null, h("div", { class: "elsa-mb-4" }, h("strong", { class: "elsa-block elsa-font-bold" }, exception.type), exception.message), !!exception.innerException ? h("div", { class: "elsa-ml-4" }, renderExceptionMessage(exception.innerException)) : undefined));
        };
        return [
            h("div", { class: "-elsa-m-3 elsa-p-3 elsa-flex elsa-items-start elsa-rounded-lg hover:elsa-bg-gray-50 elsa-transition elsa-ease-in-out elsa-duration-150" }, h("svg", { class: "elsa-flex-shrink-0 elsa-h-6 elsa-w-6 elsa-text-red-600", viewBox: "0 0 24 24", fill: "none", stroke: "currentColor", "stroke-width": "2", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("circle", { cx: "12", cy: "12", r: "10" }), h("line", { x1: "12", y1: "8", x2: "12", y2: "12" }), h("line", { x1: "12", y1: "16", x2: "12.01", y2: "16" })), h("div", { class: "elsa-ml-4" }, h("p", { class: "elsa-text-base elsa-font-medium elsa-text-gray-900" }, "Fault"), h("p", { class: "elsa-mt-1 elsa-text-sm elsa-text-gray-500" }, renderExceptionMessage(this.workflowFault.exception), h("pre", { class: "elsa-overflow-x-scroll elsa-max-w-md", onClick: e => clip(e.currentTarget) }, JSON.stringify(this.workflowFault, null, 1))))),
            h("a", { href: "#", class: "-elsa-m-3 elsa-p-3 elsa-flex elsa-items-start elsa-rounded-lg hover:elsa-bg-gray-50 elsa-transition elsa-ease-in-out elsa-duration-150" }, h("svg", { class: "elsa-flex-shrink-0 elsa-h-6 elsa-w-6 elsa-text-blue-600", width: "24", height: "24", viewBox: "0 0 24 24", "stroke-width": "2", stroke: "currentColor", fill: "none", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { stroke: "none", d: "M0 0h24v24H0z" }), h("rect", { x: "4", y: "5", width: "16", height: "16", rx: "2" }), h("line", { x1: "16", y1: "3", x2: "16", y2: "7" }), h("line", { x1: "8", y1: "3", x2: "8", y2: "7" }), h("line", { x1: "4", y1: "11", x2: "20", y2: "11" }), h("line", { x1: "11", y1: "15", x2: "12", y2: "15" }), h("line", { x1: "12", y1: "15", x2: "12", y2: "18" })), h("div", { class: "elsa-ml-4" }, h("p", { class: "elsa-text-base elsa-font-medium elsa-text-gray-900" }, "Faulted At"), h("p", { class: "elsa-mt-1 elsa-text-sm elsa-text-gray-500" }, hooks(this.faultedAt).format('DD-MM-YYYY HH:mm:ss')))),
        ];
    }
};

export { ElsaWorkflowFaultInformation as elsa_workflow_fault_information };
//# sourceMappingURL=elsa-workflow-fault-information.entry.esm.js.map
