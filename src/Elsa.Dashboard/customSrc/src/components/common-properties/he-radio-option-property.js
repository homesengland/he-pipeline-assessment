var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, h, Event, State, Prop } from '@stencil/core';
import { mapSyntaxToLanguage, parseJson, newOptionLetter } from "../../utils/utils";
import { IconProvider } from "../providers/icon-provider/icon-provider";
import PlusIcon from '../../icons/plus_icon';
import TrashCanIcon from '../../icons/trash-can';
import { PropertyOutputTypes, RadioOptionsSyntax, SyntaxNames } from '../../constants/constants';
import { SortableComponent } from '../base-component';
import SortIcon from '../../icons/sort_icon';
import { DisplayToggle } from '../display-toggle-component';
let HeRadioOptionProperty = class HeRadioOptionProperty {
    constructor() {
        this.modelSyntax = SyntaxNames.Json;
        this.iconProvider = new IconProvider();
        this.dictionary = {};
        this.switchTextHeight = "";
        this.editorHeight = "2.75em";
        this.supportedSyntaxes = [SyntaxNames.JavaScript, SyntaxNames.Liquid, SyntaxNames.Literal];
        this.syntaxSwitchCount = 0;
        this.displayValue = "table-row";
        this.hiddenValue = "none";
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
    onDefaultSyntaxValueChanged(e) {
        this.properties = e.detail;
    }
    onAddOptionClick() {
        const optionName = newOptionLetter(this._base.IdentifierArray());
        const newOption = { name: optionName, syntax: SyntaxNames.Literal, expressions: { [SyntaxNames.Literal]: '', [RadioOptionsSyntax.PrePopulated]: 'false' }, type: PropertyOutputTypes.Radio };
        this.properties = [...this.properties, newOption];
        this._base.updatePropertyModel();
    }
    onDeleteOptionClick(switchCase) {
        this.properties = this.properties.filter(x => x != switchCase);
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
    onToggleOptions(index) {
        this._toggle.onToggleDisplay(index);
    }
    render() {
        const cases = this.properties;
        const supportedSyntaxes = this.supportedSyntaxes;
        const json = JSON.stringify(cases, null, 2);
        const renderCaseEditor = (radioOption, index) => {
            var _a;
            const expression = radioOption.expressions[radioOption.syntax];
            const syntax = radioOption.syntax;
            const monacoLanguage = mapSyntaxToLanguage(syntax);
            const prePopulatedSyntax = SyntaxNames.JavaScript;
            const prePopulatedExpression = radioOption.expressions[RadioOptionsSyntax.PrePopulated];
            const prePopulatedLanguage = mapSyntaxToLanguage(prePopulatedSyntax);
            let expressionEditor = null;
            let prePopulatedExpressionEditor = null;
            let colWidth = "100%";
            const optionsDisplay = (_a = this._toggle.component.dictionary[index]) !== null && _a !== void 0 ? _a : "none";
            return (h("tbody", { key: this.keyId },
                h("tr", null,
                    h("th", { class: "sortablejs-custom-handle" },
                        h(SortIcon, { options: this.iconProvider.getOptions() })),
                    h("td", null),
                    h("td", { class: "elsa-pt-1 elsa-pr-2 elsa-text-right" },
                        h("button", { type: "button", onClick: () => this.onDeleteOptionClick(radioOption), class: "elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none" },
                            h(TrashCanIcon, { options: this.iconProvider.getOptions() })))),
                h("tr", null,
                    h("th", { class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12" }, "Identifier"),
                    h("td", { class: "elsa-py-2", colSpan: 2, style: { width: colWidth } },
                        h("input", { type: "text", value: radioOption.name, onChange: e => this._base.UpdateName(e, radioOption), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" }))),
                h("tr", null,
                    h("th", { class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12" }, "Answer"),
                    h("td", { class: "elsa-py-2", colSpan: 2, style: { width: colWidth } },
                        h("div", { class: "elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm elsa-text-box" },
                            h("he-expression-editor", { key: `expression-editor-${index}-${this.syntaxSwitchCount}-${this.keyId}`, ref: el => expressionEditor = el, expression: expression, language: monacoLanguage, "single-line": false, editorHeight: this.editorHeight, padding: "elsa-pt-1.5 elsa-pl-1 elsa-pr-28", onExpressionChanged: e => this._base.CustomUpdateExpression(e, radioOption, radioOption.syntax) }),
                            h("div", { class: "elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center elsa-select" },
                                h("select", { onChange: e => this._base.UpdateSyntax(e, radioOption, expressionEditor), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md elsa-select" }, supportedSyntaxes.map(supportedSyntax => {
                                    const selected = supportedSyntax == syntax;
                                    return h("option", { selected: selected }, supportedSyntax);
                                })))))),
                h("tr", { onClick: () => this.onToggleOptions(index) },
                    h("th", { class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-text-left elsa-tracking-wider elsa-w-2/12", colSpan: 3, style: { cursor: "zoom-in" } }, " Options"),
                    h("td", null),
                    h("td", null)),
                h("tr", { style: { display: optionsDisplay } },
                    h("th", { class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12" }, "Pre Populated"),
                    h("td", { class: "elsa-py-2 pl-5", style: { width: colWidth } },
                        h("div", { class: "elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm elsa-text-box" },
                            h("he-expression-editor", { key: `expression-editor-${index}-${this.syntaxSwitchCount}-${this.keyId}`, ref: el => prePopulatedExpressionEditor = el, expression: prePopulatedExpression, language: prePopulatedLanguage, "single-line": false, editorHeight: "2.75em", padding: "elsa-pt-1.5 elsa-pl-1 elsa-pr-28", onExpressionChanged: e => this._base.CustomUpdateExpression(e, radioOption, RadioOptionsSyntax.PrePopulated) }),
                            h("div", { class: "elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center elsa-select" },
                                h("select", { onChange: e => this._base.UpdateSyntax(e, radioOption, prePopulatedExpressionEditor), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md elsa-select" }, this.supportedSyntaxes.filter(x => x == SyntaxNames.JavaScript).map(supportedSyntax => {
                                    const selected = supportedSyntax == SyntaxNames.JavaScript;
                                    return h("option", { selected: selected }, supportedSyntax);
                                }))))),
                    h("td", null))));
        };
        const context = {
            activityTypeName: this.activityModel.type,
            propertyName: this.propertyDescriptor.name
        };
        return (h("div", null,
            h("he-multi-expression-editor", { ref: el => this.multiExpressionEditor = el, label: this.propertyDescriptor.label, defaultSyntax: SyntaxNames.Json, supportedSyntaxes: [SyntaxNames.Json], context: context, expressions: { 'Json': json }, "editor-height": "20rem", onExpressionChanged: e => this.onMultiExpressionEditorValueChanged(e), onSyntaxChanged: e => this.onMultiExpressionEditorSyntaxChanged(e) },
                h("table", { class: "elsa-min-w-full elsa-divide-y elsa-divide-gray-200", ref: el => (this.container = el) }, cases.map(renderCaseEditor)),
                h("button", { type: "button", onClick: () => this.onAddOptionClick(), class: "elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2" },
                    h(PlusIcon, { options: this.iconProvider.getOptions() }),
                    "Add Answer"))));
    }
};
__decorate([
    Prop()
], HeRadioOptionProperty.prototype, "activityModel", void 0);
__decorate([
    Prop()
], HeRadioOptionProperty.prototype, "propertyDescriptor", void 0);
__decorate([
    Prop()
], HeRadioOptionProperty.prototype, "propertyModel", void 0);
__decorate([
    Prop()
], HeRadioOptionProperty.prototype, "modelSyntax", void 0);
__decorate([
    State()
], HeRadioOptionProperty.prototype, "keyId", void 0);
__decorate([
    State()
], HeRadioOptionProperty.prototype, "properties", void 0);
__decorate([
    State()
], HeRadioOptionProperty.prototype, "iconProvider", void 0);
__decorate([
    Event()
], HeRadioOptionProperty.prototype, "expressionChanged", void 0);
__decorate([
    State()
], HeRadioOptionProperty.prototype, "dictionary", void 0);
__decorate([
    State()
], HeRadioOptionProperty.prototype, "switchTextHeight", void 0);
__decorate([
    State()
], HeRadioOptionProperty.prototype, "editorHeight", void 0);
HeRadioOptionProperty = __decorate([
    Component({
        tag: 'he-radio-options-property',
        shadow: false,
    })
], HeRadioOptionProperty);
export { HeRadioOptionProperty };
