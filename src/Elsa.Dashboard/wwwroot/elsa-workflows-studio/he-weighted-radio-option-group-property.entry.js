import { r as registerInstance, e as createEvent, h } from './index-1542df5c.js';
import { n as newOptionLetter, p as parseJson, m as mapSyntaxToLanguage } from './utils-89b7e981.js';
import { I as IconProvider } from './icon-provider-7131deff.js';
import { P as PlusIcon } from './plus_icon-368a6257.js';
import { T as TrashCanIcon } from './trash-can-639efdf2.js';
import { S as SyntaxNames, R as RadioOptionsSyntax, P as PropertyOutputTypes, W as WeightedScoringSyntax } from './constants-6ea82f24.js';
import { S as SortableComponent, D as DisplayToggle } from './display-toggle-component-5077c8d6.js';
import { S as SortIcon } from './sortable.esm-581578f1.js';
import './index-912d1a21.js';

const HeWeightedRadioOptionGroupProperty = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.expressionChanged = createEvent(this, "expressionChanged", 7);
    this.displayValue = "table-row";
    this.hiddenValue = "none";
    this.supportedSyntaxes = [SyntaxNames.JavaScript, SyntaxNames.Liquid, SyntaxNames.Literal];
    this.syntaxSwitchCount = 0;
    this.scoreSyntaxSwitchCount = 0;
    this.activityModel = undefined;
    this.propertyModel = undefined;
    this.propertyDescriptor = undefined;
    this.modelSyntax = SyntaxNames.Json;
    this.properties = [];
    this.iconProvider = new IconProvider();
    this.dictionary = {};
    this.switchTextHeight = "";
    this.editorHeight = "2.75em";
    this.keyId = undefined;
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
  onAddAnswerClick() {
    const optionName = newOptionLetter(this._base.IdentifierArray());
    const newAnswer = {
      name: optionName,
      syntax: SyntaxNames.Literal,
      expressions: {
        [SyntaxNames.Literal]: '',
        [RadioOptionsSyntax.PrePopulated]: 'false',
      }, type: PropertyOutputTypes.Radio
    };
    this.properties = [...this.properties, newAnswer];
    this.updatePropertyModel();
  }
  onDeleteOptionClick(switchCase) {
    this.properties = this.properties.filter(x => x != switchCase);
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
    const answers = this.properties;
    const supportedSyntaxes = this.supportedSyntaxes;
    const json = JSON.stringify(answers, null, 2);
    const renderCaseEditor = (radioAnswer, index) => {
      var _a;
      const expression = radioAnswer.expressions[radioAnswer.syntax];
      const syntax = radioAnswer.syntax;
      const monacoLanguage = mapSyntaxToLanguage(syntax);
      const prePopulatedSyntax = SyntaxNames.JavaScript;
      const prePopulatedExpression = radioAnswer.expressions[RadioOptionsSyntax.PrePopulated];
      const scoreExpression = radioAnswer.expressions[RadioOptionsSyntax.Score];
      const prePopulatedLanguage = mapSyntaxToLanguage(prePopulatedSyntax);
      let expressionEditor = null;
      let prePopulatedExpressionEditor = null;
      let scoreExpressionEditor = null;
      let colWidth = "100%";
      const optionsDisplay = (_a = this.dictionary[index]) !== null && _a !== void 0 ? _a : "none";
      return (h("tbody", { key: this.keyId }, h("tr", null, h("th", { class: "sortablejs-custom-handle" }, h(SortIcon, { options: this.iconProvider.getOptions() })), h("td", null), h("td", { class: "elsa-pt-1 elsa-pr-2 elsa-text-right" }, h("button", { type: "button", onClick: () => this.onDeleteOptionClick(radioAnswer), class: "elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none" }, h(TrashCanIcon, { options: this.iconProvider.getOptions() })))), h("tr", { key: `case-${index}` }, h("th", { class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12" }, "Identifier"), h("td", { class: "elsa-py-2 elsa-pr-5", colSpan: 2, style: { width: colWidth } }, h("input", { type: "text", value: radioAnswer.name, onChange: e => this._base.UpdateName(e, radioAnswer), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" }))), h("tr", null, h("th", { class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12" }, "Answer"), h("td", { class: "elsa-py-2 pl-5", colSpan: 2, style: { width: colWidth } }, h("div", { class: "elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm elsa-text-box" }, h("he-expression-editor", { key: `expression-editor-${index}-${this.syntaxSwitchCount}`, ref: el => expressionEditor = el, expression: expression, language: monacoLanguage, "single-line": false, editorHeight: this.editorHeight, padding: "elsa-pt-1.5 elsa-pl-1 elsa-pr-28", onExpressionChanged: e => this._base.CustomUpdateExpression(e, radioAnswer, radioAnswer.syntax) }), h("div", { class: "elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center elsa-select" }, h("select", { onChange: e => this._base.UpdateSyntax(e, radioAnswer, expressionEditor), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md elsa-select" }, supportedSyntaxes.map(supportedSyntax => {
        const selected = supportedSyntax == syntax;
        return h("option", { selected: selected }, supportedSyntax);
      })))))), h("tr", null, h("th", { class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12" }, "Score"), h("td", { class: "elsa-py-2 pl-5", style: { width: colWidth } }, h("div", { class: "elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm elsa-text-box" }, h("he-expression-editor", { key: `expression-editor-${index}-${this.scoreSyntaxSwitchCount}`, ref: el => scoreExpressionEditor = el, expression: scoreExpression, language: monacoLanguage, "single-line": true, editorHeight: this.editorHeight, padding: "elsa-pt-1.5 elsa-pl-1 elsa-pr-28", onExpressionChanged: e => this._base.CustomUpdateExpression(e, radioAnswer, RadioOptionsSyntax.Score) }), h("div", { class: "elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center elsa-select" }, h("select", { onChange: e => this._base.UpdateSyntax(e, radioAnswer, scoreExpressionEditor), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md elsa-select" }, this.supportedSyntaxes.filter(x => x == SyntaxNames.Literal).map(supportedSyntax => {
        const selected = supportedSyntax == SyntaxNames.Literal;
        return h("option", { selected: selected }, supportedSyntax);
      }))))), h("td", null)), h("tr", { onClick: () => this.onToggleOptions(index) }, h("th", { class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-text-left elsa-tracking-wider elsa-w-2/12", colSpan: 3, style: { cursor: "zoom-in" } }, " Options"), h("td", null), h("td", null)), h("tr", { style: { display: optionsDisplay } }, h("th", { class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12" }, "Pre Populated"), h("td", { class: "elsa-py-2 pl-5", style: { width: colWidth } }, h("div", { class: "elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm elsa-text-box" }, h("he-expression-editor", { key: `expression-editor-${index}-${this.syntaxSwitchCount}`, ref: el => prePopulatedExpressionEditor = el, expression: prePopulatedExpression, language: prePopulatedLanguage, "single-line": false, editorHeight: "2.75em", padding: "elsa-pt-1.5 elsa-pl-1 elsa-pr-28", onExpressionChanged: e => this._base.CustomUpdateExpression(e, radioAnswer, RadioOptionsSyntax.PrePopulated) }), h("div", { class: "elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center elsa-select" }, h("select", { onChange: e => this._base.UpdateSyntax(e, radioAnswer, prePopulatedExpressionEditor), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md elsa-select" }, this.supportedSyntaxes.filter(x => x == SyntaxNames.JavaScript).map(supportedSyntax => {
        const selected = supportedSyntax == SyntaxNames.JavaScript;
        return h("option", { selected: selected }, supportedSyntax);
      }))))), h("td", null))));
    };
    const groupName = "Radio group " + this.propertyModel.name + " answers";
    const context = {
      activityTypeName: this.activityModel.type,
      propertyName: groupName
    };
    return (h("div", null, h("br", null), h("div", { class: "elsa-mb-1" }, h("div", { class: "elsa-flex" }, h("div", { class: "elsa-flex-1" }, h("label", { class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700" }, "Group Name")))), h("div", null, h("div", null, h("input", { type: "text", value: this.propertyModel.name, onChange: e => this._base.UpdateName(e, this.propertyModel), class: "focus:elsa-ring-blue-500 focus:elsa-border-bue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" })), h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, "The name of the group of Answers.  Each group name must be unique.")), h("div", { class: "elsa-mb-1" }, h("div", { class: "elsa-flex" }, h("div", { class: "elsa-flex-1" }, h("label", { class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700" }, "Max Group Score")))), h("div", null, h("div", null, h("input", { type: "text", value: this.propertyModel.expressions[WeightedScoringSyntax.MaxGroupScore], onChange: e => this._base.StandardUpdateExpression(e, this.propertyModel, WeightedScoringSyntax.MaxGroupScore), class: "focus:elsa-ring-blue-500 focus:elsa-border-bue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" })), h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, "Override the maximum score that can be achieved by any number of answers in this group, even if their combined sum is greater.")), h("br", null), h("he-multi-expression-editor", { ref: el => this.multiExpressionEditor = el, label: groupName, defaultSyntax: SyntaxNames.Json, supportedSyntaxes: [SyntaxNames.Json], context: context, expressions: { 'Json': json }, "editor-height": "20rem", onExpressionChanged: e => this.onMultiExpressionEditorValueChanged(e), onSyntaxChanged: e => this.onMultiExpressionEditorSyntaxChanged(e) }, h("table", { class: "elsa-min-w-full elsa-divide-y elsa-divide-gray-600" }, answers.map(renderCaseEditor)), h("button", { type: "button", onClick: () => this.onAddAnswerClick(), class: "elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2" }, h(PlusIcon, { options: this.iconProvider.getOptions() }), "Add Answer"))));
  }
};

export { HeWeightedRadioOptionGroupProperty as he_weighted_radio_option_group_property };
