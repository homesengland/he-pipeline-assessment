import { r as registerInstance, h } from './index-1542df5c.js';
import { p as parseJson, m as mapSyntaxToLanguage } from './utils-89b7e981.js';
import { I as IconProvider } from './icon-provider-7131deff.js';
import { P as PlusIcon } from './plus_icon-368a6257.js';
import { T as TrashCanIcon } from './trash-can-639efdf2.js';
import { S as SyntaxNames } from './constants-6ea82f24.js';
import './index-912d1a21.js';

const CustomElsaSwitchCasesProperty = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.supportedSyntaxes = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
    this.syntaxSwitchCount = 0;
    this.activityModel = undefined;
    this.propertyDescriptor = undefined;
    this.propertyModel = undefined;
    this.cases = [];
    this.iconProvider = new IconProvider();
    this.switchTextHeight = "";
    this.editorHeight = "2.75em";
  }
  async componentWillLoad() {
    const propertyModel = this.propertyModel;
    const casesJson = propertyModel.expressions['Switch'];
    this.cases = parseJson(casesJson) || [];
  }
  updatePropertyModel() {
    this.propertyModel.expressions['Switch'] = JSON.stringify(this.cases);
    this.multiExpressionEditor.expressions[SyntaxNames.Json] = JSON.stringify(this.cases, null, 2);
  }
  onDefaultSyntaxValueChanged(e) {
    this.cases = e.detail;
  }
  onAddCaseClick() {
    const caseName = `Case ${this.cases.length + 1}`;
    const newCase = { name: caseName, syntax: SyntaxNames.JavaScript, expressions: { [SyntaxNames.JavaScript]: '' } };
    this.cases = [...this.cases, newCase];
    this.updatePropertyModel();
  }
  onDeleteCaseClick(switchCase) {
    this.cases = this.cases.filter(x => x != switchCase);
    this.updatePropertyModel();
  }
  onCaseNameChanged(e, switchCase) {
    switchCase.name = e.currentTarget.value.trim();
    this.updatePropertyModel();
  }
  onCaseExpressionChanged(e, switchCase) {
    switchCase.expressions[switchCase.syntax] = e.detail;
    this.updatePropertyModel();
  }
  onCaseSyntaxChanged(e, switchCase, expressionEditor) {
    const select = e.currentTarget;
    switchCase.syntax = select.value;
    expressionEditor.language = mapSyntaxToLanguage(switchCase.syntax);
    this.updatePropertyModel();
  }
  onMultiExpressionEditorValueChanged(e) {
    const json = e.detail;
    const parsed = parseJson(json);
    if (!parsed)
      return;
    if (!Array.isArray(parsed))
      return;
    this.propertyModel.expressions[SyntaxNames.Switch] = json;
    this.cases = parsed;
  }
  onMultiExpressionEditorSyntaxChanged(e) {
    e = e;
    this.syntaxSwitchCount++;
  }
  render() {
    const cases = this.cases;
    const supportedSyntaxes = this.supportedSyntaxes;
    const json = JSON.stringify(cases, null, 2);
    const renderCaseEditor = (switchCase, index) => {
      const expression = switchCase.expressions[switchCase.syntax];
      const syntax = switchCase.syntax;
      const monacoLanguage = mapSyntaxToLanguage(syntax);
      let expressionEditor = null;
      return (h("tr", { key: `case-${index}` }, h("td", { class: "elsa-py-2 elsa-pr-5" }, h("input", { type: "text", value: switchCase.name, onChange: e => this.onCaseNameChanged(e, switchCase), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" })), h("td", { class: "elsa-py-2 pl-5" }, h("div", { class: "elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm elsa-text-box" }, h("he-expression-editor", { key: `expression-editor-${index}-${this.syntaxSwitchCount}`, ref: el => expressionEditor = el, expression: expression, language: monacoLanguage, "single-line": false, editorHeight: this.editorHeight, padding: "elsa-pt-1.5 elsa-pl-1 elsa-pr-28", serverUrl: "https://localhost:7227", onExpressionChanged: e => this.onCaseExpressionChanged(e, switchCase) }), h("div", { class: "elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center elsa-select" }, h("select", { onChange: e => this.onCaseSyntaxChanged(e, switchCase, expressionEditor), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md elsa-select" }, supportedSyntaxes.map(supportedSyntax => {
        const selected = supportedSyntax == syntax;
        return h("option", { selected: selected }, supportedSyntax);
      }))))), h("td", { class: "elsa-pt-1 elsa-pr-2 elsa-text-right" }, h("button", { type: "button", onClick: () => this.onDeleteCaseClick(switchCase), class: "elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none" }, h(TrashCanIcon, { options: this.iconProvider.getOptions() })))));
    };
    const context = {
      activityTypeName: this.activityModel.type,
      propertyName: this.propertyDescriptor.name
    };
    return (h("div", null, h("elsa-multi-expression-editor", { ref: el => this.multiExpressionEditor = el, label: this.propertyDescriptor.label, defaultSyntax: SyntaxNames.Json, supportedSyntaxes: [SyntaxNames.Json], context: context, expressions: { 'Json': json }, "editor-height": "20rem", onExpressionChanged: e => this.onMultiExpressionEditorValueChanged(e), onSyntaxChanged: e => this.onMultiExpressionEditorSyntaxChanged(e) }, h("table", { class: "elsa-min-w-full elsa-divide-y elsa-divide-gray-200" }, h("thead", { class: "elsa-bg-gray-50" }, h("tr", null, h("th", { class: "elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-3/12" }, "Name"), h("th", { class: "elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-8/12" }, "Expression"), h("th", { class: "elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-text-right elsa-tracking-wider elsa-w-1/12" }))), h("tbody", null, cases.map(renderCaseEditor))), h("button", { type: "button", onClick: () => this.onAddCaseClick(), class: "elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2" }, h(PlusIcon, { options: this.iconProvider.getOptions() }), "Add Case"))));
  }
};

export { CustomElsaSwitchCasesProperty as switch_answers_property };
