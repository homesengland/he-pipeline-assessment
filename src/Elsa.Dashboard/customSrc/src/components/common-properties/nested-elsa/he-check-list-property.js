var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, h, Event, Prop, State } from '@stencil/core';
import { mapSyntaxToLanguage, parseJson, getUniversalUniqueId } from "../../../utils/utils";
import { IconProvider } from "../../providers/icon-provider/icon-provider";
import PlusIcon from '../../../icons/plus_icon';
import TrashCanIcon from '../../../icons/trash-can';
import ExpandIcon from '../../../icons/expand_icon';
import { SyntaxNames } from '../../../constants/constants';
let HEChecklistProperty = 
//Copy of Elsa Checklist
//Copied to allow us control over how the expression editor is displayed.
//Also needed to allow auto-rendering of complex objects.
class HEChecklistProperty {
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
        this.propertyModel.expressions['Switch'] = json;
        this.cases = parsed;
    }
    onMultiExpressionEditorSyntaxChanged(e) {
        e = e;
        this.syntaxSwitchCount++;
    }
    onExpandSwitchArea() {
        this.editorHeight == "2.75em" ? this.editorHeight = "8em" : this.editorHeight = "2.75em";
    }
    componentWillRender() {
        this.keyId = getUniversalUniqueId();
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
                            h("th", { class: "elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-text-right elsa-tracking-wider elsa-w-3/12" }, "Name"),
                            h("th", { class: "elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-text-right elsa-tracking-wider elsa-w-8/12" }, "Expression"),
                            h("th", { class: "elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-text-right elsa-tracking-wider elsa-w-1/12" },
                                h("button", { type: "button", onClick: () => this.onExpandSwitchArea(), class: "elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none" },
                                    h(ExpandIcon, { options: this.iconProvider.getOptions() }))))),
                    h("tbody", null, cases.map(renderCaseEditor))),
                h("button", { type: "button", onClick: () => this.onAddCaseClick(), class: "elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2" },
                    h(PlusIcon, { options: this.iconProvider.getOptions() }),
                    "Add Case"))));
    }
};
__decorate([
    Prop()
], HEChecklistProperty.prototype, "activityModel", void 0);
__decorate([
    Prop()
], HEChecklistProperty.prototype, "propertyDescriptor", void 0);
__decorate([
    Prop()
], HEChecklistProperty.prototype, "propertyModel", void 0);
__decorate([
    Prop()
], HEChecklistProperty.prototype, "keyId", void 0);
__decorate([
    State()
], HEChecklistProperty.prototype, "cases", void 0);
__decorate([
    State()
], HEChecklistProperty.prototype, "iconProvider", void 0);
__decorate([
    Event()
], HEChecklistProperty.prototype, "expressionChanged", void 0);
__decorate([
    State()
], HEChecklistProperty.prototype, "switchTextHeight", void 0);
__decorate([
    State()
], HEChecklistProperty.prototype, "editorHeight", void 0);
HEChecklistProperty = __decorate([
    Component({
        tag: 'he-check-list-property',
        shadow: false,
    })
    //Copy of Elsa Checklist
    //Copied to allow us control over how the expression editor is displayed.
    //Also needed to allow auto-rendering of complex objects.
], HEChecklistProperty);
export { HEChecklistProperty };
