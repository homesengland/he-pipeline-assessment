import { Component, h, Event, EventEmitter, Prop, State } from '@stencil/core';
import {
    ActivityDefinitionProperty,
  ActivityModel,
  ActivityPropertyDescriptor,
  HTMLElsaExpressionEditorElement,
  HTMLElsaMultiExpressionEditorElement,
  IntellisenseContext
} from "../../models/elsa-interfaces";
import { mapSyntaxToLanguage, parseJson, ToLetter, Map } from "../../utils/utils";
import { IconProvider } from "../providers/icon-provider/icon-provider";
import PlusIcon from '../../icons/plus_icon';
import TrashCanIcon from '../../icons/trash-can';
import ExpandIcon from '../../icons/expand_icon';
import { CheckboxOptionsSyntax, PropertyOutputTypes, SyntaxNames, WeightedScoringSyntax } from '../../constants/constants';
import { NestedActivityDefinitionProperty } from '../../models/custom-component-models';
import { ToggleDictionaryDisplay } from '../../functions/display-toggle'

@Component({
  tag: 'he-weighted-checkbox-option-group-property',
  shadow: false,
})
//Copy of Elsa Switch Case
//Copied to allow us control over how the expression editor is displayed.
export class HeWeightedCheckboxOptionGroupProperty {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyModel: NestedActivityDefinitionProperty;
  @State() answers: Array<NestedActivityDefinitionProperty> = [];
  @State() iconProvider = new IconProvider();
  @Event() expressionChanged: EventEmitter<string>;
  @State() optionsDisplayToggle: Map<string> = {};



  @State() switchTextHeight: string = "";

