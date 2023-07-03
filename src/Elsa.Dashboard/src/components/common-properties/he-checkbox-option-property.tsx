import { Component, Event, EventEmitter, h, Prop, State } from '@stencil/core';
import { CheckboxOptionsSyntax, PropertyOutputTypes, SyntaxNames } from '../../constants/constants';
import { ToggleDictionaryDisplay } from '../../functions/display-toggle';
import ExpandIcon from '../../icons/expand_icon';
import PlusIcon from '../../icons/plus_icon';
import TrashCanIcon from '../../icons/trash-can';
import { NestedActivityDefinitionProperty } from '../../models/custom-component-models';
import {
  ActivityDefinitionProperty,
  ActivityModel,
  ActivityPropertyDescriptor,
  HTMLElsaMultiExpressionEditorElement,
  IntellisenseContext
} from "../../models/elsa-interfaces";
import { mapSyntaxToLanguage, parseJson, ToLetter, Map } from "../../utils/utils";
import { BaseComponent, ISharedComponent } from '../base-component';
import { IconProvider } from "../providers/icon-provider/icon-provider";

@Component({
  tag: 'he-checkbox-options-property',
  shadow: false,
})

export class HeCheckboxOptionProperty implements ISharedComponent {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() propertyModel: ActivityDefinitionProperty;
  @Prop() modelSyntax: string = SyntaxNames.Json;
  @State() properties: Array<NestedActivityDefinitionProperty> = [];
  @State() iconProvider = new IconProvider();
  @Event() expressionChanged: EventEmitter<string>;
  @State() optionsDisplayToggle: Map<string> = {};

  @State() switchTextHeight: string = "";

  @State() editorHeight: string = "2.75em"

