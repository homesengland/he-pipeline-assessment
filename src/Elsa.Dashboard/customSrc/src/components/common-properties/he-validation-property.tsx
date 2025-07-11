import { Component, Event, EventEmitter, h, Prop, State } from '@stencil/core';
import {
  ActivityDefinitionProperty,
  ActivityModel,
  ActivityPropertyDescriptor,
  HTMLElsaMultiExpressionEditorElement,
  IntellisenseContext,
} from "../../models/elsa-interfaces";
import { IconProvider } from "../providers/icon-provider/icon-provider";
import { NestedActivityDefinitionProperty } from "../../models/custom-component-models";
import PlusIcon from '../../icons/plus_icon';
import { mapSyntaxToLanguage, parseJson, newOptionLetter, Map } from '../../utils/utils';
import { PropertyOutputTypes, SyntaxNames, ValidationSyntax } from '../../constants/constants';
import { BaseComponent, ISortableSharedComponent } from '../base-component';
import { DisplayToggle, IDisplayToggle } from '../display-toggle-component';
import SortIcon from '../../icons/sort_icon';
import TrashCanIcon from '../../icons/trash-can';

@Component({
  tag: 'he-validation-property',
  shadow: false,
})
export class HeValidationProperty implements ISortableSharedComponent, IDisplayToggle {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() propertyModel: ActivityDefinitionProperty;
  @Prop() modelSyntax: string = SyntaxNames.TextActivityList;
  @State() properties: Array<NestedActivityDefinitionProperty> = [];
  @State() iconProvider = new IconProvider();
  @State() keyId: string;
  @Event() expressionChanged: EventEmitter<string>;

  supportedSyntaxes: Array<string> = [SyntaxNames.Literal, SyntaxNames.JavaScript];

  @State() dictionary: Map<string> = {};
  private _base: BaseComponent;
  private _toggle: DisplayToggle;
  container: HTMLElement;


  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxSwitchCount: number = 0;
  displayValue: string = "table-row";
  hiddenValue: string = "none";

