var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, h, Event, Prop, State } from '@stencil/core';
import { mapSyntaxToLanguage, parseJson, newOptionNumber } from "../../../utils/utils";
import { IconProvider } from "../../providers/icon-provider/icon-provider";
import PlusIcon from '../../../icons/plus_icon';
import TrashCanIcon from '../../../icons/trash-can';
import { SyntaxNames } from '../../../constants/constants';
import { getUniversalUniqueId } from '../../../utils/utils';
let HeSwitchCasesProperty = 
//Copy of Elsa Switch Case
//Copied to allow us control over how the expression editor is displayed.
class HeSwitchCasesProperty {
    constructor() {
        this.cases = [];
        this.iconProvider = new IconProvider();
        this.switchTextHeight = "";
        this.editorHeight = "2.75em";
        this.supportedSyntaxes = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
        this.syntaxSwitchCount = 0;
    }
    async componentWillLoad() {
        const propertyModel = this.propertyModel;
        const casesJson = propertyModel.expressions[SyntaxNames.Switch];
        this.cases = parseJson(casesJson) || [];
    }
    async componentWillRender() {
        const propertyModel = this.propertyModel;
        const casesJson = propertyModel.expressions[SyntaxNames.Switch];
        this.cases = parseJson(casesJson) || [];
        this.keyId = getUniversalUniqueId();
    }
    updatePropertyModel() {
        this.propertyModel.expressions[SyntaxNames.Switch] = JSON.stringify(this.cases);
        this.multiExpressionEditor.expressions[SyntaxNames.Json] = JSON.stringify(this.cases, null, 2);
    }
    onDefaultSyntaxValueChanged(e) {
        this.cases = e.detail;
    }
    onAddCaseClick() {
        const caseIds = this.caseIds();
        const id = newOptionNumber(caseIds);
        const caseName = `Case ${id}`;
        const newCase = { name: caseName, syntax: SyntaxNames.JavaScript, expressions: { [SyntaxNames.JavaScript]: '' } };
        this.cases = [...this.cases, newCase];
        this.updatePropertyModel();
    }
    caseIds() {
        let activityIds = [];
        if (this.cases.length > 0) {
            activityIds = this.cases.map(function (v) {
                const caseNumberMatch = v.name.match(/^Case\s(\d+$)/);
                if (caseNumberMatch != null) {
                    return parseInt(caseNumberMatch[1]);
                }
                return 0;
            });
        }
        return activityIds;
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
            return (h("tr", { key: `case-${index}` },
                h("td", { class: "elsa-py-2 elsa-pr-5" },
                    h("input", { type: "text", value: switchCase.name, onChange: e => this.onCaseNameChanged(e, switchCase), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" })),
                h("td", { class: "elsa-py-2 pl-5" },
                    h("div", { class: "elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm elsa-text-box" },
                        h("he-expression-editor", { key: `expression-editor-${index}-${this.syntaxSwitchCount}-${this.keyId}`, ref: el => expressionEditor = el, expression: expression, language: monacoLanguage, "single-line": false, editorHeight: this.editorHeight, padding: "elsa-pt-1.5 elsa-pl-1 elsa-pr-28", onExpressionChanged: e => this.onCaseExpressionChanged(e, switchCase) }),
                        h("div", { class: "elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center elsa-select" },
                            h("select", { onChange: e => this.onCaseSyntaxChanged(e, switchCase, expressionEditor), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md elsa-select" }, supportedSyntaxes.map(supportedSyntax => {
                                const selected = supportedSyntax == syntax;
                                return h("option", { selected: selected }, supportedSyntax);
                            }))))),
                h("td", { class: "elsa-pt-1 elsa-pr-2 elsa-text-right" },
                    h("button", { type: "button", onClick: () => this.onDeleteCaseClick(switchCase), class: "elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none" },
                        h(TrashCanIcon, { options: this.iconProvider.getOptions() })))));
        };
        const context = {
            activityTypeName: this.activityModel.type,
            propertyName: this.propertyDescriptor.name
        };
        return (h("div", null,
            h("he-multi-expression-editor", { ref: el => this.multiExpressionEditor = el, label: this.propertyDescriptor.label, defaultSyntax: SyntaxNames.Json, supportedSyntaxes: [SyntaxNames.Json], context: context, expressions: { 'Json': json }, "editor-height": "20rem", onExpressionChanged: e => this.onMultiExpressionEditorValueChanged(e), onSyntaxChanged: e => this.onMultiExpressionEditorSyntaxChanged(e) },
                h("table", { class: "elsa-min-w-full elsa-divide-y elsa-divide-gray-200" },
                    h("thead", { class: "elsa-bg-gray-50" },
                        h("tr", null,
                            h("th", { class: "elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-3/12" }, "Name"),
                            h("th", { class: "elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-8/12" }, "Expression"),
                            h("th", { class: "elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-text-right elsa-tracking-wider elsa-w-1/12" }))),
                    h("tbody", null, cases.map(renderCaseEditor))),
                h("button", { type: "button", onClick: () => this.onAddCaseClick(), class: "elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2" },
                    h(PlusIcon, { options: this.iconProvider.getOptions() }),
                    "Add Case"))));
    }
};
__decorate([
    Prop()
], HeSwitchCasesProperty.prototype, "activityModel", void 0);
__decorate([
    Prop()
], HeSwitchCasesProperty.prototype, "propertyDescriptor", void 0);
__decorate([
    Prop()
], HeSwitchCasesProperty.prototype, "propertyModel", void 0);
__decorate([
    Prop()
], HeSwitchCasesProperty.prototype, "keyId", void 0);
__decorate([
    State()
], HeSwitchCasesProperty.prototype, "cases", void 0);
__decorate([
    State()
], HeSwitchCasesProperty.prototype, "iconProvider", void 0);
__decorate([
    Event()
], HeSwitchCasesProperty.prototype, "expressionChanged", void 0);
__decorate([
    State()
], HeSwitchCasesProperty.prototype, "switchTextHeight", void 0);
__decorate([
    State()
], HeSwitchCasesProperty.prototype, "editorHeight", void 0);
HeSwitchCasesProperty = __decorate([
    Component({
        tag: 'he-switch-answers-property',
        shadow: false,
    })
    //Copy of Elsa Switch Case
    //Copied to allow us control over how the expression editor is displayed.
], HeSwitchCasesProperty);
export { HeSwitchCasesProperty };
