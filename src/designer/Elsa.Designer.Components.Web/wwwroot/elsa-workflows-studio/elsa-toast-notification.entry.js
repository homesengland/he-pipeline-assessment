import { r as registerInstance, h, k as Host } from './index-ea213ee1.js';
import { e as enter, l as leave } from './index-886428b8.js';

let ElsaToastNotification = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.isVisible = false;
  }
  async show(options) {
    this.isVisible = true;
    enter(this.toast);
    if (options.autoCloseIn)
      setTimeout(async () => await this.hide(), options.autoCloseIn);
    this.title = options.title;
    this.message = options.message;
  }
  async hide() {
    leave(this.toast).then(() => this.isVisible = false);
  }
  render() {
    return this.renderToast();
  }
  renderToast() {
    return (h(Host, { class: { 'hidden': !this.isVisible, 'elsa-block': true } }, h("div", { class: "elsa-fixed elsa-inset-0 elsa-z-20 elsa-flex elsa-items-end elsa-justify-center elsa-px-4 elsa-py-6 elsa-pointer-events-none sm:elsa-p-6 sm:elsa-items-start sm:elsa-justify-end" }, h("div", { ref: el => this.toast = el, "data-transition-enter": "elsa-transform elsa-ease-out elsa-duration-300 elsa-transition", "data-transition-enter-start": "elsa-translate-y-2 elsa-opacity-0 sm:elsa-translate-y-0 sm:elsa-translate-x-2", "data-transition-enter-end": "elsa-translate-y-0 elsa-opacity-100 sm:elsa-translate-x-0", "data-transition-leave": "elsa-transition elsa-ease-in elsa-duration-100", "data-transition-leave-start": "elsa-opacity-0", "data-transition-leave-end": "elsa-opacity-0", class: "elsa-max-w-sm elsa-w-full elsa-bg-white elsa-shadow-lg elsa-rounded-lg elsa-pointer-events-auto elsa-ring-1 elsa-ring-black elsa-ring-opacity-5 elsa-overflow-hidden" }, h("div", { class: "elsa-p-4" }, h("div", { class: "elsa-flex elsa-items-start" }, h("div", { class: "elsa-flex-shrink-0" }, h("svg", { class: "elsa-h-6 elsa-w-6 elsa-text-green-400", fill: "none", viewBox: "0 0 24 24", stroke: "currentColor", "aria-hidden": "true" }, h("path", { "stroke-linecap": "round", "stroke-linejoin": "round", "stroke-width": "2", d: "M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" }))), h("div", { class: "elsa-ml-3 elsa-w-0 elsa-flex-1 elsa-pt-0.5" }, this.renderTitle(), h("p", { class: "elsa-mt-1 elsa-text-sm elsa-text-gray-500" }, this.message)), h("div", { class: "elsa-ml-4 elsa-flex-shrink-0 elsa-flex" }, h("button", { class: "elsa-bg-white elsa-rounded-md elsa-inline-flex elsa-text-gray-400 hover:elsa-text-gray-500 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500" }, h("span", { class: "elsa-sr-only" }, "Close"), h("svg", { class: "elsa-h-5 elsa-w-5", "x-description": "Heroicon name: solid/x", xmlns: "http://www.w3.org/2000/svg", viewBox: "0 0 20 20", fill: "currentColor", "aria-hidden": "true" }, h("path", { "fill-rule": "evenodd", d: "M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z", "clip-rule": "evenodd" }))))))))));
  }
  renderTitle() {
    if (!this.title || this.title.length == 0)
      return undefined;
    return (h("p", { class: "elsa-text-sm elsa-font-medium elsa-text-gray-900" }, this.title));
  }
};

export { ElsaToastNotification as elsa_toast_notification };
