var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, h, Event, Prop, State } from '@stencil/core';
import { mapSyntaxToLanguage, parseJson, newOptionLetter } from "../../utils/utils";
import { IconProvider } from "../providers/icon-provider/icon-provider";
import PlusIcon from '../../icons/plus_icon';
import TrashCanIcon from '../../icons/trash-can';
import { DataTableSyntax, PropertyOutputTypes, SyntaxNames } from '../../constants/constants';
import { SortableComponent } from '../base-component';
import SortIcon from '../../icons/sort_icon';
import { DisplayToggle } from '../display-toggle-component';
let HeDataTableProperty = class HeDataTableProperty {
    constructor() {
        this.modelSyntax = SyntaxNames.Json;
        this.properties = [];
        this.iconProvider = new IconProvider();
        this.dictionary = {};
        this.switchTextHeight = "";
        this.editorHeight = "2.75em";
        this.displayValue = "table-row";
        this.hiddenValue = "none";
        this.inputOptions = [];
        this.selectedInputType = "Currency";
        this.supportedSyntaxes = [SyntaxNames.JavaScript, SyntaxNames.Literal];
        this.syntaxSwitchCount = 0;
        this._base = new SortableComponent(this);
        this._toggle = new DisplayToggle(this);
    }
    async componentWillLoad() {
        this._base.componentWillLoad();
        this.inputOptions = ["Currency", "Decimal", "Integer", "Text"];
        if (this.propertyModel.expressions[DataTableSyntax.InputType] == null) {
            this.propertyModel.expressions[DataTableSyntax.InputType] = "Currency";
        }
    }
    async componentDidLoad() {
        this._base.componentDidLoad();
    }
    updatePropertyModel() {
        this._base.updatePropertyModel();
    }
    async componentWillRender() {
        this._base.componentWillRender();
    }
    onDefaultSyntaxValueChanged(e) {
        this.properties = e.detail;
    }
    onAddRowClick() {
        const optionName = newOptionLetter(this._base.IdentifierArray());
        const newOption = { name: optionName, syntax: SyntaxNames.Literal, expressions: { [SyntaxNames.Literal]: '', [DataTableSyntax.Identifier]: optionName }, type: PropertyOutputTypes.TableInput };
        this.properties = [...this.properties, newOption];
        this.updatePropertyModel();
    }
    onDeleteInputClick(input) {
        this.properties = this.properties.filter(x => x != input);
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
    onInputTypeChange(e) {
        this._base.StandardUpdateExpression(e, this.propertyModel, DataTableSyntax.InputType);
        this.selectedInputType = this.propertyModel.expressions[DataTableSyntax.InputType];
    }
    render() {
        const cases = this.properties;
        const supportedSyntaxes = this.supportedSyntaxes;
        const json = JSON.stringify(cases, null, 2);
        const selectedType = this.propertyModel.expressions[DataTableSyntax.InputType];
        const renderCaseEditor = (tableInput, index) => {
            var _a;
            const headerExpression = tableInput.expressions[tableInput.syntax];
            const inputExpression = tableInput.expressions[DataTableSyntax.Input];
            const syntax = tableInput.syntax;
            const monacoLanguage = mapSyntaxToLanguage(syntax);
            const sumTotalColumn = tableInput.expressions[DataTableSyntax.SumTotalColumn] == 'true';
            const readOnly = tableInput.expressions[DataTableSyntax.Readonly] == 'true';
            let expressionEditor = null;
            let colWidth = "100%";
            const optionsDisplay = (_a = this.dictionary[index]) !== null && _a !== void 0 ? _a : "none";
            const sumTotalDisplay = (this.dictionary[index] && this.selectedInputType != "Text") ? this.dictionary[index] : "none";
            return (h("tbody", { key: this.keyId },
                h("tr", null,
                    h("th", { class: "sortablejs-custom-handle" },
                        h(SortIcon, { options: this.iconProvider.getOptions() })),
                    h("td", null),
                    h("td", { class: "elsa-pt-1 elsa-pr-2 elsa-text-right" },
                        h("button", { type: "button", onClick: () => this.onDeleteInputClick(tableInput), class: "elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none" },
                            h(TrashCanIcon, { options: this.iconProvider.getOptions() })))),
                h("tr", { key: `case-${index}` },
                    h("th", { class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12" }, "Identifier"),
                    h("td", { class: "elsa-py-2", colSpan: 2, style: { width: colWidth } },
                        h("input", { type: "text", value: tableInput.expressions[DataTableSyntax.Identifier], onChange: e => this._base.UpdateExpressionFromInput(e, tableInput, DataTableSyntax.Identifier), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" }))),
                h("tr", { key: `case-${index}` },
                    h("th", { class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12" }, "Row Heading"),
                    h("td", { class: "elsa-py-2", colSpan: 2, style: { width: colWidth } },
                        h("div", { class: "elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm elsa-text-box" },
                            h("he-expression-editor", { key: `expression-editor-${index}-${this.syntaxSwitchCount}-${this.keyId}`, ref: el => expressionEditor = el, expression: headerExpression, language: monacoLanguage, "single-line": false, editorHeight: this.editorHeight, padding: "elsa-pt-1.5 elsa-pl-1 elsa-pr-28", onExpressionChanged: e => this._base.CustomUpdateExpression(e, tableInput, tableInput.syntax) }),
                            h("div", { class: "elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center elsa-select" },
                                h("select", { onChange: e => this._base.UpdateSyntax(e, tableInput, expressionEditor), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md elsa-select" }, supportedSyntaxes.map(supportedSyntax => {
                                    const selected = supportedSyntax == syntax;
                                    return h("option", { selected: selected }, supportedSyntax);
                                })))))),
                h("tr", { onClick: () => this.onToggleOptions(index) },
                    h("th", { class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-text-left elsa-tracking-wider elsa-w-1/12", colSpan: 3, style: { cursor: "zoom-in" } }, " Options")),
                h("tr", { style: { display: optionsDisplay } },
                    h("th", { class: "elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12" },
                        "Is ",
                        h("br", null),
                        "Is read-only?"),
                    h("td", { class: "elsa-py-0" },
                        h("input", { name: "choice_input", type: "checkbox", checked: readOnly, value: tableInput.expressions[DataTableSyntax.Readonly], onChange: e => this._base.UpdateCheckbox(e, tableInput, DataTableSyntax.Readonly), class: "focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" })),
                    h("td", null)),
                h("tr", { style: { display: sumTotalDisplay } },
                    h("th", { class: "elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12" },
                        "Is ",
                        h("br", null),
                        "Apply Sum Total?"),
                    h("td", { class: "elsa-py-0" },
                        h("input", { name: "choice_input", type: "checkbox", checked: sumTotalColumn, value: tableInput.expressions[DataTableSyntax.SumTotalColumn], onChange: e => this._base.UpdateCheckbox(e, tableInput, DataTableSyntax.SumTotalColumn), class: "focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" })),
                    h("td", null,
                        h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, "Indicates that this cell should be used to display the total value of all other rows for this column or table. This will be picked up by the front end, and automatically calculated if Javascript is enabled."))),
                h("tr", { style: { display: optionsDisplay } },
                    h("th", { class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12" }, "Pre-Populated Input"),
                    h("td", { class: "elsa-py-2 elsa-w-10/12", colSpan: 2, style: { width: colWidth } },
                        h("div", { class: "elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm elsa-text-box" },
                            h("he-expression-editor", { key: `expression-editor-${index}-${this.syntaxSwitchCount}-${this.keyId}`, ref: el => expressionEditor = el, expression: inputExpression, language: monacoLanguage, "single-line": false, editorHeight: this.editorHeight, padding: "elsa-pt-1.5 elsa-pl-1 elsa-pr-28", onExpressionChanged: e => this._base.CustomUpdateExpression(e, tableInput, DataTableSyntax.Input) }),
                            h("div", { class: "elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center elsa-select" },
                                h("select", { onChange: e => this._base.UpdateSyntax(e, tableInput, expressionEditor), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md elsa-select" }, supportedSyntaxes.filter(x => x == SyntaxNames.JavaScript).map(supportedSyntax => {
                                    const selected = supportedSyntax == SyntaxNames.JavaScript;
                                    return h("option", { selected: selected }, supportedSyntax);
                                }))))))));
        };
        const context = {
            activityTypeName: this.activityModel.type,
            propertyName: this.propertyDescriptor.name
        };
        return (h("div", null,
            h("div", { class: "elsa-mb-1" },
                h("div", { class: "elsa-flex" },
                    h("div", { class: "elsa-flex-1" },
                        h("label", { class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700" }, "Table input type")))),
            h("div", null,
                h("div", null,
                    h("select", { onChange: e => this.onInputTypeChange(e), class: "elsa-mt-1 elsa-block focus:elsa-ring-blue-500 elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md" }, this.inputOptions.map(inputType => {
                        const selected = inputType === selectedType;
                        return h("option", { selected: selected, value: inputType }, inputType);
                    }))),
                h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, "The type of input that will be accepted by the table in the assessment.")),
            h("br", null),
            h("div", { class: "elsa-mb-1" },
                h("div", { class: "elsa-flex" },
                    h("div", { class: "elsa-flex-1" },
                        h("label", { class: "elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700" }, "Display Group Id")))),
            h("div", null,
                h("div", null,
                    h("input", { type: "text", value: this.propertyModel.expressions[DataTableSyntax.DisplayGroupId], onChange: e => this._base.UpdateExpressionFromInput(e, this.propertyModel, DataTableSyntax.DisplayGroupId), class: "focus:elsa-ring-blue-500 focus:elsa-border-bue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" })),
                h("p", { class: "elsa-mt-2 elsa-text-sm elsa-text-gray-500" }, "This allows you to display this table as a column of a shared table with all matching Group Id's of Tables on this Question Screen..")),
            h("br", null),
            h("he-multi-expression-editor", { ref: el => this.multiExpressionEditor = el, label: this.propertyDescriptor.label, defaultSyntax: SyntaxNames.Json, supportedSyntaxes: [SyntaxNames.Json], context: context, expressions: { 'Json': json }, "editor-height": "20rem", onExpressionChanged: e => this.onMultiExpressionEditorValueChanged(e), onSyntaxChanged: e => this.onMultiExpressionEditorSyntaxChanged(e) },
                h("table", { class: "elsa-min-w-full elsa-divide-y elsa-divide-gray-200", ref: el => (this.container = el) }, cases.map(renderCaseEditor)),
                h("button", { type: "button", onClick: () => this.onAddRowClick(), class: "elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2" },
                    h(PlusIcon, { options: this.iconProvider.getOptions() }),
                    "Add Table Row"))));
    }
};
__decorate([
    Prop()
], HeDataTableProperty.prototype, "activityModel", void 0);
__decorate([
    Prop()
], HeDataTableProperty.prototype, "propertyDescriptor", void 0);
__decorate([
    Prop()
], HeDataTableProperty.prototype, "propertyModel", void 0);
__decorate([
    Prop()
], HeDataTableProperty.prototype, "modelSyntax", void 0);
__decorate([
    State()
], HeDataTableProperty.prototype, "keyId", void 0);
__decorate([
    State()
], HeDataTableProperty.prototype, "properties", void 0);
__decorate([
    State()
], HeDataTableProperty.prototype, "iconProvider", void 0);
__decorate([
    Event()
], HeDataTableProperty.prototype, "expressionChanged", void 0);
__decorate([
    State()
], HeDataTableProperty.prototype, "dictionary", void 0);
__decorate([
    State()
], HeDataTableProperty.prototype, "switchTextHeight", void 0);
__decorate([
    State()
], HeDataTableProperty.prototype, "editorHeight", void 0);
__decorate([
    State()
], HeDataTableProperty.prototype, "inputOptions", void 0);
__decorate([
    State()
], HeDataTableProperty.prototype, "selectedInputType", void 0);
HeDataTableProperty = __decorate([
    Component({
        tag: 'he-data-table-property',
        shadow: false,
    })
], HeDataTableProperty);
export { HeDataTableProperty };
