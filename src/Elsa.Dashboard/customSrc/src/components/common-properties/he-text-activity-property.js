var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, Event, h, Prop, State } from '@stencil/core';
import { IconProvider } from "../providers/icon-provider/icon-provider";
import PlusIcon from '../../icons/plus_icon';
import TrashCanIcon from '../../icons/trash-can';
import { mapSyntaxToLanguage, parseJson, newOptionLetter } from '../../utils/utils';
import { PropertyOutputTypes, SyntaxNames, TextActivityOptionsSyntax } from '../../constants/constants';
import { SortableComponent } from '../base-component';
import SortIcon from '../../icons/sort_icon';
import { DisplayToggle } from '../display-toggle-component';
let TextActivityProperty = class TextActivityProperty {
    constructor() {
        this.modelSyntax = SyntaxNames.TextActivityList;
        this.properties = [];
        this.iconProvider = new IconProvider();
        this.supportedSyntaxes = [SyntaxNames.Literal, SyntaxNames.JavaScript];
        this.dictionary = {};
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
            expressions: { [SyntaxNames.Literal]: '', [TextActivityOptionsSyntax.Paragraph]: 'true', [TextActivityOptionsSyntax.Condition]: 'true' },
            type: PropertyOutputTypes.Information,
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
        const renderCaseEditor = (nestedTextActivity, index) => {
            var _a, _b;
            const textSyntax = nestedTextActivity.syntax;
            const conditionSyntax = SyntaxNames.JavaScript;
            const textExpression = nestedTextActivity.expressions[textSyntax];
            const conditionExpression = nestedTextActivity.expressions[TextActivityOptionsSyntax.Condition];
            const urlExpression = (_a = nestedTextActivity.expressions[TextActivityOptionsSyntax.Url]) !== null && _a !== void 0 ? _a : "https://www";
            const paragraphChecked = nestedTextActivity.expressions[TextActivityOptionsSyntax.Paragraph] == 'true';
            const guidanceChecked = nestedTextActivity.expressions[TextActivityOptionsSyntax.Guidance] == 'true';
            const hyperlinkChecked = nestedTextActivity.expressions[TextActivityOptionsSyntax.Hyperlink] == 'true';
            const textLanguage = mapSyntaxToLanguage(textSyntax);
            const conditionLanguage = mapSyntaxToLanguage(conditionSyntax);
            const urlLanguage = mapSyntaxToLanguage(SyntaxNames.Literal);
            const urlIndex = index + "_url";
            const conditionEditorHeight = "2.75em";
            if (this.dictionary[index] == null && (guidanceChecked || hyperlinkChecked)) {
                this.dictionary[index] = "table-row";
                if (this.dictionary[urlIndex] == null && hyperlinkChecked) {
                    this.dictionary[urlIndex] = "table-row";
                }
            }
            const optionsDisplay = (_b = this._toggle.component.dictionary[index]) !== null && _b !== void 0 ? _b : "none";
            const urlDisplay = this.dictionary[urlIndex] != null && this.dictionary[index] != null && this.dictionary[index] != "none"
                ? this.dictionary[urlIndex]
                : "none";
            let textExpressionEditor = null;
            let conditionExpressionEditor = null;
            let colWidth = "100%";
            let textContext = {
                activityTypeName: this.activityModel.type,
                propertyName: this.propertyDescriptor.name
            };
            return (h("tbody", { key: this.keyId },
                h("tr", null,
                    h("th", { class: "sortablejs-custom-handle" },
                        h(SortIcon, { options: this.iconProvider.getOptions() })),
                    h("td", null),
                    h("td", { class: "elsa-pt-1 elsa-pr-2 elsa-text-right" },
                        h("button", { type: "button", onClick: () => this.onHandleDelete(nestedTextActivity), class: "elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none" },
                            h(TrashCanIcon, { options: this.iconProvider.getOptions() })))),
                h("tr", null,
                    h("th", { class: "elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12" }, "Text"),
                    h("td", { class: "elsa-py-2 pl-5", colSpan: 2, style: { width: colWidth } },
                        h("div", { class: "elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm elsa-text-box" },
                            h("he-expression-editor", { key: `expression-editor-${index}-${this.syntaxSwitchCount}`, ref: el => textExpressionEditor = el, expression: textExpression, language: textLanguage, context: textContext, "single-line": false, editorHeight: "2.75em", padding: "elsa-pt-1.5 elsa-pl-1 elsa-pr-28", onExpressionChanged: e => this._base.CustomUpdateExpression(e, nestedTextActivity, nestedTextActivity.syntax) }),
                            h("div", { class: "elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center elsa-select" },
                                h("select", { onChange: e => this._base.UpdateSyntax(e, nestedTextActivity, textExpressionEditor), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md elsa-select" }, this.supportedSyntaxes.map(supportedSyntax => {
                                    const selected = supportedSyntax == textSyntax;
                                    return h("option", { selected: selected }, supportedSyntax);
                                })))))),
                h("tr", { onClick: () => this.onToggleOptions(index) },
                    h("th", { class: "elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-text-left elsa-tracking-wider elsa-w-1/12", colSpan: 3, style: { cursor: "zoom-in" } }, " Options")),
                h("tr", { style: { display: optionsDisplay } },
                    h("th", { class: "elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12" }, "Display on Page"),
                    h("td", { class: "elsa-py-2 pl-5", colSpan: 2, style: { width: colWidth } },
                        h("div", { class: "elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm elsa-text-box" },
                            h("he-expression-editor", { key: `expression-editor-${index}-${this.syntaxSwitchCount}`, ref: el => conditionExpressionEditor = el, expression: conditionExpression, language: conditionLanguage, "single-line": false, editorHeight: conditionEditorHeight, padding: "elsa-pt-1.5 elsa-pl-1 elsa-pr-28", onExpressionChanged: e => this._base.CustomUpdateExpression(e, nestedTextActivity, TextActivityOptionsSyntax.Condition) }),
                            h("div", { class: "elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center elsa-select" },
                                h("select", { onChange: e => this._base.UpdateSyntax(e, nestedTextActivity, conditionExpressionEditor), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md elsa-select" }, this.supportedSyntaxes.filter(x => x == SyntaxNames.JavaScript).map(supportedSyntax => {
                                    const selected = supportedSyntax == SyntaxNames.JavaScript;
                                    return h("option", { selected: selected }, supportedSyntax);
                                })))))),
                h("tr", { style: { display: optionsDisplay } },
                    h("th", { class: "elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12" },
                        "Is ",
                        h("br", null),
                        "Paragraph"),
                    h("td", { class: "elsa-py-0" },
                        h("input", { name: "choice_input", type: "checkbox", checked: paragraphChecked, value: nestedTextActivity.expressions[TextActivityOptionsSyntax.Paragraph], onChange: e => this._base.UpdateCheckbox(e, nestedTextActivity, TextActivityOptionsSyntax.Paragraph), class: "focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" })),
                    h("td", null)),
                h("tr", { style: { display: optionsDisplay } },
                    h("th", { class: "elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12" },
                        "Is ",
                        h("br", null),
                        "Bold"),
                    h("td", { class: "elsa-py-0" },
                        h("input", { name: "choice_input", type: "checkbox", checked: guidanceChecked, value: nestedTextActivity.expressions[TextActivityOptionsSyntax.Bold], onChange: e => this._base.UpdateCheckbox(e, nestedTextActivity, TextActivityOptionsSyntax.Bold), class: "focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" })),
                    h("td", null)),
                h("tr", { style: { display: optionsDisplay } },
                    h("th", { class: "elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12" },
                        "Is ",
                        h("br", null),
                        "Hyperlink"),
                    h("td", { class: "elsa-py-0" },
                        h("input", { name: "choice_input", type: "checkbox", checked: hyperlinkChecked, value: nestedTextActivity.expressions[TextActivityOptionsSyntax.Hyperlink], onChange: e => [this._base.UpdateCheckbox(e, nestedTextActivity, TextActivityOptionsSyntax.Hyperlink), this.onToggleOptions(urlIndex)], class: "focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" })),
                    h("td", null)),
                h("tr", { style: { display: urlDisplay } },
                    h("th", { class: "elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12" }, "Url"),
                    h("td", { class: "elsa-py-2 pl-5", style: { width: colWidth } },
                        h("div", { class: "elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm" },
                            h("he-expression-editor", { key: `expression-editor-${index}-${this.syntaxSwitchCount}`, ref: el => conditionExpressionEditor = el, expression: urlExpression, language: urlLanguage, "single-line": true, editorHeight: "2.75em", padding: "elsa-pt-1.5 elsa-pl-1 elsa-pr-28", onExpressionChanged: e => this._base.CustomUpdateExpression(e, nestedTextActivity, TextActivityOptionsSyntax.Url) }))),
                    h("td", null))));
        };
        const context = {
            activityTypeName: this.activityModel.type,
            propertyName: this.propertyDescriptor.name
        };
        return (h("div", null,
            h("he-multi-expression-editor", { ref: el => this.multiExpressionEditor = el, label: this.propertyDescriptor.label, hint: this.propertyDescriptor.hint, defaultSyntax: SyntaxNames.Json, supportedSyntaxes: [SyntaxNames.Json], context: context, expressions: { 'Json': json }, "editor-height": "20rem", onExpressionChanged: e => this.onMultiExpressionEditorValueChanged(e), onSyntaxChanged: e => this.onMultiExpressionEditorSyntaxChanged(e) },
                h("table", { class: "elsa-min-w-full elsa-divide-y elsa-divide-gray-200 elsa-table-striped", ref: el => (this.container = el) }, textElements.map(renderCaseEditor)),
                h("button", { type: "button", onClick: () => this.onAddElementClick(), class: "elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2" },
                    h(PlusIcon, { options: this.iconProvider.getOptions() }),
                    "Add Paragraph"))));
    }
};
__decorate([
    Prop()
], TextActivityProperty.prototype, "activityModel", void 0);
__decorate([
    Prop()
], TextActivityProperty.prototype, "propertyDescriptor", void 0);
__decorate([
    Prop()
], TextActivityProperty.prototype, "propertyModel", void 0);
__decorate([
    Prop()
], TextActivityProperty.prototype, "modelSyntax", void 0);
__decorate([
    State()
], TextActivityProperty.prototype, "properties", void 0);
__decorate([
    State()
], TextActivityProperty.prototype, "iconProvider", void 0);
__decorate([
    State()
], TextActivityProperty.prototype, "keyId", void 0);
__decorate([
    Event()
], TextActivityProperty.prototype, "expressionChanged", void 0);
__decorate([
    State()
], TextActivityProperty.prototype, "dictionary", void 0);
TextActivityProperty = __decorate([
    Component({
        tag: 'he-text-activity-property',
        shadow: false,
    })
], TextActivityProperty);
export { TextActivityProperty };
