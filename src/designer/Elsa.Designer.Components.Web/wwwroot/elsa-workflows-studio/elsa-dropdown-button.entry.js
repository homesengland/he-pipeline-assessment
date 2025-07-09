import { r as registerInstance, e as createEvent, h } from './index-ea213ee1.js';
import { l as leave, t as toggle } from './index-886428b8.js';
import { D as DropdownButtonOrigin } from './models-9655958f.js';

const elsaDropdownButtonCss = "";

let ElsaContextMenu = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.itemSelected = createEvent(this, "itemSelected", 7);
    this.btnClass = " elsa-w-full elsa-bg-white elsa-border elsa-border-gray-300 elsa-rounded-md elsa-shadow-sm elsa-px-4 elsa-py-2 elsa-inline-flex elsa-justify-center elsa-text-sm elsa-font-medium elsa-text-gray-700 hover:elsa-bg-gray-50 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500";
    this.origin = DropdownButtonOrigin.TopLeft;
    this.items = [];
  }
  onWindowClicked(event) {
    const target = event.target;
    if (!this.element.contains(target))
      this.closeContextMenu();
  }
  closeContextMenu() {
    if (!!this.contextMenu)
      leave(this.contextMenu);
  }
  toggleMenu() {
    if (!!this.contextMenu)
      toggle(this.contextMenu);
  }
  getOriginClass() {
    switch (this.origin) {
      case DropdownButtonOrigin.TopLeft:
        return `elsa-left-0 elsa-origin-top-left`;
      case DropdownButtonOrigin.TopRight:
      default:
        return 'elsa-right-0 elsa-origin-top-right';
    }
  }
  async onItemClick(e, menuItem) {
    e.preventDefault();
    this.itemSelected.emit(menuItem);
    this.closeContextMenu();
  }
  render() {
    return (h("div", { class: "elsa-relative", ref: el => this.element = el }, h("button", { onClick: e => this.toggleMenu(), type: "button", class: this.btnClass, "aria-haspopup": "true", "aria-expanded": "false" }, this.renderIcon(), this.text, h("svg", { class: "elsa-ml-2.5 -elsa-elsa-mr-1.5 elsa-h-5 elsa-w-5 elsa-text-gray-400", "x-description": "Heroicon name: chevron-down", xmlns: "http://www.w3.org/2000/svg", viewBox: "0 0 20 20", fill: "currentColor", "aria-hidden": "true" }, h("path", { "fill-rule": "evenodd", d: "M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z", "clip-rule": "evenodd" }))), this.renderMenu()));
  }
  renderMenu() {
    if (this.items.length == 0)
      return;
    const originClass = this.getOriginClass();
    return h("div", { ref: el => this.contextMenu = el, "data-transition-enter": "elsa-transition elsa-ease-out elsa-duration-100", "data-transition-enter-start": "elsa-transform elsa-opacity-0 elsa-scale-95", "data-transition-leave": "elsa-transition elsa-ease-in elsa-duration-75", "data-transition-leave-start": "elsa-transform elsa-opacity-100 elsa-scale-100", "data-transition-leave-end": "elsa-transform elsa-opacity-0 elsa-scale-95", class: `hidden ${originClass} elsa-z-10 elsa-absolute elsa-mt-2 elsa-w-56 elsa-rounded-md elsa-shadow-lg elsa-bg-white elsa-ring-1 elsa-ring-black elsa-ring-opacity-5` }, h("div", { class: "elsa-py-1", role: "menu", "aria-orientation": "vertical" }, this.renderItems()));
  }
  renderItems() {
    return this.items.map(item => {
      const selectedCssClass = item.isSelected ? "elsa-bg-blue-600 hover:elsa-bg-blue-700 elsa-text-white" : "hover:elsa-bg-gray-100 elsa-text-gray-700 hover:elsa-text-gray-900";
      return !!item.url
        ? h("stencil-route-link", { onClick: e => this.closeContextMenu(), url: item.url, anchorClass: `elsa-block elsa-px-4 elsa-py-2 elsa-text-sm ${selectedCssClass} elsa-cursor-pointer`, role: "menuitem" }, item.text)
        : h("a", { href: "#", onClick: e => this.onItemClick(e, item), class: `elsa-block elsa-px-4 elsa-py-2 elsa-text-sm ${selectedCssClass}`, role: "menuitem" }, item.text);
    });
  }
  renderIcon() {
    if (!this.icon)
      return;
    return this.icon;
  }
};
ElsaContextMenu.style = elsaDropdownButtonCss;

export { ElsaContextMenu as elsa_dropdown_button };
