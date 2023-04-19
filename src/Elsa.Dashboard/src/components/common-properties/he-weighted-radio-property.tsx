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
//import TrashCanIcon from '../../icons/trash-can';
//import ExpandIcon from '../../icons/expand_icon';
import { PropertyOutputTypes, RadioOptionsSyntax, SyntaxNames } from '../../constants/constants';
import { NestedActivityDefinitionProperty } from '../../models/custom-component-models';
import TrashCanIcon from '../../icons/trash-can';

@Component({
  tag: 'he-weighted-radio-property',
  shadow: false,
})
//Copy of Elsa Switch Case
//Copied to allow us control over how the expression editor is displayed.
export class HeWeightedRadioProperty {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() propertyModel: ActivityDefinitionProperty;
  @State() groups: Array<NestedActivityDefinitionProperty> = [];
  @State() iconProvider = new IconProvider();
  @Event() expressionChanged: EventEmitter<string>;
  @State() optionsDisplayToggle: Map<string> = {};



  @State() switchTextHeight: string = "";

  @State() editorHeight: string = "2.75em"

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid, SyntaxNames.Literal];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxSwitchCount: number = 0;
  scoreSyntaxSwitchCount: number= 0;

  async componentWillLoad() {
    const propertyModel = this.propertyModel;
    const optionsJson = propertyModel.expressions[SyntaxNames.Json];
    this.groups = parseJson(optionsJson) || [];
  }


  updatePropertyModel() {
    this.propertyModel.expressions[SyntaxNames.Json] = JSON.stringify(this.groups);
    this.multiExpressionEditor.expressions[SyntaxNames.Json] = JSON.stringify(this.groups, null, 2);
    this.expressionChanged.emit(JSON.stringify(this.propertyModel))
  }

  onDefaultSyntaxValueChanged(e: CustomEvent) {
    this.groups = e.detail;
  }

  onAddGroupClick() {
    const groupName = ToLetter(this.groups.length + 1);
    const newGroup: NestedActivityDefinitionProperty = {
      name: groupName,
      syntax: SyntaxNames.Json,
      expressions: {
        [SyntaxNames.Json]: '',
      }, type: PropertyOutputTypes.RadioGroup
    };
    this.groups = [... this.groups, newGroup];
    this.updatePropertyModel();
    console.log("Added Group, property Updated");
  }

  onDeleteGroupClick(radioGroup: NestedActivityDefinitionProperty) {
    this.groups = this.groups.filter(x => x != radioGroup);
    this.updatePropertyModel();
  }

  onGroupNameChanged(e: Event, radioOption: NestedActivityDefinitionProperty) {
    radioOption.name = (e.currentTarget as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }

  onGroupExpressionChanged(e: CustomEvent<string>, radioOption: NestedActivityDefinitionProperty) {
    radioOption.expressions[radioOption.syntax] = e.detail;
    this.updatePropertyModel();
  }

  onScoreExpressionChanged(e: CustomEvent<string>, radioOption: NestedActivityDefinitionProperty) {
    radioOption.expressions[RadioOptionsSyntax.Score] = e.detail;
    this.updatePropertyModel();
  }

  onOptionSyntaxChanged(e: Event, property: NestedActivityDefinitionProperty, expressionEditor: HTMLElsaExpressionEditorElement) {
    const select = e.currentTarget as HTMLSelectElement;
    property.syntax = select.value;
    expressionEditor.language = mapSyntaxToLanguage(property.syntax);
    this.updatePropertyModel();
  }

  onPotScoreChanged(e: Event, property: NestedActivityDefinitionProperty) {
    const select = e.currentTarget as HTMLSelectElement;
    property.expressions[RadioOptionsSyntax.PotScore] = select.value;
    this.updatePropertyModel();
  }

  onPrePopulatedChanged(e: CustomEvent<string>, radio: NestedActivityDefinitionProperty) {
    radio.expressions[RadioOptionsSyntax.PrePopulated] = e.detail;
    this.updatePropertyModel();
  }

  onPropertyExpressionChange(event: Event, property: NestedActivityDefinitionProperty) {
    console.log("Expression changed");
    event = event;
    property = property;
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
    this.groups = parsed;
  }

  onMultiExpressionEditorSyntaxChanged(e: CustomEvent<string>) {
    e = e;
    this.syntaxSwitchCount++;
  }

  onExpandSwitchArea() {
    this.editorHeight == "2.75em" ? this.editorHeight = "8em" : this.editorHeight = "2.75em"
  }

  onToggleOptions(index: number) {
    let tempValue = Object.assign(this.optionsDisplayToggle);
    let tableRowClass = this.optionsDisplayToggle[index];
    if (tableRowClass == null) {
      tempValue[index] = "table-row";
    } else {
      this.optionsDisplayToggle[index] == "none" ? tempValue[index] = "table-row" : tempValue[index] = "none";
    }
    this.optionsDisplayToggle = { ... this.optionsDisplayToggle, tempValue }
  }

  render() {
    const answerGroups = this.groups;
    const json = JSON.stringify(answerGroups, null, 2);

    const renderRadioGroups = (radioGroup: NestedActivityDefinitionProperty) => {

    const eventHandler = this.onPropertyExpressionChange.bind(this);

      return (
        <div>
          <br/>
          <div class="elsa-mb-1">
            <div class="elsa-flex">
              <div class="elsa-flex-1 elsa-mx-auto">
                <h3>Group: {radioGroup.name}</h3>
              </div>
              <div>
                <button type="button" onClick={() => this.onDeleteGroupClick(radioGroup)}
                  class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
                  <TrashCanIcon options={this.iconProvider.getOptions()}></TrashCanIcon>
                </button>
              </div>
            </div>
          </div>


      <he-weighted-radio-option-group-property
        activityModel={this.activityModel}
          propertyModel={radioGroup}
            onExpressionChanged={e => eventHandler(e, radioGroup)}>
        </he-weighted-radio-option-group-property>
        <br/>
          <hr/>
        </div>
      )
    }

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
          <hr/>
          <div class="elsa-min-w-full elsa-divide-y elsa-divide-gray-200">
            {answerGroups.map(renderRadioGroups)}
          </div>

          <button type="button" onClick={() => this.onAddGroupClick()}
            class="elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2">
            <PlusIcon options={this.iconProvider.getOptions()}></PlusIcon>
            Add Answer Group
          </button>
        </elsa-multi-expression-editor>
      </div>
    );
  }
}

