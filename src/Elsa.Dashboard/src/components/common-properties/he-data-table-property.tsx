import { Component, h, Event, EventEmitter, Prop, State } from '@stencil/core';
import {
  ActivityDefinitionProperty,
  ActivityModel,
  ActivityPropertyDescriptor,
  HTMLElsaMultiExpressionEditorElement,
  IntellisenseContext
} from "../../models/elsa-interfaces";
import { mapSyntaxToLanguage, parseJson, ToLetter,Map } from "../../utils/utils";
import { IconProvider } from "../providers/icon-provider/icon-provider";
import PlusIcon from '../../icons/plus_icon';
import TrashCanIcon from '../../icons/trash-can';
import ExpandIcon from '../../icons/expand_icon';
import { DataTableSyntax, PropertyOutputTypes, SyntaxNames } from '../../constants/constants';
import { NestedActivityDefinitionProperty } from '../../models/custom-component-models';
import { ToggleDictionaryDisplay } from '../../functions/display-toggle'
import { UpdateCheckbox, CustomUpdateExpression, UpdateExpressionFromInput, StandardUpdateExpression, UpdateName, UpdateSyntax } from '../../functions/updateModel';

@Component({
  tag: 'he-data-table-property',
  shadow: false,
})

export class HeDataTableProperty {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() propertyModel: ActivityDefinitionProperty;
  @State() inputs: Array<NestedActivityDefinitionProperty> = [];
  @State() iconProvider = new IconProvider();
  @Event() expressionChanged: EventEmitter<string>;
  @State() optionsDisplayToggle: Map<string> = {};

  @State() switchTextHeight: string = "";

  @State() editorHeight: string = "2.75em"

