import { Component, h, Event, EventEmitter, Prop, State } from '@stencil/core';
import {
  ActivityDefinitionProperty,
  ActivityModel,
  ActivityPropertyDescriptor,
  HTMLElsaMultiExpressionEditorElement,
  IntellisenseContext
} from "../../models/elsa-interfaces";
import { mapSyntaxToLanguage, parseJson, newOptionLetter, Map } from "../../utils/utils";
import { IconProvider } from "../providers/icon-provider/icon-provider";
import PlusIcon from '../../icons/plus_icon';
import TrashCanIcon from '../../icons/trash-can';
import { PropertyOutputTypes, RadioOptionsSyntax, SyntaxNames } from '../../constants/constants';
import { NestedActivityDefinitionProperty } from '../../models/custom-component-models';
import { SortableComponent, ISortableSharedComponent } from '../base-component';
import SortIcon from '../../icons/sort_icon';
import { DisplayToggle, IDisplayToggle } from '../display-toggle-component';

@Component({
  tag: 'he-potscore-radio-options-property',
  shadow: false,
})

export class HePotScoreRadioOptionProperty implements ISortableSharedComponent, IDisplayToggle {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() propertyModel: ActivityDefinitionProperty;
  @State() keyId: string;
  @State() properties: Array<NestedActivityDefinitionProperty> = [];
  @State() iconProvider = new IconProvider();
  @Event() expressionChanged: EventEmitter<string>;
  @State() dictionary: Map<string> = {};
  @State() switchTextHeight: string = "";
  @State() editorHeight: string = "2.75em"
  displayValue: string = "table-row";
  hiddenValue: string = "none";

