import { r as registerInstance, h, j as Host } from './index-CL6j2ec2.js';
import { e as eventBus } from './event-bus-axQqcjdg.js';
import { A as ActivityTraits } from './index-D7wXd6HU.js';
import { a as state } from './store-B_H_ZDGs.js';
import './utils-C0M_5Llz.js';
import { A as ActivityIcon } from './activity-icon-nWEHGFat.js';
import { E as EventTypes } from './events-CpKc8CLe.js';
import './collection-B4sYCr2r.js';
import './_commonjsHelpers-Cf5sKic0.js';

const ElsaActivityPickerModal = class {
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
        return (h(Host, { key: 'e94571bca38b047c35147efdade9de9e928fe3b4', class: "elsa-block" }, h("elsa-modal-dialog", { key: 'eaecd6d4d906dae9cc91c2a8c2479f39782ace4f', ref: el => this.dialog = el }, h("div", { key: '121fa1b5c089359edef8bca6765b497eda9c241b', slot: "content", class: "elsa-py-8" }, h("div", { key: 'c95e8631bde986544afc108725ec49ec6ec475e3', class: "elsa-flex" }, h("div", { key: '77c52f6943a6ab5941adcd88ea97fb3877770cdf', class: "elsa-px-8" }, h("nav", { key: '1adc32d18c616decb8b5ff176389b233a6279bf6', class: "elsa-space-y-1", "aria-label": "Sidebar" }, categories.map(category => (h("a", { href: "#", onClick: e => this.onCategoryClick(e, category), class: `${category == this.selectedCategory ? selectedCategoryClass : defaultCategoryClass} elsa-text-gray-600 hover:elsa-bg-gray-50 hover:elsa-text-gray-900 elsa-flex elsa-items-center elsa-px-3 elsa-py-2 elsa-text-sm elsa-font-medium elsa-rounded-md` }, h("span", { class: "elsa-truncate" }, category)))))), h("div", { key: '68f5be7bc03f996b1349a8b22ee76d797bd5262f', class: "elsa-flex-1 elsa-pr-8" }, h("div", { key: '077936bc674a4b84a0706707f3579bdbf13a9749', class: "elsa-p-0 elsa-mb-6" }, h("div", { key: '39ee6d95011121dfb2c067c3657a1164855b342d', class: "elsa-relative elsa-rounded-md elsa-shadow-sm" }, h("div", { key: '33a5b66b6bc5e97a27dde13edf1c4a8014eebc96', class: "elsa-absolute elsa-inset-y-0 elsa-left-0 elsa-pl-3 elsa-flex elsa-items-center elsa-pointer-events-none" }, h("svg", { key: 'a54210a27c5c1d9b115f77227c62da401b90fb29', class: "elsa-h-6 elsa-w-6 elsa-text-gray-400", width: "24", height: "24", viewBox: "0 0 24 24", "stroke-width": "2", stroke: "currentColor", fill: "none", "stroke-linecap": "round", "stroke-linejoin": "round" }, h("path", { key: '8cb63e26461c1326cb1cfdccf25cbb61a155673b', stroke: "none", d: "M0 0h24v24H0z" }), h("circle", { key: '2ca501e8fbc3ba48cf3fba3f2459f21641af8f05', cx: "10", cy: "10", r: "7" }), h("line", { key: '7e3fdf3aafb12f4c1e49ca29c0670409b0695128', x1: "21", y1: "21", x2: "15", y2: "15" }))), h("input", { key: '4ec4ef756fdd4967ff7f44d7062e28e888438784', type: "text", value: this.searchText, onInput: e => this.onSearchTextChange(e), class: "form-input elsa-block elsa-w-full elsa-pl-10 sm:elsa-text-sm sm:elsa-leading-5 focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-rounded-md elsa-border-gray-300", placeholder: "Search activities" }))), h("div", { key: '90824d04d6125db396bf0127c2debc6797b826cd', class: "elsa-max-w-4xl elsa-mx-auto elsa-p-0" }, categories.map(category => {
            const displayContexts = filteredDisplayContexts.filter(x => x.activityDescriptor.category == category);
            if (displayContexts.length == 0)
                return undefined;
            return (h("div", null, h("h2", { class: "elsa-my-4 elsa-text-lg elsa-leading-6 elsa-font-medium" }, category), h("div", { class: "elsa-divide-y elsa-divide-gray-200 sm:elsa-divide-y-0 sm:elsa-grid sm:elsa-grid-cols-2 sm:elsa-gap-px" }, displayContexts.map(displayContext => (h("a", { href: "#", onClick: e => this.onActivityClick(e, displayContext.activityDescriptor), class: "elsa-relative elsa-rounded elsa-group elsa-p-6 focus-within:elsa-ring-2 focus-within:elsa-ring-inset focus-within:elsa-ring-blue-500" }, h("div", { class: "elsa-flex elsa-space-x-10" }, h("div", { class: "elsa-flex elsa-flex-0 elsa-items-center" }, h("div", { innerHTML: displayContext.activityIcon })), h("div", { class: "elsa-flex-1 elsa-mt-2" }, h("h3", { class: "elsa-text-lg elsa-font-medium" }, h("a", { href: "#", class: "focus:elsa-outline-none" }, h("span", { class: "elsa-absolute elsa-inset-0", "aria-hidden": "true" }), displayContext.activityDescriptor.displayName)), h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, displayContext.activityDescriptor.description)))))))));
        }))))), h("div", { key: '0e6ac24aab61e5709b7ebb60b83974ae02db5cbe', slot: "buttons" }, h("div", { key: 'b695ad81daa7e572877e3d3bd572758b3d2500fb', class: "elsa-bg-gray-50 elsa-px-4 elsa-py-3 sm:elsa-px-6 sm:elsa-flex sm:elsa-flex-row-reverse" }, h("button", { key: '5445815244e264e540e3d4465f0c01fc6d2c161a', type: "button", onClick: () => this.onCancelClick(), class: "elsa-mt-3 elsa-w-full elsa-inline-flex elsa-justify-center elsa-rounded-md elsa-border elsa-border-gray-300 elsa-shadow-sm elsa-px-4 elsa-py-2 elsa-bg-white elsa-text-base elsa-font-medium elsa-text-gray-700 hover:elsa-bg-gray-50 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 sm:elsa-mt-0 sm:elsa-ml-3 sm:elsa-w-auto sm:elsa-text-sm" }, "Cancel"))))));
    }
};

export { ElsaActivityPickerModal as elsa_activity_picker_modal };
//# sourceMappingURL=elsa-activity-picker-modal.entry.esm.js.map
