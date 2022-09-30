var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, h, Prop, State } from '@stencil/core';
import { 
//IntellisenseContext,
SyntaxNames } from '../../models/elsa-interfaces';
//import {
//  Map
//} from '../../models/utils';
//import {
//  IconName,
//  IconProviderOptions
//} from '../../models/icon-provider';
import { IconProvider, IconName } from './Test';
// import {parseJson, mapSyntaxToLanguage} from "../../../../../Elsa-Core-HE/src/designer/elsa-workflows-studio/src/utils/utils";
// import {IconName, iconProvider} from "../../../../../Elsa-Core-HE/src/designer/elsa-workflows-studio/src/services/icon-provider";
// Temporary hacks until imports commented out above are sorted
//class IconProvider {
//  private map: Map<(options?: IconProviderOptions) => any> = {
//    'plus': (options?: IconProviderOptions) =>
//      <svg
//        class={`-elsa-ml-1 elsa-mr-2 elsa-h-5 elsa-w-5 ${options?.color ? `elsa-text-${options.color}-500` : ''} ${options?.hoverColor ? `hover:elsa-text-${options.hoverColor}-500` : ''}`}
//        width="24" height="24" viewBox="0 0 24 24"
//        stroke-width="2" stroke="currentColor" fill="transparent" stroke-linecap="round"
//        stroke-linejoin="round">
//        <path stroke="none" d="M0 0h24v24H0z"/>
//        <line x1="12" y1="5" x2="12" y2="19"/>
//        <line x1="5" y1="12" x2="19" y2="12"/>
//      </svg>,
//    'trash-bin-outline': (options?: IconProviderOptions) =>
//      <svg
//        class={`elsa-h-5 elsa-w-5 ${options?.color ? `elsa-text-${options.color}-500` : ''} ${options?.hoverColor ? `hover:elsa-text-${options.hoverColor}-500` : ''}`}
//        width="24" height="24" viewBox="0 0 24 24"
//        stroke-width="2" stroke="currentColor" fill="transparent" stroke-linecap="round"
//        stroke-linejoin="round">
//        <polyline points="3 6 5 6 21 6"/>
//        <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"/>
//        <line x1="10" y1="11" x2="10" y2="17"/>
//        <line x1="14" y1="11" x2="14" y2="17"/>
//      </svg>
//  };
//  getIcon(name: IconName, options?: IconProviderOptions): any {
//    const provider = this.map[name];
//    if (!provider)
//      return undefined;
//    return provider(options);
//  }
//}
const iconProvider = new IconProvider();
function parseJson(json) {
    if (!json)
        return null;
    try {
        return JSON.parse(json);
    }
    catch (e) {
        console.warn(`Error parsing JSON: ${e}`);
    }
    return undefined;
}
let ElsaMultiChoiceRecordsProperty = class ElsaMultiChoiceRecordsProperty {
    constructor() {
        this.choices = [];
        // singleLineProperty: Components.ElsaSingleLineProperty;
        this.supportedSyntaxes = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
        this.syntaxMultiChoiceCount = 0;
    }
    async componentWillLoad() {
        const propertyModel = this.propertyModel;
        const choicesJson = propertyModel.expressions['Multichoice'];
        this.choices = parseJson(choicesJson) || [];
    }
    updatePropertyModel() {
        console.log(this.propertyModel);
        console.log(this.choices);
        this.propertyModel.expressions['Multichoice'] = JSON.stringify(this.choices);
        // this.multiExpressionEditor.expressions[SyntaxNames.Json] = JSON.stringify(this.choices, null, 2);
    }
    onAddChoiceClick() {
        const choiceName = `Choice ${this.choices.length + 1}`;
        const newChoice = { answer: choiceName, isSingle: false };
        this.choices = [...this.choices, newChoice];
        this.updatePropertyModel();
    }
    onDeleteChoiceClick(multiChoice) {
        this.choices = this.choices.filter(x => x != multiChoice);
        this.updatePropertyModel();
    }
    onChoiceNameChanged(e, multiChoice) {
        multiChoice.answer = e.currentTarget.value.trim();
        this.updatePropertyModel();
    }
    onCheckChanged(e, multiChoice) {
        const checkbox = e.target;
        multiChoice.isSingle = checkbox.checked;
        // const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
        // this.propertyModel.expressions[defaultSyntax] = multiChoice.isSingle.toString();
        this.updatePropertyModel();
    }
    render() {
        const choices = this.choices;
        // const supportedSyntaxes = this.supportedSyntaxes;
        // const json = JSON.stringify(choices, null, 2);
        const renderChoiceEditor = (multiChoice, index) => {
            // const expression = multiChoice.answer;
            // const monacoLanguage = mapSyntaxToLanguage(syntax);
            // let expressionEditor = null;
            const propertyDescriptor = this.propertyDescriptor;
            const propertyName = propertyDescriptor.name;
            const fieldId = propertyName;
            const fieldName = propertyName;
            let isChecked = multiChoice.isSingle;
            console.log("moo render");
            console.log(multiChoice);
            return (h("tr", { key: `choice-${index}` },
                h("td", { class: "elsa-py-2 elsa-pr-5" },
                    h("input", { type: "text", value: multiChoice.answer, onChange: e => this.onChoiceNameChanged(e, multiChoice), class: "focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" })),
                h("td", { class: "elsa-py-0" },
                    h("input", { id: fieldId, name: fieldName, type: "checkbox", checked: isChecked, value: 'true', onChange: e => this.onCheckChanged(e, multiChoice), class: "focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" })),
                h("td", { class: "elsa-pt-1 elsa-pr-2 elsa-text-right" },
                    h("button", { type: "button", onClick: () => this.onDeleteChoiceClick(multiChoice), class: "elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none" }, iconProvider.getIcon(IconName.TrashBinOutline)))));
        };
        // const context: IntellisenseContext = {
        //   activityTypeName: this.activityModel.type,
        //   propertyName: this.propertyDescriptor.name
        // };
        console.log("moo");
        console.log(choices);
        return (h("div", null,
            h("table", { class: "elsa-min-w-full elsa-divide-y elsa-divide-gray-200" },
                h("thead", { class: "elsa-bg-gray-50" },
                    h("tr", null,
                        h("th", { class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-10/12" }, "Answer"),
                        h("th", { class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-1/12" }, "IsSingle"),
                        h("th", { class: "elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-1/12" }, "\u00A0"))),
                h("tbody", null, choices.map(renderChoiceEditor))),
            h("button", { type: "button", onClick: () => this.onAddChoiceClick(), class: "elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2" },
                iconProvider.getIcon(IconName.Plus),
                "Add Choice")));
    }
};
__decorate([
    Prop()
], ElsaMultiChoiceRecordsProperty.prototype, "activityModel", void 0);
__decorate([
    Prop()
], ElsaMultiChoiceRecordsProperty.prototype, "propertyDescriptor", void 0);
__decorate([
    Prop()
], ElsaMultiChoiceRecordsProperty.prototype, "propertyModel", void 0);
__decorate([
    State()
], ElsaMultiChoiceRecordsProperty.prototype, "choices", void 0);
ElsaMultiChoiceRecordsProperty = __decorate([
    Component({
        tag: 'elsa-multichoice-records-property',
        shadow: false,
    })
], ElsaMultiChoiceRecordsProperty);
export { ElsaMultiChoiceRecordsProperty };
