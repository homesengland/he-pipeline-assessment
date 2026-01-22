import { r as registerInstance, e as createEvent, h } from './index-1542df5c.js';
import { I as IconProvider } from './icon-provider-7131deff.js';
import { P as PlusIcon } from './plus_icon-368a6257.js';
import { p as parseJson, n as newOptionLetter, m as mapSyntaxToLanguage } from './utils-89b7e981.js';
import { S as SyntaxNames, V as ValidationSyntax, P as PropertyOutputTypes } from './constants-6ea82f24.js';
import { B as BaseComponent, D as DisplayToggle } from './display-toggle-component-5077c8d6.js';
import { S as SortIcon } from './sortable.esm-581578f1.js';
import { T as TrashCanIcon } from './trash-can-639efdf2.js';
import './index-912d1a21.js';

const HeValidationProperty = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.expressionChanged = createEvent(this, "expressionChanged", 7);
    this.supportedSyntaxes = [SyntaxNames.Literal, SyntaxNames.JavaScript];
    this.syntaxSwitchCount = 0;
    this.displayValue = "table-row";
    this.hiddenValue = "none";
    this.activityModel = undefined;
    this.propertyDescriptor = undefined;
    this.propertyModel = undefined;
    this.modelSyntax = SyntaxNames.TextActivityList;
    this.properties = [];
    this.iconProvider = new IconProvider();
    this.keyId = undefined;
    this.dictionary = {};
    this._base = new BaseComponent(this);
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
  onAddElementClick() {
    const textName = newOptionLetter(this._base.IdentifierArray());
    const newTextElement = {
      syntax: SyntaxNames.Literal,
      expressions: { [SyntaxNames.Literal]: '', [ValidationSyntax.ValidationRule]: '' },
      type: PropertyOutputTypes.Validation,
      name: textName
    };
    this.properties = [...this.properties, newTextElement];
    this.updatePropertyModel();
  }
  onHandleDelete(textActivity) {
    this.properties = this.properties.filter(x => x != textActivity);
    this.updatePropertyModel();
  }
  onToggleOptions(index) {
    this._toggle.onToggleDisplay(index);
  }
  render() {
    const textElements = this.properties;
    const json = JSON.stringify(textElements, null, 2);
    const renderCaseEditor = (validationRule, index) => {
      const errorMessageSyntax = validationRule.syntax;
      const ruleExpression = validationRule.expressions[ValidationSyntax.ValidationRule];
      const errorMessageExpression = validationRule.expressions[validationRule.syntax];
      const ruleLanguage = mapSyntaxToLanguage(SyntaxNames.JavaScript);
      const errorLanguage = mapSyntaxToLanguage(errorMessageSyntax);
      const conditionEditorHeight = "2.75em";
      let ruleExpressionEditor = null;
      ruleExpressionEditor = ruleExpressionEditor;
      let errorMessageExpressionEditor = null;
      let colWidth = "100%";
      let textContext = {
        activityTypeName: this.activityModel.type,
        propertyName: this.propertyDescriptor.name
      };
      return (h("tbody", { key: this.keyId }, h("tr", null, h("th", { class: "sortablejs-custom-handle" }, h(SortIcon, { options: this.iconProvider.getOptions() })), h("td", null), h("td", { class: "elsa-pt-1 elsa-pr-2 elsa-text-right" }, h("button", { type: "button", onClick: () => this.onHandleDelete(validationRule), class: "elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none" }, h(TrashCanIcon, { options: this.iconProvider.getOptions() })))), h("tr", null, h("th", { class: "elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12" }, "Validation Rule"), h("td", { class: "elsa-py-2 pl-5", colSpan: 2, style: { width: colWidth } }, h("div", { class: "elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm" }, h("he-expression-editor", { key: `expression-editor-${index}-${this.syntaxSwitchCount}`, ref: el => ruleExpressionEditor = el, expression: ruleExpression, language: ruleLanguage, context: textContext, "single-line": false, editorHeight: "2.75em", padding: "elsa-pt-1.5 elsa-pl-1 elsa-pr-28", onExpressionChanged: e => this._base.CustomUpdateExpression(e, validationRule, ValidationSyntax.ValidationRule) })), h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, "The validation rule to evaluate for this question.  This must be written in Javascript, and will default to \"true\" if left blank."), h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, "Rules that evaluate as true will pass validation, and rules that evaluate as false will fail, and display the validation message below."))), h("tr", null, h("th", { class: "elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12" }, "Validation Message"), h("td", { class: "elsa-py-2 pl-5", colSpan: 2, style: { width: colWidth } }, h("div", { class: "elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm" }, h("he-expression-editor", { key: `expression-editor-${index}-${this.syntaxSwitchCount}`, ref: el => errorMessageExpressionEditor = el, expression: errorMessageExpression, language: errorLanguage, "single-line": false, editorHeight: conditionEditorHeight, padding: "elsa-pt-1.5 elsa-pl-1 elsa-pr-28", onExpressionChanged: e => this._base.CustomUpdateExpression(e, validationRule, errorMessageSyntax) }), h("div", { class: "elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center elsa-select" }, h("select", { onChange: e => this._base.UpdateSyntax(e, validationRule, errorMessageExpressionEditor), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md" }, this.supportedSyntaxes.map(supportedSyntax => {
        const selected = supportedSyntax == errorMessageSyntax;
        return h("option", { selected: selected }, supportedSyntax);
      })))), h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, "The error message to display for this question in the front end, if the validation rule returns \"false\".")))));
    };
    const context = {
      activityTypeName: this.activityModel.type,
      propertyName: this.propertyDescriptor.name
    };
    return (h("div", null, h("he-multi-expression-editor", { ref: el => this.multiExpressionEditor = el, label: this.propertyDescriptor.label, defaultSyntax: SyntaxNames.Json, supportedSyntaxes: [SyntaxNames.Json], context: context, expressions: { 'Json': json }, "editor-height": "20rem", onExpressionChanged: e => this.onMultiExpressionEditorValueChanged(e), onSyntaxChanged: e => this.onMultiExpressionEditorSyntaxChanged(e) }, h("table", { class: "elsa-min-w-full elsa-divide-y elsa-divide-gray-200 elsa-table-striped", ref: el => (this.container = el) }, textElements.map(renderCaseEditor)), h("button", { type: "button", onClick: () => this.onAddElementClick(), class: "elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2" }, h(PlusIcon, { options: this.iconProvider.getOptions() }), "Add Validation Rule"))));
  }
};

export { HeValidationProperty as he_validation_property };
