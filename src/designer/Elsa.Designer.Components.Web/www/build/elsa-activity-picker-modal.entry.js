import { r as registerInstance, h, k as Host } from './index-ea213ee1.js';
import { e as eventBus } from './event-bus-5d6f3774.js';
import { E as EventTypes, A as ActivityTraits } from './index-0f68dbd6.js';
import { a as state } from './store-52e2ea41.js';
import './utils-db96334c.js';
import { A as ActivityIcon } from './activity-icon-41a40887.js';
import './collection-ba09a015.js';
import './_commonjsHelpers-6cb8dacb.js';

let ElsaActivityPickerModal = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.selectedTrait = 7;
    this.selectedCategory = 'All';
    this.categories = [];
    this.filteredActivityDescriptorDisplayContexts = [];
    this.onShowActivityPicker = async () => {
      await this.dialog.show(true);
    };
  }
  connectedCallback() {
    eventBus.on(EventTypes.ShowActivityPicker, this.onShowActivityPicker);
  }
  disconnectedCallback() {
    eventBus.detach(EventTypes.ShowActivityPicker, this.onShowActivityPicker);
  }
  componentWillRender() {
    const activityDescriptors = state.activityDescriptors;
    this.categories = ['All', ...activityDescriptors.map(x => x.category).distinct().sort()];
    const searchText = this.searchText ? this.searchText.toLowerCase() : '';
    let filteredActivityDescriptors = activityDescriptors;
    if (searchText.length > 0) {
      filteredActivityDescriptors = filteredActivityDescriptors.filter(x => {
        const category = x.category || '';
        const description = x.description || '';
        const displayName = x.displayName || '';
        const type = x.type || '';
        return category.toLowerCase().indexOf(searchText) >= 0
          || description.toLowerCase().indexOf(searchText) >= 0
          || displayName.toLowerCase().indexOf(searchText) >= 0
          || type.toLowerCase().indexOf(searchText) >= 0;
      });
    }
    else {
      filteredActivityDescriptors = filteredActivityDescriptors.filter(x => (x.traits & this.selectedTrait) == x.traits);
      filteredActivityDescriptors = !this.selectedCategory || this.selectedCategory == 'All' ? filteredActivityDescriptors : filteredActivityDescriptors.filter(x => x.category == this.selectedCategory);
    }
    this.filteredActivityDescriptorDisplayContexts = filteredActivityDescriptors.map(x => {
      const color = (x.traits &= ActivityTraits.Trigger) == ActivityTraits.Trigger ? 'rose' : (x.traits &= ActivityTraits.Job) == ActivityTraits.Job ? 'yellow' : 'sky';
      return {
        activityDescriptor: x,
        activityIcon: h(ActivityIcon, { color: color })
      };
    });
    for (const context of this.filteredActivityDescriptorDisplayContexts)
      eventBus.emit(EventTypes.ActivityDescriptorDisplaying, this, context);
  }
  selectTrait(trait) {
    this.selectedTrait = trait;
  }
  selectCategory(category) {
    this.selectedCategory = category;
  }
  onTraitClick(e, trait) {
    e.preventDefault();
    this.selectTrait(trait);
  }
  onCategoryClick(e, category) {
    e.preventDefault();
    this.selectCategory(category);
  }
  onSearchTextChange(e) {
    this.searchText = e.target.value;
  }
  async onCancelClick() {
    await this.dialog.hide(true);
  }
  async onActivityClick(e, activityDescriptor) {
    e.preventDefault();
    eventBus.emit(EventTypes.ActivityPicked, this, activityDescriptor);
    await this.dialog.hide(false);
  }
  render() {
    const selectedCategoryClass = 'elsa-bg-gray-100 elsa-text-gray-900 elsa-flex';
    const defaultCategoryClass = 'elsa-text-gray-600 hover:elsa-bg-gray-50 hover:elsa-text-gray-900';
    const filteredDisplayContexts = this.filteredActivityDescriptorDisplayContexts;
    const categories = this.categories;
    return (h(Host, { class: "elsa-block" }, h("elsa-modal-dialog", { ref: el => this.dialog = el }, h("div", { slot: "content", class: "elsa-py-8" }, h("div", { class: "elsa-flex" }, h("div", { class: "elsa-px-8" }, h("nav", { class: "elsa-space-y-1", "aria-label": "Sidebar" }, categories.map(category => (h("a", { href: "#", onClick: e => this.onCategoryClick(e, category), class: `${category == this.selectedCategory ? selectedCategoryClass : defaultCategoryClass} elsa-text-gray-600 hover:elsa-bg-gray-50 hover:elsa-text-gray-900 elsa-flex elsa-items-center elsa-px-3 elsa-py-2 elsa-text-sm elsa-font-medium elsa-rounded-md` }, h("span", { class: "elsa-truncate" }, category)))))), h("div", { class: "elsa-flex-1 elsa-pr-8" }, h("div", { class: "elsa-p-0 elsa-mb-6" }, h("div", { class: "elsa-relative elsa-rounded-md elsa-shadow-sm" }, h("div", { class: "elsa-absolute elsa-inset-y-0 elsa-left-0 elsa-pl-3 elsa-flex elsa-items-center elsa-pointer-events-none" }, h("svg", { class: "elsa-h-6 elsa-w-6 elsa-text-gray-400", width: "24", height: "24", viewBox: "0 0 24 24", "stroke-width": "2", stroke: "currentColor", fill: "none", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { stroke: "none", d: "M0 0h24v24H0z" }), h("circle", { cx: "10", cy: "10", r: "7" }), h("line", { x1: "21", y1: "21", x2: "15", y2: "15" }))), h("input", { type: "text", value: this.searchText, onInput: e => this.onSearchTextChange(e), class: "form-input elsa-block elsa-w-full elsa-pl-10 sm:elsa-text-sm sm:elsa-leading-5 focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-rounded-md elsa-border-gray-300", placeholder: "Search activities" }))), h("div", { class: "elsa-max-w-4xl elsa-mx-auto elsa-p-0" }, categories.map(category => {
      const displayContexts = filteredDisplayContexts.filter(x => x.activityDescriptor.category == category);
      if (displayContexts.length == 0)
        return undefined;
      return (h("div", null, h("h2", { class: "elsa-my-4 elsa-text-lg elsa-leading-6 elsa-font-medium" }, category), h("div", { class: "elsa-divide-y elsa-divide-gray-200 sm:elsa-divide-y-0 sm:elsa-grid sm:elsa-grid-cols-2 sm:elsa-gap-px" }, displayContexts.map(displayContext => (h("a", { href: "#", onClick: e => this.onActivityClick(e, displayContext.activityDescriptor), class: "elsa-relative elsa-rounded elsa-group elsa-p-6 focus-within:elsa-ring-2 focus-within:elsa-ring-inset focus-within:elsa-ring-blue-500" }, h("div", { class: "elsa-flex elsa-space-x-10" }, h("div", { class: "elsa-flex elsa-flex-0 elsa-items-center" }, h("div", { innerHTML: displayContext.activityIcon })), h("div", { class: "elsa-flex-1 elsa-mt-2" }, h("h3", { class: "elsa-text-lg elsa-font-medium" }, h("a", { href: "#", class: "focus:elsa-outline-none" }, h("span", { class: "elsa-absolute elsa-inset-0", "aria-hidden": "true" }), displayContext.activityDescriptor.displayName)), h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, displayContext.activityDescriptor.description)))))))));
    }))))), h("div", { slot: "buttons" }, h("div", { class: "elsa-bg-gray-50 elsa-px-4 elsa-py-3 sm:elsa-px-6 sm:elsa-flex sm:elsa-flex-row-reverse" }, h("button", { type: "button", onClick: () => this.onCancelClick(), class: "elsa-mt-3 elsa-w-full elsa-inline-flex elsa-justify-center elsa-rounded-md elsa-border elsa-border-gray-300 elsa-shadow-sm elsa-px-4 elsa-py-2 elsa-bg-white elsa-text-base elsa-font-medium elsa-text-gray-700 hover:elsa-bg-gray-50 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 sm:elsa-mt-0 sm:elsa-ml-3 sm:elsa-w-auto sm:elsa-text-sm" }, "Cancel"))))));
  }
};

export { ElsaActivityPickerModal as elsa_activity_picker_modal };
