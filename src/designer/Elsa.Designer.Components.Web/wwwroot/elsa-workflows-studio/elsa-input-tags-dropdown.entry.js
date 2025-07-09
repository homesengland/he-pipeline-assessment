import { r as registerInstance, e as createEvent, h } from './index-ea213ee1.js';

let ElsaInputTagsDropdown = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.valueChanged = createEvent(this, "valueChanged", 7);
    this.placeHolder = 'Add tag';
    this.values = [];
    this.dropdownValues = [];
    this.currentValues = [];
  }
  valuesChangedHandler(newValue) {
    let values = [];
    const dropdownValues = this.dropdownValues || [];
    if (!!newValue) {
      newValue.forEach(value => {
        dropdownValues.forEach(tag => {
          if (value === tag.value) {
            values.push(tag);
          }
        });
      });
    }
    this.currentValues = values || [];
  }
  componentWillLoad() {
    const dropdownValues = this.dropdownValues || [];
    let values = [];
    this.values.forEach(value => {
      dropdownValues.forEach(tag => {
        if (value === tag.value) {
          values.push(tag);
        }
      });
    });
    this.currentValues = values;
  }
  async onTagSelected(e) {
    e.preventDefault();
    const input = e.target;
    const currentTag = {
      text: input.options[input.selectedIndex].text.trim(),
      value: input.value
    };
    if (currentTag.value.length == 0)
      return;
    const values = [...this.currentValues];
    values.push(currentTag);
    this.currentValues = values.distinct();
    input.value = "Add";
    await this.valueChanged.emit(values);
  }
  async onDeleteTagClick(e, currentTag) {
    e.preventDefault();
    this.currentValues = this.currentValues.filter(tag => tag.value !== currentTag.value);
    await this.valueChanged.emit(this.currentValues);
  }
  render() {
    let values = this.currentValues || [];
    let dropdownItems = this.dropdownValues.filter(x => values.findIndex(y => y.value === x.value) < 0);
    if (!Array.isArray(values))
      values = [];
    const valuesJson = JSON.stringify(values.map(tag => tag.value));
    return (h("div", { class: "elsa-py-2 elsa-px-3 elsa-bg-white elsa-shadow-sm elsa-border elsa-border-gray-300 elsa-rounded-md" }, values.map(tag => (h("a", { href: "#", onClick: e => this.onDeleteTagClick(e, tag), class: "elsa-inline-block elsa-text-xs elsa-bg-blue-400 elsa-text-white elsa-py-2 elsa-px-3 elsa-mr-1 elsa-mb-1 rounded" }, h("input", { type: "hidden", value: tag.value }), h("span", null, tag.text), h("span", { class: "elsa-text-white hover:elsa-text-white elsa-ml-1" }, "\u00D7")))), h("select", { id: this.fieldId, class: "elsa-inline-block elsa-text-xs elsa-py-2 elsa-px-3 elsa-mr-1 elsa-mb-1 elsa-pr-8 elsa-border-gray-300 focus:elsa-outline-none focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-rounded", onChange: (e) => this.onTagSelected(e) }, h("option", { value: "Add", disabled: true, selected: true }, this.placeHolder), dropdownItems.map(tag => (h("option", { value: tag.value }, tag.text)))), h("input", { type: "hidden", name: this.fieldName, value: valuesJson })));
  }
  static get watchers() { return {
    "values": ["valuesChangedHandler"]
  }; }
};

export { ElsaInputTagsDropdown as elsa_input_tags_dropdown };
