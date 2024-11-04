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
import { CheckboxOptionsSyntax, PropertyOutputTypes, SyntaxNames, WeightedScoringSyntax } from '../../constants/constants';
import { NestedActivityDefinitionProperty } from '../../models/custom-component-models';
import { SortableComponent, ISortableSharedComponent } from '../base-component';
import SortIcon from '../../icons/sort_icon';
import { DisplayToggle, IDisplayToggle } from '../display-toggle-component';

@Component({
  tag: 'he-weighted-checkbox-option-group-property',
  shadow: false,
})

export class HeWeightedCheckboxOptionGroupProperty implements ISortableSharedComponent, IDisplayToggle {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyModel: NestedActivityDefinitionProperty;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() modelSyntax: string = SyntaxNames.Json;
  @Prop() keyId: string;
  @State() properties: Array<NestedActivityDefinitionProperty> = [];
  @State() iconProvider = new IconProvider();
  @Event() expressionChanged: EventEmitter<string>;
  @State() dictionary: Map<string> = {};
  @State() switchTextHeight: string = "";
  @State() editorHeight: string = "2.75em"
  private _base: SortableComponent;
  private _toggle: DisplayToggle;
  container: HTMLElement;

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid, SyntaxNames.Literal];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxSwitchCount: number = 0;
  scoreSyntaxSwitchCount: number = 0;
  displayValue: string = "table-row";
  hiddenValue: string = "none";

  constructor() {
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

  onDefaultSyntaxValueChanged(e: CustomEvent) {
    this.properties = e.detail;
  }

  onAddAnswerClick() {
    const optionName = newOptionLetter(this._base.IdentifierArray());
    const newAnswer: NestedActivityDefinitionProperty = {
      name: optionName,
      syntax: SyntaxNames.Literal,
      expressions: {
        [SyntaxNames.Literal]: '',
        [CheckboxOptionsSyntax.PrePopulated]: 'false',
      }, type: PropertyOutputTypes.Checkbox
    };
    this.properties = [...this.properties, newAnswer];
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

  multiTextPropertyDescriptor(): ActivityPropertyDescriptor {
    return {
      uiHint: 'multi-text',
      isReadOnly: false,
      name: "Group Array Score",
      hint: "The score for the group, based on the corresponding number of questions answered in the current group.This is not compatable with Question Score Array, and this will be superceded by it.",
      label: "Group Array Score",
      supportedSyntaxes: [SyntaxNames.Literal],
      defaultSyntax: SyntaxNames.Literal,
      considerValuesAsOutcomes: false,
      disableWorkflowProviderSelection: true

    } as ActivityPropertyDescriptor;
  }

  multiTextModel() {
    let jsonModel = this.propertyModel.expressions[WeightedScoringSyntax.GroupArrayScore];
    let model = jsonModel != null && jsonModel != '' ?
      JSON.parse(jsonModel) as NestedActivityDefinitionProperty :
      null;
    if (model != null && model.expressions != null) {
      return model;
    }
    else {
      const newModel: ActivityDefinitionProperty = {
        name: "MultiTextModel",
        expressions: {
          
        }
      };
      return newModel;
    }
  }

  render() {
    const answers = this.properties;
    const supportedSyntaxes = this.supportedSyntaxes;
    const json = JSON.stringify(answers, null, 2);

    const renderCaseEditor = (checkboxAnswer: NestedActivityDefinitionProperty, index: number) => {
      const expression = checkboxAnswer.expressions[checkboxAnswer.syntax];
      const syntax = checkboxAnswer.syntax;
      const monacoLanguage = mapSyntaxToLanguage(syntax);
      const prePopulatedSyntax = SyntaxNames.JavaScript;
      const prePopulatedExpression = checkboxAnswer.expressions[CheckboxOptionsSyntax.PrePopulated];
      const scoreExpression = checkboxAnswer.expressions[CheckboxOptionsSyntax.Score];
      const isSingleChecked = checkboxAnswer.expressions[CheckboxOptionsSyntax.Single] == 'true';
      const isGlobalChecked = checkboxAnswer.expressions[CheckboxOptionsSyntax.ExclusiveToQuestion] == 'true';

      const prePopulatedLanguage = mapSyntaxToLanguage(prePopulatedSyntax);


      let expressionEditor = null;
      let prePopulatedExpressionEditor = null;
      let scoreExpressionEditor = null;
      let colWidth = "100%";
      const optionsDisplay = this.dictionary[index] ?? "none";

      return (
        <tbody key={this.keyId}>
          <tr>
            <th class="sortablejs-custom-handle"><SortIcon options={this.iconProvider.getOptions() }></SortIcon></th>
            <td></td>
            <td class="elsa-pt-1 elsa-pr-2 elsa-text-right">
              <button type="button" onClick={() => this.onDeleteOptionClick(checkboxAnswer)}
                class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
                <TrashCanIcon options={this.iconProvider.getOptions()}></TrashCanIcon>
              </button>
            </td>
          </tr>
          <tr key={`case-${index}`}>
            <th
              class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Identifier
            </th>
            <td class="elsa-py-2" colSpan={2} style={{ width: colWidth }}>
              <input type="text" value={checkboxAnswer.name} onChange={e => this._base.UpdateName(e, checkboxAnswer)}
                class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
            </td>
          </tr>

          <tr>
            <th
              class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Answer
            </th>
            <td class="elsa-py-2" colSpan={2} style={{ width: colWidth }}>
              <div class="elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm elsa-text-box">
                <he-expression-editor
                  key={`expression-editor-${index}-${this.syntaxSwitchCount}-${this.keyId}`}
                  ref={el => expressionEditor = el}
                  expression={expression}
                  language={monacoLanguage}
                  single-line={false}
                  editorHeight={this.editorHeight}
                  padding="elsa-pt-1.5 elsa-pl-1 elsa-pr-28"
                  onExpressionChanged={e => this._base.CustomUpdateExpression(e, checkboxAnswer, checkboxAnswer.syntax)}
                />
                <div class="elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center elsa-select">
                  <select onChange={e => this._base.UpdateSyntax(e, checkboxAnswer, expressionEditor)}
                    class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md elsa-select">
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
              Score
            </th>
            <td class="elsa-py-2" colSpan={2} style={{ width: colWidth }}>
              <div class="elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm elsa-text-box">
                <he-expression-editor
                  key={`expression-editor-${index}-${this.scoreSyntaxSwitchCount}-${this.keyId}`}
                  ref={el => scoreExpressionEditor = el}
                  expression={scoreExpression}
                  language={monacoLanguage}
                  single-line={true}
                  editorHeight={this.editorHeight}
                  padding="elsa-pt-1.5 elsa-pl-1 elsa-pr-28"
                  onExpressionChanged={e => this._base.CustomUpdateExpression(e, checkboxAnswer, CheckboxOptionsSyntax.Score)}
                />
                <div class="elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center elsa-select">
                  <select onChange={e => this._base.UpdateSyntax(e, checkboxAnswer, scoreExpressionEditor)}
                    class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md elsa-select">
                    {this.supportedSyntaxes.filter(x => x == SyntaxNames.Literal).map(supportedSyntax => {
                      const selected = supportedSyntax == SyntaxNames.Literal;
                      return <option selected={selected}>{supportedSyntax}</option>;
                    })}
                  </select>
                </div>
              </div>
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

          <tr style={{ display: optionsDisplay }} >
            <th class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Is Single (Within Group)</th>
            <td class="elsa-py-0">
              <input name="choice_input" type="checkbox" checked={isSingleChecked} value={'true'}
                onChange={e => this._base.UpdateCheckbox(e, checkboxAnswer, CheckboxOptionsSyntax.Single)}
                class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
            </td>
            <td></td>
          </tr>

          <tr style={{ display: optionsDisplay }} >
            <th class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Is Exclusive To Question</th>
            <td class="elsa-py-0">
              <input name="choice_input" type="checkbox" checked={isGlobalChecked} value={'true'}
                onChange={e => this._base.UpdateCheckbox(e, checkboxAnswer, CheckboxOptionsSyntax.ExclusiveToQuestion)}
                class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
            </td>
            <td></td>
          </tr>

          <tr style={{ display: optionsDisplay }}>
            <th class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Pre Populated</th>
            <td class="elsa-py-2" colSpan={2} style={{ width: colWidth }}>
              <div class="elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm elsa-text-box">

                <he-expression-editor
                  key={`expression-editor-${index}-${this.syntaxSwitchCount}`}
                  ref={el => prePopulatedExpressionEditor = el}
                  expression={prePopulatedExpression}
                  language={prePopulatedLanguage}
                  single-line={false}
                  editorHeight="2.75em"
                  padding="elsa-pt-1.5 elsa-pl-1 elsa-pr-28"
                  onExpressionChanged={e => this._base.CustomUpdateExpression(e, checkboxAnswer, CheckboxOptionsSyntax.PrePopulated)}
                />
                <div class="elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center elsa-select">
                  <select onChange={e => this._base.UpdateSyntax(e, checkboxAnswer, prePopulatedExpressionEditor)}
                    class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md elsa-select">
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

    const groupName = "Checkbox group " + this.propertyModel.name + " answers";

    const context: IntellisenseContext = {
      activityTypeName: this.activityModel.type,
      propertyName: groupName
    };

    let multiTextDescriptor = this.multiTextPropertyDescriptor();
    let multiTextModel = this.multiTextModel();


    return (
      <div>
        <br />
        <div class="elsa-mb-1">
          <div class="elsa-flex">
            <div class="elsa-flex-1">
              <label class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">Group Name</label>
            </div>
          </div>
        </div>

        <div>
          <div>
            <input type="text" value={this.propertyModel.name} onChange={e => this._base.UpdateName(e, this.propertyModel)}
              class="focus:elsa-ring-blue-500 focus:elsa-border-bue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
          </div>
          <p class="elsa-mt-2 elsa-text-sm elsa-text-gray-500">The name of the group of Answers.  Each group name must be unique.</p>
        </div>

        <div class="elsa-mb-1">
          <div class="elsa-flex">
            <div class="elsa-flex-1">
              <label class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">Max Group Score</label>
            </div>
          </div>
        </div>

        <div>
          <div>
            <input type="text" value={this.propertyModel.expressions[WeightedScoringSyntax.MaxGroupScore]} onChange={e => this._base.StandardUpdateExpression(e, this.propertyModel, WeightedScoringSyntax.MaxGroupScore)}
              class="focus:elsa-ring-blue-500 focus:elsa-border-bue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
          </div>
          <p class="elsa-mt-2 elsa-text-sm elsa-text-gray-500">Override the maximum score that can be achieved by any number of answers in this group, even if their combined sum is greater.</p>
        </div>

        <div>
            <he-multi-text-property
            activityModel={this.activityModel}
            propertyModel={multiTextModel}
            onExpressionChanged={e => this._base.CustomUpdateExpression(e, this.propertyModel, WeightedScoringSyntax.GroupArrayScore)}
            propertyDescriptor={ multiTextDescriptor }
            >
            </he-multi-text-property>
        </div>

        <br />

        <he-multi-expression-editor
          ref={el => this.multiExpressionEditor = el}
          label={groupName}
          defaultSyntax={SyntaxNames.Json}
          supportedSyntaxes={[SyntaxNames.Json]}
          context={context}
          expressions={{ 'Json': json }}
          editor-height="20rem"
          onExpressionChanged={e => this.onMultiExpressionEditorValueChanged(e)}
          onSyntaxChanged={e => this.onMultiExpressionEditorSyntaxChanged(e)}
        >
          <table class="elsa-min-w-full elsa-divide-y elsa-divide-gray-600" ref={el => (this.container = el as HTMLElement)}>
            {answers.map(renderCaseEditor)}
          </table>

          <button type="button" onClick={() => this.onAddAnswerClick()}
            class="elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2">
            <PlusIcon options={this.iconProvider.getOptions()}></PlusIcon>
            Add Answer
          </button>
        </he-multi-expression-editor>
      </div>
    );
  }
}