  private _base: SortableComponent;
  private _toggle: DisplayToggle;
  modelSyntax: string = SyntaxNames.Json;
  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid, SyntaxNames.Literal];
  potScoreOptions: Array<string> = [];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxSwitchCount: number = 0;

  container: HTMLElement;

  constructor() {
    this._base = new SortableComponent(this);
    this._toggle = new DisplayToggle(this);
  }


  async componentWillLoad() {
    this._base.componentWillLoad();
    this.potScoreOptions = parseJson(this.propertyDescriptor.options);
  }

  async componentDidLoad() {
    this._base.componentDidLoad();
  }

  async componentWillRender() {
    this._base.componentWillRender();
  }


  updatePropertyModel() {
    this.propertyModel.expressions[SyntaxNames.Json] = JSON.stringify(this.properties);
    this.multiExpressionEditor.expressions[SyntaxNames.Json] = JSON.stringify(this.properties, null, 2);
    this.expressionChanged.emit(JSON.stringify(this.propertyModel))
  }

  onDefaultSyntaxValueChanged(e: CustomEvent) {
    this.properties = e.detail;
  }

  onAddOptionClick() {
    const optionName = newOptionLetter(this._base.IdentifierArray());
    const newOption: NestedActivityDefinitionProperty = {
      name: optionName,
      syntax: SyntaxNames.Literal,
      expressions: {
        [SyntaxNames.Literal]: '',
        [RadioOptionsSyntax.PrePopulated]: 'false',
        [RadioOptionsSyntax.PotScore]: this.potScoreOptions[0]
      }, type: PropertyOutputTypes.Radio
    };
    this.properties = [...this.properties, newOption];
    this.updatePropertyModel();
  }

  onDeleteOptionClick(switchCase: NestedActivityDefinitionProperty) {
    this.properties = this.properties.filter(x => x != switchCase);
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
    this.properties = parsed;
  }

  onMultiExpressionEditorSyntaxChanged(e: CustomEvent<string>) {
    e = e;
    this.syntaxSwitchCount++;
  }

  onToggleOptions(index: number) {
    this._toggle.onToggleDisplay(index);
  }

  render() {
    const cases = this.properties;
    const supportedSyntaxes = this.supportedSyntaxes;
    const json = JSON.stringify(cases, null, 2);

    const renderCaseEditor = (radioOption: NestedActivityDefinitionProperty, index: number) => {
      const selectedScore = radioOption.expressions[RadioOptionsSyntax.PotScore];
      const expression = radioOption.expressions[radioOption.syntax];
      const syntax = radioOption.syntax;
      const monacoLanguage = mapSyntaxToLanguage(syntax);
      const prePopulatedSyntax = SyntaxNames.JavaScript;
      const prePopulatedExpression = radioOption.expressions[RadioOptionsSyntax.PrePopulated];

      const prePopulatedLanguage = mapSyntaxToLanguage(prePopulatedSyntax);

      let expressionEditor = null;
      let prePopulatedExpressionEditor = null;
      let colWidth = "100%";
      const optionsDisplay = this.dictionary[index] ?? "none";

      return (
        <tbody key={this.keyId}>
          <tr>
            <th class="sortablejs-custom-handle"><SortIcon options={this.iconProvider.getOptions()}></SortIcon>
            </th>
            <td></td>
            <td class="elsa-pt-1 elsa-pr-2 elsa-text-right">
              <button type="button" onClick={() => this.onDeleteOptionClick(radioOption)}
                class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
                <TrashCanIcon options={this.iconProvider.getOptions()}></TrashCanIcon>
              </button>
            </td>
          </tr>
          <tr>
            <th
              class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Identifier
            </th>
            <td class="elsa-py-2" colSpan={2} style={{ width: colWidth }}>
              <input type="text" value={radioOption.name} onChange={e => this._base.UpdateName(e, radioOption)}
                class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
            </td>
          </tr>

          <tr>

            <th
              class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Answer
            </th>
            <td class="elsa-py-2" colSpan={2} style={{ width: colWidth }}>
              <div class="elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm">
                <elsa-expression-editor
                  key={`expression-editor-${index}-${this.syntaxSwitchCount}-${this.keyId}`}
                  ref={el => expressionEditor = el}
                  expression={expression}
                  language={monacoLanguage}
                  single-line={false}
                  editorHeight={this.editorHeight}
                  padding="elsa-pt-1.5 elsa-pl-1 elsa-pr-28"
                  onExpressionChanged={e => this._base.CustomUpdateExpression(e, radioOption, radioOption.syntax)}
                />
                <div class="elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center">
                  <select onChange={e => this._base.UpdateSyntax(e, radioOption, expressionEditor)}
                    class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md">
                    {supportedSyntaxes.map(supportedSyntax => {
                      const selected = supportedSyntax == syntax;
                      return <option selected={selected}>{supportedSyntax}</option>;
                    })}
                  </select>
                </div>
              </div>
            </td>
          </tr>

          <tr>
            <th class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">
              Pot Score
            </th>
            <td class="elsa-py-2" style={{ width: colWidth }}>
              <select onChange={e => this._base.UpdateDropdown(e, radioOption, RadioOptionsSyntax.PotScore)}
                class="elsa-mt-1 elsa-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md">
                {this.potScoreOptions.map(potScore => {
                  const selected = potScore.trim() === selectedScore;
                  return <option selected={selected} value={potScore}>{potScore}</option>;
                })}
              </select>
            </td>
            <td></td>
          </tr>

          <tr onClick={() => this.onToggleOptions(index)}>
            <th
              class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-text-left elsa-tracking-wider elsa-w-2/12" colSpan={3} style={{ cursor: "zoom-in" }}> Options
            </th>
            <td></td>
            <td></td>
          </tr>

          <tr style={{ display: optionsDisplay }}>
            <th class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Pre Populated</th>
            <td class="elsa-py-2" style={{ width: colWidth }}>
              <div class="elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm">
                <elsa-expression-editor
                  key={`expression-editor-${index}-${this.syntaxSwitchCount}-${this.keyId}`}
                  ref={el => prePopulatedExpressionEditor = el}
                  expression={prePopulatedExpression}
                  language={prePopulatedLanguage}
                  single-line={false}
                  editorHeight="2.75em"
                  padding="elsa-pt-1.5 elsa-pl-1 elsa-pr-28"
                  onExpressionChanged={e => this._base.CustomUpdateExpression(e, radioOption, RadioOptionsSyntax.PrePopulated)}
                />
                <div class="elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center">
                  <select onChange={e => this._base.UpdateSyntax(e, radioOption, prePopulatedExpressionEditor)}
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
          <table class="elsa-min-w-full elsa-divide-y elsa-divide-gray-200" ref={el => (this.container = el as HTMLElement) }>
            {cases.map(renderCaseEditor)}
          </table>

          <button type="button" onClick={() => this.onAddOptionClick()}
            class="elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2">
            <PlusIcon options={this.iconProvider.getOptions()}></PlusIcon>
            Add Answer
          </button>
        </elsa-multi-expression-editor>
      </div>
    );
  }
}