  private _base: BaseComponent;

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid, SyntaxNames.Literal];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxSwitchCount: number = 0;

  constructor() {
    this._base = new BaseComponent(this);
  }

  async componentWillLoad() {
    this._base.componentWillLoad();
  }
  

  onDefaultSyntaxValueChanged(e: CustomEvent) {
    this.properties = e.detail;
  }

  onAddOptionClick() {
    const optionName = ToLetter(this.properties.length + 1);
    const newOption: NestedActivityDefinitionProperty = { name: optionName, syntax: SyntaxNames.Literal, expressions: { [SyntaxNames.Literal]: '', [CheckboxOptionsSyntax.Single]: 'false', [CheckboxOptionsSyntax.PrePopulated]: 'false' }, type: PropertyOutputTypes.Checkbox };
    this.properties = [...this.properties, newOption];
    this._base.updatePropertyModel();
  }

  onDeleteOptionClick(checkbox: NestedActivityDefinitionProperty) {
    this.properties = this.properties.filter(x => x != checkbox);
    this._base.updatePropertyModel();
  }

  onMultiExpressionEditorValueChanged(e: CustomEvent<string>) {
    const json = e.detail;
    const parsed = parseJson(json);

    if (!parsed)
      return;

    if (!Array.isArray(parsed))
      return;

    this.propertyModel.expressions[SyntaxNames.Json] = json;
    this.properties = parsed;
  }

  onMultiExpressionEditorSyntaxChanged(e: CustomEvent<string>) {
    e = e;
    this.syntaxSwitchCount++;
  }

  onExpandSwitchArea() {
    this.editorHeight == "2.75em" ? this.editorHeight = "8em" : this.editorHeight = "2.75em"
  }

  onToggleOptions(index: number) {
    let tempValue = ToggleDictionaryDisplay(index, this.optionsDisplayToggle)
    this.optionsDisplayToggle = { ... this.optionsDisplayToggle, tempValue }
  }

  render() {
    const cases = this.properties;
    const supportedSyntaxes = this.supportedSyntaxes;
    const json = JSON.stringify(cases, null, 2);

    const renderCaseEditor = (checkboxOption: NestedActivityDefinitionProperty, index: number) => {
      const expression = checkboxOption.expressions[checkboxOption.syntax];
      const syntax = checkboxOption.syntax;
      const monacoLanguage = mapSyntaxToLanguage(syntax);
      const checked = checkboxOption.expressions[CheckboxOptionsSyntax.Single] == 'true';
      const prePopulatedSyntax = SyntaxNames.JavaScript;
      const prePopulatedExpression = checkboxOption.expressions[CheckboxOptionsSyntax.PrePopulated];

      const prePopulatedLanguage = mapSyntaxToLanguage(prePopulatedSyntax);

      let expressionEditor = null;
      let prePopulatedExpressionEditor = null;
      let colWidth = "100%";

      const optionsDisplay = this.optionsDisplayToggle[index] ?? "none";

      return (
        <tbody>
          <tr key={`case-${index}`}>
            <th class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Identifier</th>

            <td class="elsa-py-2 pl-5" style={{ width: colWidth }}>
              <div>
                <input type="text" value={checkboxOption.name} onChange={e => this._base.UpdateName(e, checkboxOption)}
                  class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
              </div>
            </td>

            <td class="elsa-pt-1 elsa-pr-2 elsa-text-right">
              <button type="button" onClick={() => this.onDeleteOptionClick(checkboxOption)}
                class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
                <TrashCanIcon options={this.iconProvider.getOptions()}></TrashCanIcon>
              </button>
            </td>

          </tr>

          <tr>
            <th class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Answer</th>

            <td class="elsa-py-2 pl-5" style={{ width: colWidth }}>
              <div class="elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm">
                <elsa-expression-editor
                  key={`expression-editor-${index}-${this.syntaxSwitchCount}`}
                  ref={el => expressionEditor = el}
                  expression={expression}
                  language={monacoLanguage}
                  single-line={false}
                  editorHeight={this.editorHeight}
                  padding="elsa-pt-1.5 elsa-pl-1 elsa-pr-28"
                  onExpressionChanged={e => this._base.CustomUpdateExpression(e, checkboxOption, checkboxOption.syntax)}
                />
                <div class="elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center">
                  <select onChange={e => this._base.UpdateSyntax(e, checkboxOption, expressionEditor)}
                    class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md">
                    {supportedSyntaxes.map(supportedSyntax => {
                      const selected = supportedSyntax == syntax;
                      return <option selected={selected}>{supportedSyntax}</option>;
                    })}
                  </select>
                </div>
              </div>
            </td>
            <td class="elsa-pt-1 elsa-pr-2 elsa-text-right">
              <button type="button" onClick={() => this.onExpandSwitchArea()}
                class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
                <ExpandIcon options={this.iconProvider.getOptions()}></ExpandIcon>
              </button>
            </td>
          </tr>

          <tr onClick={() => this.onToggleOptions(index)}>
            <th
              class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-text-left elsa-tracking-wider elsa-w-2/12" colSpan={3} style={{ cursor: "zoom-in" }}> Options
            </th>
            <td></td>
            <td></td>
          </tr>

          <tr style={{ display: optionsDisplay }} >
            <th class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">IsSingle</th>
            <td class="elsa-py-0">
              <input name="choice_input" type="checkbox" checked={checked} value={'true'}
                onChange={e => this._base.UpdateCheckbox(e, checkboxOption, CheckboxOptionsSyntax.Single)}
                class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
            </td>
            <td></td>
          </tr>


          <tr style={{ display: optionsDisplay }} >
            <th class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Pre Populated</th>
            <td class="elsa-py-2 pl-5" style={{ width: colWidth }}>
              <div class="elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm">
                <elsa-expression-editor
                  key={`expression-editor-${index}-${this.syntaxSwitchCount}`}
                  ref={el => prePopulatedExpressionEditor = el}
                  expression={prePopulatedExpression}
                  language={prePopulatedLanguage}
                  single-line={false}
                  editorHeight="2.75em"
                  padding="elsa-pt-1.5 elsa-pl-1 elsa-pr-28"
                  onExpressionChanged={e => this._base.CustomUpdateExpression(e, checkboxOption, CheckboxOptionsSyntax.PrePopulated)}
                />
                <div class="elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center">
                  <select onChange={e => this._base.UpdateCheckbox(e, checkboxOption, prePopulatedExpressionEditor)}
                    class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md">
                    {this.supportedSyntaxes.filter(x => x == SyntaxNames.JavaScript).map(supportedSyntax => {
                      const selected = supportedSyntax == SyntaxNames.JavaScript;
                      return <option selected={selected}>{supportedSyntax}</option>;
                    })}
                  </select>
                </div>
              </div>
            </td>
            <td></td>
          </tr>
          </tbody>
        
      );
    };

    const context: IntellisenseContext = {
      activityTypeName: this.activityModel.type,
      propertyName: this.propertyDescriptor.name
    };

    return (
      <div>

        <elsa-multi-expression-editor
          ref={el => this.multiExpressionEditor = el}
          label={this.propertyDescriptor.label}
          defaultSyntax={SyntaxNames.Json}
          supportedSyntaxes={[SyntaxNames.Json]}
          context={context}
          expressions={{ 'Json': json }}
          editor-height="20rem"
          onExpressionChanged={e => this.onMultiExpressionEditorValueChanged(e)}
          onSyntaxChanged={e => this.onMultiExpressionEditorSyntaxChanged(e)}
        >

          <table class="elsa-min-w-full elsa-divide-y elsa-divide-gray-200">
          {cases.map(renderCaseEditor)}
          </table>
          <button type="button" onClick={() => this.onAddOptionClick()}
            class="elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2">
            <PlusIcon options={this.iconProvider.getOptions()}></PlusIcon>
            Add Answer
          </button>
        </elsa-multi-expression-editor>
      </div >
    );
  }
}
