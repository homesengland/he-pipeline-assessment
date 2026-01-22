import { h } from './index-1542df5c.js';

// Supports hierarchical field names, e.g. 'foo.bar.baz`, which will map to e.g. { foo: { bar: { baz: ''} } }.
class FormContext {
  constructor(model, updater) {
    this.model = model;
    this.updater = updater;
  }
}
function textInput(context, fieldName, label, value, hint, fieldId, readonlyField) {
  fieldId = fieldId || fieldName;
  return (h("div", null,
    h("label", { htmlFor: fieldName, class: "block elsa-text-sm elsa-font-medium elsa-text-gray-700" }, label),
    h("div", { class: "elsa-mt-1" }, readonlyField ? h("input", { type: "text", readonly: true, id: fieldId, name: fieldName, value: value, class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" })
      : h("input", { type: "text", id: fieldId, name: fieldName, value: value, onChange: e => onTextInputChange(e, context), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" })),
    hint && hint.length > 0 ? h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, hint) : undefined));
}
function checkBox(context, fieldName, label, checked, hint, fieldId) {
  fieldId = fieldId || fieldName;
  return (h("div", { class: "elsa-relative elsa-flex elsa-items-start" },
    h("div", { class: "elsa-flex elsa-items-center elsa-h-5" },
      h("input", { id: fieldId, name: fieldName, type: "checkbox", value: "true", checked: checked, onChange: e => onCheckBoxChange(e, context), class: "focus:elsa-ring-blue-500 elsa-h-4 elsa-w-4 elsa-text-blue-600 elsa-border-gray-300 rounded" })),
    h("div", { class: "elsa-ml-3 elsa-text-sm" },
      h("label", { htmlFor: fieldId, class: "elsa-font-medium elsa-text-gray-700" }, label),
      hint && hint.length > 0 ? h("p", { class: "elsa-text-gray-500" }, hint) : undefined)));
}
function textArea(context, fieldName, label, value, hint, fieldId) {
  fieldId = fieldId || fieldName;
  return (h("div", null,
    h("label", { htmlFor: fieldName, class: "block elsa-text-sm elsa-font-medium elsa-text-gray-700" }, label),
    h("div", { class: "elsa-mt-1" },
      h("textarea", { id: fieldId, name: fieldName, value: value, onChange: e => onTextAreaChange(e, context), rows: 3, class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" })),
    hint && hint.length > 0 ? h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, hint) : undefined));
}
function selectField(context, fieldName, label, value, options, hint, fieldId) {
  fieldId = fieldId || fieldName;
  return (h("div", null,
    h("label", { htmlFor: fieldName, class: "block elsa-text-sm elsa-font-medium elsa-text-gray-700" }, label),
    h("div", { class: "elsa-mt-1" },
      h("select", { id: fieldId, name: fieldName, onChange: e => onSelectChange(e, context), class: "block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md" }, options.map(item => {
        const selected = item.value === value;
        return h("option", { value: item.value, selected: selected }, item.text);
      }))),
    hint && hint.length > 0 ? h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, hint) : undefined));
}
function section(title, subTitle) {
  return h("div", null,
    h("h3", { class: "elsa-text-lg elsa-leading-6 elsa-font-medium elsa-text-gray-900" }, title),
    !!subTitle ? (h("p", { class: "elsa-mt-1 elsa-max-w-2xl elsa-text-sm elsa-text-gray-500" }, subTitle))
      : undefined);
}
function updateField(context, fieldName, value) {
  const fieldNameHierarchy = fieldName.split('.');
  let clone = Object.assign({}, context.model);
  let current = clone;
  for (const name of fieldNameHierarchy.slice(0, fieldNameHierarchy.length - 1)) {
    if (!current[name])
      current[name] = {};
    current = current[name];
  }
  const leafFieldName = fieldNameHierarchy[fieldNameHierarchy.length - 1];
  current[leafFieldName] = value;
  context.model = clone;
  context.updater(clone);
}
function onTextInputChange(e, context) {
  const element = e.target;
  updateField(context, element.name, element.value.trim());
}
function onTextAreaChange(e, context) {
  const element = e.target;
  updateField(context, element.name, element.value.trim());
}
function onCheckBoxChange(e, context) {
  const element = e.target;
  updateField(context, element.name, element.checked);
}
function onSelectChange(e, context) {
  const element = e.target;
  updateField(context, element.name, element.value.trim());
}

export { FormContext as F, section as a, textArea as b, checkBox as c, selectField as s, textInput as t };
