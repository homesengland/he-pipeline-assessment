import { r as registerInstance, e as createEvent, h } from './index-1542df5c.js';
import { n as newOptionLetter, p as parseJson, m as mapSyntaxToLanguage } from './utils-89b7e981.js';
import { I as IconProvider } from './icon-provider-7131deff.js';
import { P as PlusIcon } from './plus_icon-368a6257.js';
import { T as TrashCanIcon } from './trash-can-639efdf2.js';
import { S as SyntaxNames, T as TextActivityOptionsSyntax, P as PropertyOutputTypes } from './constants-6ea82f24.js';
import { S as SortableComponent, D as DisplayToggle } from './display-toggle-component-5077c8d6.js';
import { S as SortIcon } from './sortable.esm-581578f1.js';
import { M as MinimiseIcon, a as MaximiseIcon } from './minimise_icon-cf342904.js';
import './index-912d1a21.js';

const HeTextGroupProperty = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.expressionChanged = createEvent(this, "expressionChanged", 7);
    this.supportedSyntaxes = [SyntaxNames.JavaScript, SyntaxNames.Liquid, SyntaxNames.Literal];
    this.syntaxSwitchCount = 0;
    this.scoreSyntaxSwitchCount = 0;
    this.displayValue = "table-row";
    this.hiddenValue = "none";
    this.groupTextButton = "Add Text Group";
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
    //TODO - a little messy and we probably want to look to inject options into this
    //but due to time constraints this should work as an initial check.
    if (this.propertyDescriptor != null && this.propertyDescriptor.name != null) {
      this.groupTextButton = this.propertyDescriptor.name.toLowerCase().includes('guidance')
        ? "Add Guidance" : this.groupTextButton;
    }
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
        [SyntaxNames.GroupedInformationText]: '',
        [TextActivityOptionsSyntax.Condition]: 'true'
      }, type: PropertyOutputTypes.InformationGroup
    };
    this.properties = [...this.properties, newGroup];
    this.updatePropertyModel();
  }
  onDeleteGroupClick(informationGroup) {
    this.properties = this.properties.filter(x => x != informationGroup);
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
  informationTextDescriptor() {
    return {
      uiHint: 'he-text-activity-property',
      isReadOnly: false,
      name: "Information text",
      hint: "",
      label: "Information Text",
      supportedSyntaxes: [SyntaxNames.GroupedInformationText, SyntaxNames.Json],
      defaultSyntax: SyntaxNames.GroupedInformationText,
      considerValuesAsOutcomes: false,
      disableWorkflowProviderSelection: true
    };
  }
  render() {
    const textGroups = this.properties;
    const json = JSON.stringify(textGroups, null, 2);
    const renderCheckboxGroups = (textGroup) => {
      const descriptor = this.informationTextDescriptor();
      const title = textGroup.expressions[TextActivityOptionsSyntax.Title];
      const isCollapsedChecked = textGroup.expressions[TextActivityOptionsSyntax.Collapsed] == 'true';
      const isGuidanceChecked = textGroup.expressions[TextActivityOptionsSyntax.Guidance] == 'true';
      const isBulletsChecked = textGroup.expressions[TextActivityOptionsSyntax.Bulletpoints] == 'true';
      const eventHandler = this.onPropertyExpressionChange.bind(this);
      const groupKey = "group_" + textGroup.name;
      const isMinimised = this.dictionary[groupKey] != null && this.dictionary[groupKey] == this.displayValue;
      let minimiseIconStyle = isMinimised ? this.hiddenValue : this.displayValue;
      let maximiseIconStyle = !isMinimised ? this.hiddenValue : this.displayValue;
      let displayGroupStyle = isMinimised ? this.hiddenValue : "";
      const conditionSyntax = SyntaxNames.JavaScript;
      const conditionLanguage = mapSyntaxToLanguage(conditionSyntax);
      const conditionExpression = textGroup.expressions[TextActivityOptionsSyntax.Condition];
      const conditionEditorHeight = "2.75em";
      let conditionExpressionEditor = null;
      return (h("div", { class: "elsa-border-gray-300 elsa-rounded-md elsa-group-border", key: this.keyId }, h("br", null), h("div", { class: "elsa-mb-1" }, h("div", { class: "elsa-flex" }, h("div", { class: "elsa-flex-1 sortablejs-custom-handle" }, h(SortIcon, { options: this.iconProvider.getOptions() })), h("div", { class: "elsa-flex-1 elsa-text-left elsa-mx-auto" }, h("h2", { class: "inline" }, "Group: ", textGroup.name), h("button", { type: "button", onClick: () => this.onToggleOptions(groupKey), class: "elsa-h-5 inline float-right elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none", style: { display: minimiseIconStyle } }, h(MinimiseIcon, { options: this.iconProvider.getOptions() })), h("button", { type: "button", onClick: () => this.onToggleOptions(groupKey), class: "elsa-h-5 float-right inline elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none", style: { display: maximiseIconStyle } }, h(MaximiseIcon, { options: this.iconProvider.getOptions() }))), h("div", { class: "px-3 inline" }), h("div", null, h("button", { type: "button", onClick: () => this.onDeleteGroupClick(textGroup), class: "elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none" }, h(TrashCanIcon, { options: this.iconProvider.getOptions() })))), h("br", null), h("div", { style: { display: displayGroupStyle } }, h("div", { class: "elsa-mb-1" }, h("div", { class: "elsa-flex" }, h("div", { class: "elsa-flex-1" }, h("label", { class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700" }, "Title")))), h("div", null, h("div", null, h("input", { type: "text", value: title, onChange: e => this._base.StandardUpdateExpression(e, textGroup, TextActivityOptionsSyntax.Title), class: "focus:elsa-ring-blue-500 focus:elsa-border-bue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" })), h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, "The title of the text grouping.  This can be left empty, but if a value is given, it will display as a heading.")), h("br", null), h("div", { class: "elsa-mb-1" }, h("div", { class: "elsa-flex" }, h("div", { class: "elsa-flex-1" }, h("label", { class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700" }, "Display on Page")))), h("div", { class: "elsa-relative" }, h("he-expression-editor", { key: `expression-editor-${this.syntaxSwitchCount}`, ref: el => conditionExpressionEditor = el, expression: conditionExpression, language: conditionLanguage, "single-line": false, editorHeight: conditionEditorHeight, padding: "elsa-pt-1.5 elsa-pl-1 elsa-pr-28", onExpressionChanged: e => this._base.CustomUpdateExpression(e, textGroup, TextActivityOptionsSyntax.Condition) }), h("div", { class: "elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center elsa-select" }, h("select", { onChange: e => this._base.UpdateSyntax(e, textGroup, conditionExpressionEditor), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md" }, this.supportedSyntaxes.filter(x => x == SyntaxNames.JavaScript).map(supportedSyntax => {
        const selected = supportedSyntax == SyntaxNames.JavaScript;
        return h("option", { selected: selected }, supportedSyntax);
      })))), h("br", null), h("div", { class: "elsa-mb-1" }, h("div", { class: "elsa-flex" }, h("div", { class: "elsa-flex-1" }, h("label", { class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700" }, "Is Guidance")))), h("div", null, h("div", null, h("input", { type: "checkbox", checked: isGuidanceChecked, onChange: e => this._base.UpdateCheckbox(e, textGroup, TextActivityOptionsSyntax.Guidance), class: "focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" })), h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, "Should the group of text be wrapped in a guidance block.")), h("br", null), h("div", { class: "elsa-mb-1" }, h("div", { class: "elsa-flex" }, h("div", { class: "elsa-flex-1" }, h("label", { class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700" }, "Is Collapsed")))), h("div", null, h("div", null, h("input", { type: "checkbox", checked: isCollapsedChecked, onChange: e => this._base.UpdateCheckbox(e, textGroup, TextActivityOptionsSyntax.Collapsed), class: "focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" })), h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, "Should the group of text be hidden by default, behind a collapseable element. Users will still be able to see the text by clicking to view the hidden text.")), h("br", null), h("div", { class: "elsa-mb-1" }, h("div", { class: "elsa-flex" }, h("div", { class: "elsa-flex-1" }, h("label", { class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700" }, "Is Bulletpointed")))), h("div", null, h("div", null, h("input", { type: "checkbox", checked: isBulletsChecked, onChange: e => this._base.UpdateCheckbox(e, textGroup, TextActivityOptionsSyntax.Bulletpoints), class: "focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" })), h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, "Each paragraph of text will be displayed as a bullet-point list within this group.")))), h("he-text-activity-property", { activityModel: this.activityModel, propertyModel: textGroup, propertyDescriptor: descriptor, onExpressionChanged: e => eventHandler(e, textGroup), modelSyntax: SyntaxNames.TextActivity, style: { display: displayGroupStyle } }), h("br", null), h("hr", null)));
    };
    const context = {
      activityTypeName: this.activityModel.type,
      propertyName: this.propertyDescriptor.name
    };
    return (h("div", null, h("he-multi-expression-editor", { ref: el => this.multiExpressionEditor = el, label: this.propertyDescriptor.label, hint: this.propertyDescriptor.hint, defaultSyntax: SyntaxNames.Json, supportedSyntaxes: [SyntaxNames.Json], context: context, expressions: { 'Json': json }, "editor-height": "20rem", onExpressionChanged: e => this.onMultiExpressionEditorValueChanged(e), onSyntaxChanged: e => this.onMultiExpressionEditorSyntaxChanged(e) }, h("hr", null), h("div", { class: "elsa-min-w-full elsa-divide-y elsa-divide-gray-200", ref: el => (this.container = el) }, textGroups.map(renderCheckboxGroups)), h("button", { type: "button", onClick: () => this.onAddGroupClick(), class: "elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2" }, h(PlusIcon, { options: this.iconProvider.getOptions() }), "Add Text Group"))));
  }
};

export { HeTextGroupProperty as he_text_group_property };
