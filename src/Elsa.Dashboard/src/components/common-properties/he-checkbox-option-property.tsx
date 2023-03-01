import { Component, Event, EventEmitter, h, Prop, State } from '@stencil/core';
import { PropertyOutputTypes, SyntaxNames } from '../../constants/constants';
import ExpandIcon from '../../icons/expand_icon';
import PlusIcon from '../../icons/plus_icon';
import TrashCanIcon from '../../icons/trash-can';
import { NestedActivityDefinitionProperty } from '../../models/custom-component-models';
import {
    ActivityDefinitionProperty,
    ActivityModel,
    ActivityPropertyDescriptor,
    HTMLElsaExpressionEditorElement,
    HTMLElsaMultiExpressionEditorElement,
    IntellisenseContext
} from "../../models/elsa-interfaces";
import { mapSyntaxToLanguage, parseJson, ToLetter } from "../../utils/utils";
import { IconProvider } from "../providers/icon-provider/icon-provider";

@Component({
  tag: 'he-checkbox-options-property',
  shadow: false,
})
//Copy of Elsa Switch Case
//Copied to allow us control over how the expression editor is displayed.
export class HeCheckboxOptionProperty {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() propertyModel: ActivityDefinitionProperty;
  @State() options: Array<NestedActivityDefinitionProperty> = [];
  @State() iconProvider = new IconProvider();
  @Event() expressionChanged: EventEmitter<string>;


  @State() switchTextHeight: string = "";

  @State() editorHeight: string = "2.75em"

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid, SyntaxNames.Literal];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxSwitchCount: number = 0;

  async componentWillLoad() {
    const propertyModel = this.propertyModel;
    const optionsJson = propertyModel.expressions[SyntaxNames.Json]
    this.options = parseJson(optionsJson) || [];
  }

  updatePropertyModel() {
    this.propertyModel.expressions[SyntaxNames.Json] = JSON.stringify(this.options);
    this.multiExpressionEditor.expressions[SyntaxNames.Json] = JSON.stringify(this.options, null, 2);
    this.expressionChanged.emit(JSON.stringify(this.propertyModel))
  }

  onDefaultSyntaxValueChanged(e: CustomEvent) {
    this.options = e.detail;
  }

  onAddOptionClick() {
    const optionName = ToLetter(this.options.length + 1);
    const newOption: NestedActivityDefinitionProperty = { name: optionName, syntax: SyntaxNames.Literal, expressions: { [SyntaxNames.Literal]: '', [SyntaxNames.Checked]: 'false' }, type: PropertyOutputTypes.Checkbox };
    this.options = [...this.options, newOption];
    this.updatePropertyModel();
  }

  onDeleteOptionClick(checkbox: NestedActivityDefinitionProperty) {
    this.options = this.options.filter(x => x != checkbox);
    this.updatePropertyModel();
  }

  onOptionNameChanged(e: Event, checkbox: NestedActivityDefinitionProperty) {
    checkbox.name = (e.currentTarget as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }

  onOptionExpressionChanged(e: CustomEvent<string>, checkbox: NestedActivityDefinitionProperty) {
    checkbox.expressions[checkbox.syntax] = e.detail;
    this.updatePropertyModel();
  }

  onCheckChanged(e: Event, checkbox: NestedActivityDefinitionProperty) {
    const checkboxElement = (e.currentTarget as HTMLInputElement);
    checkbox.expressions[SyntaxNames.Checked] = checkboxElement.checked.toString();
    this.updatePropertyModel();
  }

  onOptionSyntaxChanged(e: Event, switchCase: NestedActivityDefinitionProperty, expressionEditor: HTMLElsaExpressionEditorElement) {
    const select = e.currentTarget as HTMLSelectElement;
    switchCase.syntax = select.value;
    expressionEditor.language = mapSyntaxToLanguage(switchCase.syntax);
    this.updatePropertyModel();
  }

  onMultiExpressionEditorValueChanged(e: CustomEvent<string>) {
    const json = e.detail;
    const parsed = parseJson(json);

    if (!parsed)
      return;

    if (!Array.isArray(parsed))
      return;

    this.propertyModel.expressions[SyntaxNames.Json] = json;
    this.options = parsed;
  }

  onMultiExpressionEditorSyntaxChanged(e: CustomEvent<string>) {
    e = e;
    this.syntaxSwitchCount++;
  }

  onExpandSwitchArea() {
    this.editorHeight == "2.75em" ? this.editorHeight = "8em" : this.editorHeight = "2.75em"
  }

  render() {
    const cases = this.options;
    const supportedSyntaxes = this.supportedSyntaxes;
    const json = JSON.stringify(cases, null, 2);

    const renderCaseEditor = (checkboxOption: NestedActivityDefinitionProperty, index: number) => {
      const expression = checkboxOption.expressions[checkboxOption.syntax];
      const syntax = checkboxOption.syntax;
      const monacoLanguage = mapSyntaxToLanguage(syntax);
      const checked = checkboxOption.expressions[SyntaxNames.Checked] == 'true';
      let expressionEditor = null;

      return (
        <tr key={`case-${index}`}>
          <td class="elsa-py-2 elsa-pr-5">
            <input type="text" value={checkboxOption.name} onChange={e => this.onOptionNameChanged(e, checkboxOption)}
              class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
          </td>
          <td class="elsa-py-2 pl-5">

            <div class="elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm">
              <elsa-expression-editor
                key={`expression-editor-${index}-${this.syntaxSwitchCount}`}
                ref={el => expressionEditor = el}
                expression={expression}
                language={monacoLanguage}
                single-line={false}
                editorHeight={this.editorHeight}
                padding="elsa-pt-1.5 elsa-pl-1 elsa-pr-28"
                onExpressionChanged={e => this.onOptionExpressionChanged(e, checkboxOption)}
              />
              <div class="elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center">
                <select onChange={e => this.onOptionSyntaxChanged(e, checkboxOption, expressionEditor)}
                  class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md">
                  {supportedSyntaxes.map(supportedSyntax => {
                    const selected = supportedSyntax == syntax;
                    return <option selected={selected}>{supportedSyntax}</option>;
                  })}
                </select>
              </div>
            </div>
          </td>
          <td class="elsa-py-0">
            <input name="choice_input" type="checkbox" checked={checked} value={'true'}
              onChange={e => this.onCheckChanged(e, checkboxOption)}
              class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
          </td>
          <td class="elsa-pt-1 elsa-pr-2 elsa-text-right">
            <button type="button" onClick={() => this.onDeleteOptionClick(checkboxOption)}
              class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
              <TrashCanIcon options={this.iconProvider.getOptions()}></TrashCanIcon>
            </button>
          </td>
        </tr>
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
            <thead class="elsa-bg-gray-50">
              <tr>
                <th
                  class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Identifier
                </th>
                <th
                  class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-8/12">Answer
                </th>
                <th
                  class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-1/12">IsSingle
                </th>
                <th
                  class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-1/12">
                  <button type="button" onClick={() => this.onExpandSwitchArea()}
                    class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
                    <ExpandIcon options={this.iconProvider.getOptions()}></ExpandIcon>
                  </button>
                </th>
              </tr>
            </thead>
            <tbody>
              {cases.map(renderCaseEditor)}
            </tbody>
          </table>
          <button type="button" onClick={() => this.onAddOptionClick()}
            class="elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2">
            <PlusIcon options={this.iconProvider.getOptions()}></PlusIcon>
            Add Case
          </button>
        </elsa-multi-expression-editor>
      </div>
    );
  }
}