  @State() editorHeight: string = "2.75em"

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid, SyntaxNames.Literal];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxSwitchCount: number = 0;
  scoreSyntaxSwitchCount: number = 0;

  async componentWillLoad() {
    console.log("Loading group component");
    const propertyModel = this.propertyModel;
    const answersJson = propertyModel.expressions[SyntaxNames.Json];
    this.answers = parseJson(answersJson) || [];
  }


  updatePropertyModel() {
    this.propertyModel.expressions[SyntaxNames.Json] = JSON.stringify(this.answers);
    this.multiExpressionEditor.expressions[SyntaxNames.Json] = JSON.stringify(this.answers, null, 2);
    this.expressionChanged.emit(JSON.stringify(this.propertyModel))
  }

  onDefaultSyntaxValueChanged(e: CustomEvent) {
    this.answers = e.detail;
  }

  onAddAnswerClick() {
    const optionName = ToLetter(this.answers.length + 1);
    const newAnswer: NestedActivityDefinitionProperty = {
      name: optionName,
      syntax: SyntaxNames.Literal,
      expressions: {
        [SyntaxNames.Literal]: '',
        [CheckboxOptionsSyntax.PrePopulated]: 'false',
      }, type: PropertyOutputTypes.Checkbox
    };
    this.answers = [...this.answers, newAnswer];
    this.updatePropertyModel();
  }

  onDeleteOptionClick(switchCase: NestedActivityDefinitionProperty) {
    this.answers = this.answers.filter(x => x != switchCase);
    this.updatePropertyModel();
  }

  onOptionNameChanged(e: Event, checkboxOption: NestedActivityDefinitionProperty) {
    checkboxOption.name = (e.currentTarget as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }

  onGroupNameChanged(e: Event) {
    this.propertyModel.name = (e.currentTarget as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }

  onMaxGroupScoreChanged(e: Event) {
    this.propertyModel.expressions[WeightedScoringSyntax.MaxGroupScore] = (e.currentTarget as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }

  onGroupArrayScoreChanged(e: Event) {
    this.propertyModel.expressions[WeightedScoringSyntax.GroupArrayScore] = (e.currentTarget as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }

  onOptionExpressionChanged(e: CustomEvent<string>, checkboxOption: NestedActivityDefinitionProperty) {
    checkboxOption.expressions[checkboxOption.syntax] = e.detail;
    this.updatePropertyModel();
  }

  onScoreExpressionChanged(e: CustomEvent<string>, checkboxOption: NestedActivityDefinitionProperty) {
    checkboxOption.expressions[CheckboxOptionsSyntax.Score] = e.detail;
    this.updatePropertyModel();
  }

  onAnswerSyntaxChanged(e: Event, property: NestedActivityDefinitionProperty, expressionEditor: HTMLElsaExpressionEditorElement) {
    const select = e.currentTarget as HTMLSelectElement;
    property.syntax = select.value;
    expressionEditor.language = mapSyntaxToLanguage(property.syntax);
    this.updatePropertyModel();
  }

  onPrePopulatedChanged(e: CustomEvent<string>, checkbox: NestedActivityDefinitionProperty) {
    checkbox.expressions[CheckboxOptionsSyntax.PrePopulated] = e.detail;
    this.updatePropertyModel();
  }

  onCheckChanged(e: Event, checkbox: NestedActivityDefinitionProperty) {
    const checkboxElement = (e.currentTarget as HTMLInputElement);
    checkbox.expressions[CheckboxOptionsSyntax.Single] = checkboxElement.checked.toString();
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
    this.answers = parsed;
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
    const answers = this.answers;
    const supportedSyntaxes = this.supportedSyntaxes;
    const json = JSON.stringify(answers, null, 2);

    const renderCaseEditor = (checkboxAnswer: NestedActivityDefinitionProperty, index: number) => {
      const expression = checkboxAnswer.expressions[checkboxAnswer.syntax];
      const syntax = checkboxAnswer.syntax;
      const monacoLanguage = mapSyntaxToLanguage(syntax);
      const prePopulatedSyntax = SyntaxNames.JavaScript;
      const prePopulatedExpression = checkboxAnswer.expressions[CheckboxOptionsSyntax.PrePopulated];
      const scoreExpression = checkboxAnswer.expressions[CheckboxOptionsSyntax.Score];
      const checked = checkboxAnswer.expressions[CheckboxOptionsSyntax.Single] == 'true';

      const prePopulatedLanguage = mapSyntaxToLanguage(prePopulatedSyntax);


      let expressionEditor = null;
      let prePopulatedExpressionEditor = null;
      let scoreExpressionEditor = null;
      let colWidth = "100%";
      const optionsDisplay = this.optionsDisplayToggle[index] ?? "none";

      return (
        <tbody>
          <tr key={`case-${index}`}>
            <th
              class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Identifier
            </th>
            <td class="elsa-py-2 elsa-pr-5" style={{ width: colWidth }}>
              <input type="text" value={checkboxAnswer.name} onChange={e => this.onOptionNameChanged(e, checkboxAnswer)}
                class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
            </td>
            <td class="elsa-pt-1 elsa-pr-2 elsa-text-right">
              <button type="button" onClick={() => this.onDeleteOptionClick(checkboxAnswer)}
                class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
                <TrashCanIcon options={this.iconProvider.getOptions()}></TrashCanIcon>
              </button>
            </td>

          </tr>

          <tr>

            <th
              class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Answer
            </th>
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
                  onExpressionChanged={e => this.onOptionExpressionChanged(e, checkboxAnswer)}
                />
                <div class="elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center">
                  <select onChange={e => this.onAnswerSyntaxChanged(e, checkboxAnswer, expressionEditor)}
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

          <tr>
            <th class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">
              Score
            </th>
            <td class="elsa-py-2 pl-5" style={{ width: colWidth }}>
              <div class="elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm">
                <elsa-expression-editor
                  key={`expression-editor-${index}-${this.scoreSyntaxSwitchCount}`}
                  ref={el => scoreExpressionEditor = el}
                  expression={scoreExpression}
                  language={monacoLanguage}
                  single-line={true}
                  editorHeight={this.editorHeight}
                  padding="elsa-pt-1.5 elsa-pl-1 elsa-pr-28"
                  onExpressionChanged={e => this.onScoreExpressionChanged(e, checkboxAnswer)}
                />
                <div class="elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center">
                  <select onChange={e => this.onAnswerSyntaxChanged(e, checkboxAnswer, scoreExpressionEditor)}
                    class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md">
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
            <th class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">IsSingle</th>
            <td class="elsa-py-0">
              <input name="choice_input" type="checkbox" checked={checked} value={'true'}
                onChange={e => this.onCheckChanged(e, checkboxAnswer)}
                class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
            </td>
            <td></td>
          </tr>

          <tr style={{ display: optionsDisplay }}>
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
                  onExpressionChanged={e => this.onPrePopulatedChanged(e, checkboxAnswer)}
                />
                <div class="elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center">
                  <select onChange={e => this.onAnswerSyntaxChanged(e, checkboxAnswer, prePopulatedExpressionEditor)}
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
            <input type="text" value={this.propertyModel.name} onChange={e => this.onGroupNameChanged(e)}
              class="focus:elsa-ring-blue-500 focus:elsa-border-bue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
          </div>
          <p class="elsa-mt-2 elsa-text-sm elsa-text-gray-500">The name of the group of Anwers.  Each group name must be unique.</p>
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
            <input type="text" value={this.propertyModel.expressions[WeightedScoringSyntax.MaxGroupScore]} onChange={e => this.onMaxGroupScoreChanged(e)}
              class="focus:elsa-ring-blue-500 focus:elsa-border-bue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
          </div>
          <p class="elsa-mt-2 elsa-text-sm elsa-text-gray-500">Override the maximum score that can be achieved by any number of answers in this group, even if their combined sum is greater.</p>
        </div>

        <div>
            <he-multi-text-property
            activityModel={this.activityModel}
              propertyModel={multiTextModel}
              onChange={e => this.onGroupArrayScoreChanged(e)}
              propertyDescriptor={ multiTextDescriptor }
            >
            </he-multi-text-property>
        </div>

        <br />

        <elsa-multi-expression-editor
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
          <table class="elsa-min-w-full elsa-divide-y elsa-divide-gray-600">
            {answers.map(renderCaseEditor)}
          </table>

          <button type="button" onClick={() => this.onAddAnswerClick()}
            class="elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2">
            <PlusIcon options={this.iconProvider.getOptions()}></PlusIcon>
            Add Answer
          </button>
        </elsa-multi-expression-editor>
      </div>
    );
  }
}