  constructor() {
    this._base = new BaseComponent(this);
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

  onAddElementClick() {
    const textName = newOptionLetter(this._base.IdentifierArray());
    const newTextElement: NestedActivityDefinitionProperty = {
      syntax: SyntaxNames.Literal,
      expressions: { [SyntaxNames.Literal]: '', [ValidationSyntax.ValidationRule]: ''},
      type: PropertyOutputTypes.Validation,
      name: textName
    };
    this.properties = [...this.properties, newTextElement];
    this.updatePropertyModel();
  }

  onHandleDelete(textActivity: NestedActivityDefinitionProperty) {
    this.properties = this.properties.filter(x => x != textActivity);
    this.updatePropertyModel();
  }

  onToggleOptions(index: any) {
    this._toggle.onToggleDisplay(index);
  }

  render() {
    const textElements = this.properties;
    const json = JSON.stringify(textElements, null, 2);
    const renderCaseEditor = (validationRule: NestedActivityDefinitionProperty, index: number) => {  

      const errorMessageSyntax = validationRule.syntax;
      const ruleExpression = validationRule.expressions[ValidationSyntax.ValidationRule];
      const errorMessageExpression = validationRule.expressions[validationRule.syntax];

      const ruleLanguage = mapSyntaxToLanguage(SyntaxNames.JavaScript);
      const errorLanguage = mapSyntaxToLanguage(errorMessageSyntax);

      const conditionEditorHeight = "2.75em";

      let ruleExpressionEditor = null;
      ruleExpressionEditor = ruleExpressionEditor;
      let errorMessageExpressionEditor = null;

      let colWidth = "100%";

      let textContext: IntellisenseContext = {
        activityTypeName: this.activityModel.type,
        propertyName: this.propertyDescriptor.name
      };

      return (
        <tbody key={this.keyId}>

          <tr>
            <th class="sortablejs-custom-handle"><SortIcon options={this.iconProvider.getOptions()}></SortIcon>
            </th>
            <td></td>
            <td class="elsa-pt-1 elsa-pr-2 elsa-text-right">
              <button type="button" onClick={() => this.onHandleDelete(validationRule)}
                class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
                <TrashCanIcon options={this.iconProvider.getOptions()}></TrashCanIcon>
              </button>
            </td>
          </tr>

          <tr>
            <th class="elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Validation Rule
            </th>
            <td class="elsa-py-2 pl-5" colSpan={2} style={{ width: colWidth }}>
              <div class="elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm">
              <he-expression-editor
                key={`expression-editor-${index}-${this.syntaxSwitchCount}`}
                  ref={el => ruleExpressionEditor = el}
                  expression={ruleExpression}
                  language={ruleLanguage}
                  context={textContext}
                  single-line={false}
                editorHeight="2.75em"
                  padding="elsa-pt-1.5 elsa-pl-1 elsa-pr-28"
                  onExpressionChanged={e => this._base.CustomUpdateExpression(e, validationRule, ValidationSyntax.ValidationRule)}
                />
                {/*<div class="elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center">*/}
                  {/*<select onChange={e => this._base.UpdateSyntax(e, validationRule, textExpressionEditor)}*/}
                  {/*  class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md">*/}
                  {/*  {ruleSyntax.map(supportedSyntax => {*/}
                  {/*    const selected = supportedSyntax == textSyntax;*/}
                  {/*    return <option selected={selected}>{supportedSyntax}</option>;*/}
                  {/*  })}*/}
                  {/*</select>*/}
                {/*</div>*/}
              </div>
              <p class="elsa-mt-2 elsa-text-sm elsa-text-gray-500">The validation rule to evaluate for this question.  This must be written in Javascript, and will default to "true" if left blank.</p>
              <p class="elsa-mt-2 elsa-text-sm elsa-text-gray-500">Rules that evaluate as true will pass validation, and rules that evaluate as false will fail, and display the validation message below.</p>
            </td>
          </tr>

          <tr>
              <th
                class="elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Validation Message
            </th>
            <td class="elsa-py-2 pl-5" colSpan={2} style={{ width: colWidth }}>
                <div class="elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm">
                  <he-expression-editor
                    key={`expression-editor-${index}-${this.syntaxSwitchCount}`}
                  ref={el => errorMessageExpressionEditor = el}
                  expression={errorMessageExpression}
                  language={errorLanguage}
                    single-line={false}
                    editorHeight={conditionEditorHeight}
                  padding="elsa-pt-1.5 elsa-pl-1 elsa-pr-28"
                  onExpressionChanged={e => this._base.CustomUpdateExpression(e, validationRule, errorMessageSyntax)}
                  />
                <div class="elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center elsa-select">
                  <select onChange={e => this._base.UpdateSyntax(e, validationRule, errorMessageExpressionEditor)}
                    class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md">
                    {this.supportedSyntaxes.map(supportedSyntax => {
                      const selected = supportedSyntax == errorMessageSyntax;
                      return <option selected={selected}>{supportedSyntax}</option>;
                    })}
                  </select>
                  </div>
              </div>
              <p class="elsa-mt-2 elsa-text-sm elsa-text-gray-500">The error message to display for this question in the front end, if the validation rule returns "false".</p>
              </td>
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

        <he-multi-expression-editor
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

          <table class="elsa-min-w-full elsa-divide-y elsa-divide-gray-200 elsa-table-striped" ref={el => (this.container = el as HTMLElement)}>

             {textElements.map(renderCaseEditor) }

          </table>
          <button type="button" onClick={() => this.onAddElementClick()}
            class="elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2">
            <PlusIcon options={this.iconProvider.getOptions()}></PlusIcon>
            Add Validation Rule
          </button>
        </he-multi-expression-editor>
      </div>
    );
  }
}
