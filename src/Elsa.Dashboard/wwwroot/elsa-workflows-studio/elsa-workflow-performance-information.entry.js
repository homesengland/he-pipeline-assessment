import { r as registerInstance, h } from './index-1542df5c.js';
import { h as hooks } from './moment-fe70f3d6.js';
import { d as durationToString } from './utils-db96334c.js';
import './collection-ba09a015.js';
import './_commonjsHelpers-6cb8dacb.js';

const ElsaWorkflowPerformanceInformation = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.activityStats = undefined;
  }
  render() {
    if (!this.activityStats)
      return undefined;
    return [
      h("a", { href: "#", class: "-elsa-m-3 elsa-p-3 elsa-flex elsa-items-start elsa-rounded-lg hover:elsa-bg-gray-50 elsa-transition elsa-ease-in-out elsa-duration-150" }, h("svg", { class: "elsa-flex-shrink-0 elsa-h-6 elsa-w-6 elsa-text-blue-600", width: "24", height: "24", viewBox: "0 0 24 24", "stroke-width": "2", stroke: "currentColor", fill: "none", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { stroke: "none", d: "M0 0h24v24H0z" }), h("rect", { x: "4", y: "5", width: "16", height: "16", rx: "2" }), h("line", { x1: "16", y1: "3", x2: "16", y2: "7" }), h("line", { x1: "8", y1: "3", x2: "8", y2: "7" }), h("line", { x1: "4", y1: "11", x2: "20", y2: "11" }), h("line", { x1: "11", y1: "15", x2: "12", y2: "15" }), h("line", { x1: "12", y1: "15", x2: "12", y2: "18" })), h("div", { class: "elsa-ml-4" }, h("p", { class: "elsa-text-base elsa-font-medium elsa-text-gray-900" }, "Average Execution Time"), h("p", { class: "elsa-mt-1 elsa-text-sm elsa-text-gray-500" }, durationToString(hooks.duration(this.activityStats.averageExecutionTime))))),
      h("a", { href: "#", class: "-m-3 elsa-p-3 elsa-flex elsa-items-start elsa-rounded-lg hover:elsa-bg-gray-50 elsa-transition elsa-ease-in-out elsa-duration-150" }, h("svg", { class: "elsa-flex-shrink-0 elsa-h-6 elsa-w-6 elsa-text-green-500", width: "24", height: "24", viewBox: "0 0 24 24", "stroke-width": "2", stroke: "currentColor", fill: "none", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { stroke: "none", d: "M0 0h24v24H0z" }), h("circle", { cx: "12", cy: "12", r: "9" }), h("polyline", { points: "12 7 12 12 15 15" })), h("div", { class: "elsa-ml-4" }, h("p", { class: "elsa-text-base elsa-font-medium elsa-text-gray-900" }, "Fastest Execution Time"), h("p", { class: "elsa-mt-1 elsa-text-sm elsa-text-gray-500" }, durationToString(hooks.duration(this.activityStats.fastestExecutionTime))))),
      h("a", { href: "#", class: "-elsa-m-3 elsa-p-3 elsa-flex elsa-items-start elsa-rounded-lg hover:elsa-bg-gray-50 elsa-transition elsa-ease-in-out elsa-duration-150" }, h("svg", { class: "elsa-flex-shrink-0 elsa-h-6 elsa-w-6 elsa-text-yellow-500", width: "24", height: "24", viewBox: "0 0 24 24", "stroke-width": "2", stroke: "currentColor", fill: "none", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { stroke: "none", d: "M0 0h24v24H0z" }), h("circle", { cx: "12", cy: "12", r: "9" }), h("polyline", { points: "12 7 12 12 15 15" })), h("div", { class: "elsa-ml-4" }, h("p", { class: "elsa-text-base elsa-font-medium elsa-text-gray-900" }, "Slowest Execution Time"), h("p", { class: "elsa-mt-1 elsa-text-sm elsa-text-gray-500" }, durationToString(hooks.duration(this.activityStats.slowestExecutionTime))))),
      h("a", { href: "#", class: "-elsa-m-3 elsa-p-3 elsa-flex elsa-items-start elsa-rounded-lg hover:elsa-bg-gray-50 elsa-transition elsa-ease-in-out elsa-duration-150" }, h("svg", { class: "elsa-flex-shrink-0 elsa-h-6 elsa-w-6 elsa-text-blue-600", width: "24", height: "24", viewBox: "0 0 24 24", "stroke-width": "2", stroke: "currentColor", fill: "none", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { stroke: "none", d: "M0 0h24v24H0z" }), h("rect", { x: "4", y: "5", width: "16", height: "16", rx: "2" }), h("line", { x1: "16", y1: "3", x2: "16", y2: "7" }), h("line", { x1: "8", y1: "3", x2: "8", y2: "7" }), h("line", { x1: "4", y1: "11", x2: "20", y2: "11" }), h("line", { x1: "11", y1: "15", x2: "12", y2: "15" }), h("line", { x1: "12", y1: "15", x2: "12", y2: "18" })), h("div", { class: "elsa-ml-4" }, h("p", { class: "elsa-text-base elsa-font-medium elsa-text-gray-900" }, "Last Executed At"), h("p", { class: "elsa-mt-1 elsa-text-sm elsa-text-gray-500" }, hooks(this.activityStats.lastExecutedAt).format('DD-MM-YYYY HH:mm:ss'))))
    ];
  }
};

export { ElsaWorkflowPerformanceInformation as elsa_workflow_performance_information };