  @State() inputOptions: Array<string> = [];

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Literal];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxSwitchCount: number = 0;

  UpdateExpression: Function = CustomUpdateExpression.bind(this);
  UpdateInput: Function = UpdateExpressionFromInput.bind(this);
  StandardUpdateExpression: Function = StandardUpdateExpression.bind(this);
  UpdateName: Function = UpdateName.bind(this);
  UpdateCheckbox: Function = UpdateCheckbox.bind(this);
  UpdateSyntax: Function = UpdateSyntax.bind(this);

  async componentWillLoad() {
    const propertyModel = this.propertyModel;
    const optionsJson = propertyModel.expressions[SyntaxNames.Json]
    this.inputs = parseJson(optionsJson) || [];
    this.inputOptions = ["Currency", "Decimal", "Integer", "Text"];

    if (propertyModel.expressions[DataTableSyntax.InputType] == null)
    {
      this.propertyModel.expressions[DataTableSyntax.InputType] = "Currency";

    }

  }

  updatePropertyModel() {
    this.propertyModel.expressions[SyntaxNames.Json] = JSON.stringify(this.inputs);
    this.multiExpressionEditor.expressions[SyntaxNames.Json] = JSON.stringify(this.inputs, null, 2);
    this.expressionChanged.emit(JSON.stringify(this.propertyModel))
  }

  onDefaultSyntaxValueChanged(e: CustomEvent) {
    this.inputs = e.detail;
  }

  onAddRowClick() {
    const optionName = ToLetter(this.inputs.length + 1);
    const newOption: NestedActivityDefinitionProperty = { name: optionName, syntax: SyntaxNames.Literal, expressions: { [SyntaxNames.Literal]: '', [DataTableSyntax.Identifier]: optionName }, type: PropertyOutputTypes.TableInput };
    this.inputs = [...this.inputs, newOption];
    this.updatePropertyModel();
  }

  onDeleteInputClick(input: NestedActivityDefinitionProperty) {
    this.inputs = this.inputs.filter(x => x != input);
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
    this.inputs = parsed;
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
    const cases = this.inputs;
    const supportedSyntaxes = this.supportedSyntaxes;
    const json = JSON.stringify(cases, null, 2);

    const renderCaseEditor = (tableInput: NestedActivityDefinitionProperty, index: number) => {
      const headerExpression = tableInput.expressions[tableInput.syntax]
      const inputExpression = tableInput.expressions[DataTableSyntax.Input];
      const syntax = tableInput.syntax;
      const monacoLanguage = mapSyntaxToLanguage(syntax);

      const sumTotalColumn = tableInput.expressions[DataTableSyntax.SumTotalColumn] == 'true';
      const readOnly = tableInput.expressions[DataTableSyntax.Readonly] == 'true';

      let expressionEditor = null;
      let colWidth = "100%";

      const optionsDisplay = this.optionsDisplayToggle[index] ?? "none";

      return (
        <tbody>
          <tr key={`case-${index}`}>
            <th
              class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Identifier
            </th>
            <td class="elsa-py-2 elsa-pr-5" style={{ width: colWidth }}>
              <input type="text" value={tableInput.expressions[DataTableSyntax.Identifier]} onChange={e => this.UpdateInput(e, tableInput, DataTableSyntax.Identifier)}
                class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
            </td>
            <td class="elsa-pt-1 elsa-pr-2 elsa-text-right">
              <button type="button" onClick={() => this.onDeleteInputClick(tableInput)}
                class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
                <TrashCanIcon options={this.iconProvider.getOptions()}></TrashCanIcon>
              </button>
            </td>

          </tr>
          <tr key={`case-${index}`}>
            <th
              class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Row Heading
            </th>
            <td class="elsa-py-2 pl-5" style={{ width: colWidth }}>
              <div class="elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm">
                <elsa-expression-editor
                  key={`expression-editor-${index}-${this.syntaxSwitchCount}`}
                  ref={el => expressionEditor = el}
                  expression={headerExpression}
                  language={monacoLanguage}
                  single-line={false}
                  editorHeight={this.editorHeight}
                  padding="elsa-pt-1.5 elsa-pl-1 elsa-pr-28"
                  onExpressionChanged={e => this.UpdateExpression(e, tableInput, tableInput.syntax)}
                />
                <div class="elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center">
                  <select onChange={e => this.UpdateSyntax(e, tableInput, expressionEditor)}
                    class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md">
                    {supportedSyntaxes.map(supportedSyntax => {
                      const selected = supportedSyntax == syntax;
                      return <option selected={selected}>{supportedSyntax}</option>;
                    })}
                  </select>
                </div>
              </div>
            </td>
            <td
              class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">
              <button type="button" onClick={() => this.onExpandSwitchArea()}
                class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
                <ExpandIcon options={this.iconProvider.getOptions()}></ExpandIcon>
              </button>
            </td>

          </tr>


          <tr onClick={() => this.onToggleOptions(index)}>
            <th
              class="elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-text-left elsa-tracking-wider elsa-w-1/12" colSpan={3} style={{ cursor: "zoom-in" }}> Options
            </th>
          </tr>

          <tr style={{ display: optionsDisplay }}>
            <th colSpan={2}
              class="elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Is <br />Is read-only?
            </th>
            <td class="elsa-py-0">
              <input name="choice_input" type="checkbox" checked={readOnly} value={tableInput.expressions[DataTableSyntax.Readonly]}
                onChange={e => this.UpdateCheckbox(e, tableInput, DataTableSyntax.Readonly)}
                class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
            </td>
          </tr>

          <tr style={{ display: optionsDisplay }}>
            <th colSpan={2}
              class="elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Is <br />Apply Sum Total?
              <p class="elsa-mt-2 elsa-text-sm elsa-text-gray-500">Indicates that this cell should be used to indicate the total value of all other rows of this column's rows.
                This will be picked up by the front end, and automatically calculated if Javascript is enabled..</p>

            </th>
            <td class="elsa-py-0">
              <input name="choice_input" type="checkbox" checked={sumTotalColumn} value={tableInput.expressions[DataTableSyntax.SumTotalColumn]}
                onChange={e => this.UpdateCheckbox(e, tableInput, DataTableSyntax.SumTotalColumn)}
                class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
            </td>
          </tr>

          <tr style={{ display: optionsDisplay }} >
       
              <th
                class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Pre-Populated Input
            </th>
            <td class="elsa-py-2 pl-5" style={{ width: colWidth }}>
            <div class="elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm">
              <elsa-expression-editor
                key={`expression-editor-${index}-${this.syntaxSwitchCount}`}
                  ref={el => expressionEditor = el}
                  expression={inputExpression}
                language={monacoLanguage}
                single-line={false}
                editorHeight={this.editorHeight}
                  padding="elsa-pt-1.5 elsa-pl-1 elsa-pr-28"
                  onExpressionChanged={e => this.UpdateExpression(e, tableInput, DataTableSyntax.Input)}
              />
              <div class="elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center">
                  <select onChange={e => this.UpdateSyntax(e, tableInput, expressionEditor)}
                    class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md">
                    {supportedSyntaxes.filter(x => x == SyntaxNames.JavaScript).map(supportedSyntax => {
                      const selected = supportedSyntax == SyntaxNames.JavaScript;
                    return <option selected={selected}>{supportedSyntax}</option>;
                  })}
                </select>
              </div>
            </div>
            </td>
            <td
              class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">
              <button type="button" onClick={() => this.onExpandSwitchArea()}
                class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
                <ExpandIcon options={this.iconProvider.getOptions()}></ExpandIcon>
              </button>
            </td>
          </tr>
        </tbody>
      );
    };

    const context: IntellisenseContext = {
      activityTypeName: this.activityModel.type,
      propertyName: this.propertyDescriptor.name
    };

    const selectedType = this.propertyModel.expressions[DataTableSyntax.InputType];
    return (
      <div>



        <div class="elsa-mb-1">
          <div class="elsa-flex">
            <div class="elsa-flex-1">
              <label class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">Table input type</label>
            </div>
          </div>
        </div>

        <div>
          <div>
            <select onChange={e => this.StandardUpdateExpression(e, this.propertyModel, DataTableSyntax.InputType)}
              class="elsa-mt-1 elsa-block focus:elsa-ring-blue-500 StandardUpdateExpression:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md">
              {this.inputOptions.map(inputType => {
                const selected = inputType === selectedType;
                return <option selected={selected} value={inputType}>{inputType}</option>;
              })}
            </select>
          </div>
          <p class="elsa-mt-2 elsa-text-sm elsa-text-gray-500">The type of input that will be accepted by the table in the assessment.</p>
        </div>

        <br />

                <div class="elsa-mb-1">
          <div class="elsa-flex">
            <div class="elsa-flex-1">
              <label class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">Display Group Id</label>
            </div>
          </div>
        </div>

        <div>
          <div>
            <input type="text" value={this.propertyModel.expressions[DataTableSyntax.DisplayGroupId]} onChange={e => this.UpdateInput(e, this.propertyModel, DataTableSyntax.DisplayGroupId)}
              class="focus:elsa-ring-blue-500 focus:elsa-border-bue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
          </div>
          <p class="elsa-mt-2 elsa-text-sm elsa-text-gray-500">This allows you to display this table as a column of a shared table with all matching Group Id's of Tables on this Question Screen..</p>
        </div>

        <br />

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
        
          <button type="button" onClick={() => this.onAddRowClick()}
            class="elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2">
            <PlusIcon options={this.iconProvider.getOptions()}></PlusIcon>
            Add Table Row
          </button>
        </elsa-multi-expression-editor>
      </div>
    );
  }
}
