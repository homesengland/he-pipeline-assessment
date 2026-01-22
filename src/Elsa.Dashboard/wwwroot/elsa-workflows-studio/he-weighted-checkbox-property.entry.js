import { r as registerInstance, e as createEvent, h } from './index-1542df5c.js';
import { n as newOptionLetter, p as parseJson } from './utils-89b7e981.js';
import { I as IconProvider } from './icon-provider-7131deff.js';
import { P as PlusIcon } from './plus_icon-368a6257.js';
import { T as TrashCanIcon } from './trash-can-639efdf2.js';
import { S as SyntaxNames, W as WeightedScoringSyntax, P as PropertyOutputTypes } from './constants-6ea82f24.js';
import { S as SortableComponent, D as DisplayToggle } from './display-toggle-component-5077c8d6.js';
import { S as SortIcon } from './sortable.esm-581578f1.js';
import { M as MinimiseIcon, a as MaximiseIcon } from './minimise_icon-cf342904.js';
import './index-912d1a21.js';

const HeWeightedCheckboxProperty = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.expressionChanged = createEvent(this, "expressionChanged", 7);
    this.supportedSyntaxes = [SyntaxNames.JavaScript, SyntaxNames.Liquid, SyntaxNames.Literal];
    this.syntaxSwitchCount = 0;
    this.scoreSyntaxSwitchCount = 0;
    this.displayValue = "table-row";
    this.hiddenValue = "none";
    this.activityModel = undefined;
    this.propertyDescriptor = undefined;
    this.propertyModel = undefined;
    this.modelSyntax = SyntaxNames.Json;
    this.keyId = undefined;
    this.properties = [];
    this.iconProvider = new IconProvider();
    this.dictionary = {};
    this.switchTextHeight = "";
    this.editorHeight = "2.75em";
    this._base = new SortableComponent(this);
    this._toggle = new DisplayToggle(this);
  }
  async componentWillLoad() {
    this._base.componentWillLoad();
  }
  async componentDidLoad() {
    this._base.componentDidLoad();
  }
  async componentWillRender() {
    this._base.componentWillRender();
  }
  updatePropertyModel() {
    this._base.updatePropertyModel();
  }
  onDefaultSyntaxValueChanged(e) {
    this.properties = e.detail;
  }
  onAddGroupClick() {
    const groupName = newOptionLetter(this._base.IdentifierArray());
    const newGroup = {
      name: groupName,
      syntax: SyntaxNames.Json,
      expressions: {
        [SyntaxNames.Json]: '',
        [WeightedScoringSyntax.GroupArrayScore]: ''
      }, type: PropertyOutputTypes.CheckboxGroup
    };
    this.properties = [...this.properties, newGroup];
    this.updatePropertyModel();
  }
  onDeleteGroupClick(checkboxGroup) {
    this.properties = this.properties.filter(x => x != checkboxGroup);
    this.updatePropertyModel();
  }
  onPropertyExpressionChange(event, property) {
    event = event;
    property = property;
    this.updatePropertyModel();
  }
  onMultiExpressionEditorValueChanged(e) {
    const json = e.detail;
    const parsed = parseJson(json);
    if (!parsed)
      return;
    if (!Array.isArray(parsed))
      return;
    this.propertyModel.expressions[SyntaxNames.Json] = json;
    this.properties = parsed;
  }
  onMultiExpressionEditorSyntaxChanged(e) {
    e = e;
    this.syntaxSwitchCount++;
  }
  onToggleOptions(index) {
    this._toggle.onToggleDisplay(index);
  }
  render() {
    const answerGroups = this.properties;
    const json = JSON.stringify(answerGroups, null, 2);
    const renderCheckboxGroups = (checkboxGroup) => {
      const eventHandler = this.onPropertyExpressionChange.bind(this);
      const groupKey = "group_" + checkboxGroup.name;
      const isMinimised = this.dictionary[groupKey] != null && this.dictionary[groupKey] == this.displayValue;
      let minimiseIconStyle = isMinimised ? this.hiddenValue : this.displayValue;
      let maximiseIconStyle = !isMinimised ? this.hiddenValue : this.displayValue;
      let displayGroupStyle = isMinimised ? this.hiddenValue : "";
      return (h("div", { key: this.keyId }, h("br", null), h("div", { class: "elsa-mb-1" }, h("div", { class: "elsa-flex" }, h("div", { class: "elsa-flex-1 sortablejs-custom-handle" }, h(SortIcon, { options: this.iconProvider.getOptions() })), h("div", { class: "elsa-flex-1 elsa-text-left elsa-mx-auto" }, h("h2", { class: "inline" }, "Group: ", checkboxGroup.name), h("button", { type: "button", onClick: () => this.onToggleOptions(groupKey), class: "elsa-h-5 inline float-right elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none", style: { display: minimiseIconStyle } }, h(MinimiseIcon, { options: this.iconProvider.getOptions() })), h("button", { type: "button", onClick: () => this.onToggleOptions(groupKey), class: "elsa-h-5 float-right inline elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none", style: { display: maximiseIconStyle } }, h(MaximiseIcon, { options: this.iconProvider.getOptions() }))), h("div", { class: "px-3 inline" }), h("div", null, h("button", { type: "button", onClick: () => this.onDeleteGroupClick(checkboxGroup), class: "elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none" }, h(TrashCanIcon, { options: this.iconProvider.getOptions() }))))), h("he-weighted-checkbox-option-group-property", { activityModel: this.activityModel, propertyModel: checkboxGroup, onExpressionChanged: e => eventHandler(e, checkboxGroup), style: { display: displayGroupStyle } }), h("br", null), h("hr", null)));
    };
    const context = {
      activityTypeName: this.activityModel.type,
      propertyName: this.propertyDescriptor.name
    };
    return (h("div", null, h("he-multi-expression-editor", { ref: el => this.multiExpressionEditor = el, label: this.propertyDescriptor.label, defaultSyntax: SyntaxNames.Json, supportedSyntaxes: [SyntaxNames.Json], context: context, expressions: { 'Json': json }, "editor-height": "20rem", onExpressionChanged: e => this.onMultiExpressionEditorValueChanged(e), onSyntaxChanged: e => this.onMultiExpressionEditorSyntaxChanged(e) }, h("hr", null), h("div", { class: "elsa-min-w-full elsa-divide-y elsa-divide-gray-200", ref: el => (this.container = el) }, answerGroups.map(renderCheckboxGroups)), h("button", { type: "button", onClick: () => this.onAddGroupClick(), class: "elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2" }, h(PlusIcon, { options: this.iconProvider.getOptions() }), "Add Answer Group"))));
  }
};

export { HeWeightedCheckboxProperty as he_weighted_checkbox_property };
