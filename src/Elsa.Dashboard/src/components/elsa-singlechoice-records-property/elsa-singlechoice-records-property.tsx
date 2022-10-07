import { Component, h, Prop, State } from '@stencil/core';

import TrashCanIcon from '../../icons/trash-can';
import PlusIcon from '../../icons/plus_icon';
import {
  ActivityDefinitionProperty,
  ActivityModel,
  ActivityPropertyDescriptor,
  HTMLElsaMultiExpressionEditorElement,
  //IntellisenseContext,
  SyntaxNames
} from '../../models/elsa-interfaces';

//import {
//  SyntaxNames
//} from '@elsa-workflows/elsa-workflows-studio'

import {
  SingleChoiceRecord,
  SingleChoiceActivity
} from '../../models/custom-component-models';

import {
  IconProvider,
} from '../icon-provider/icon-provider'


function parseJson(json: string): any {
  if (!json)
    return null;

  try {
    return JSON.parse(json);
  } catch (e) {
    console.warn(`Error parsing JSON: ${e}`);
  }
  return undefined;
}

@Component({
  tag: 'elsa-singlechoice-records-property',
  shadow: false,
})

export class ElsaSingleChoiceRecordsProperty {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() propertyModel: ActivityDefinitionProperty;
  //@State() choices: Array<MultiChoiceRecord> = []; //TODO - Remove
  @State() singleChoiceModel: SingleChoiceActivity = new SingleChoiceActivity();
  @State() iconProvider = new IconProvider();

  // singleLineProperty: Components.ElsaSingleLineProperty;
  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxMultiChoiceCount: number = 0;

  async componentWillLoad() {
    console.log('component will load');
    const propertyModel = this.propertyModel;
    const choicesJson = propertyModel.expressions[SyntaxNames.Json]
    //this.choices = parseJson(choicesJson) || [];
    this.singleChoiceModel = parseJson(choicesJson) || this.defaultActivityModel();
  }

  defaultActivityModel() {
    var activity = new SingleChoiceActivity();
    activity.choices = [];
    return activity;
  }

  updatePropertyModel() {
    console.log(this.propertyModel);
    //console.log(this.choices);
    this.propertyModel.expressions[SyntaxNames.Json] = JSON.stringify(this.singleChoiceModel);
    // this.multiExpressionEditor.expressions[SyntaxNames.Json] = JSON.stringify(this.choices, null, 2);
  }

  onAddChoiceClick() {
    const choiceName = `Choice ${this.singleChoiceModel.choices.length + 1}`;
    const newChoice = { answer: choiceName};
    this.singleChoiceModel = { ...this.singleChoiceModel, choices: [...this.singleChoiceModel.choices, newChoice] };
    this.updatePropertyModel();
  }

  onDeleteChoiceClick(singleChoice: SingleChoiceRecord) {
    this.singleChoiceModel = { ...this.singleChoiceModel, choices: this.singleChoiceModel.choices.filter(x => x != singleChoice) };
    this.updatePropertyModel();
  }

  onChoiceNameChanged(e: Event, singleChoice: SingleChoiceRecord) {
    singleChoice.answer = (e.currentTarget as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }

  render() {
    const choices = this.singleChoiceModel.choices;
    // const supportedSyntaxes = this.supportedSyntaxes;
    // const json = JSON.stringify(choices, null, 2);

    const renderChoiceEditor = (singleChoice: SingleChoiceRecord, index: number) => {
      // const expression = multiChoice.answer;
      // const monacoLanguage = mapSyntaxToLanguage(syntax);
      // let expressionEditor = null;
      console.log("moo render");
      console.log(singleChoice);
      return (
        <tr key={`choice-${index}`}>
          <td class="elsa-py-2 elsa-pr-5">
            <input type="text" value={singleChoice.answer} onChange={e => this.onChoiceNameChanged(e, singleChoice)}
              class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
          </td>
          <td class="elsa-pt-1 elsa-pr-2 elsa-text-right">
            <button type="button" onClick={() => this.onDeleteChoiceClick(singleChoice)}
              class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
              <TrashCanIcon options={this.iconProvider.getOptions()} />
            </button>
          </td>
        </tr>
      );
    };

    // const context: IntellisenseContext = {
    //   activityTypeName: this.activityModel.type,
    //   propertyName: this.propertyDescriptor.name
    // };
    console.log("moo");
    console.log(choices);
    return (
      <div>
        {/* 
        <elsa-multi-expression-editor
          ref={el => this.multiExpressionEditor = el}
          label={this.propertyDescriptor.label}
          defaultSyntax={SyntaxNames.Literal}
          supportedSyntaxes={[SyntaxNames.Literal]}
          context={context}
          expressions={{'Json': json}}
          editor-height="20rem"
          onExpressionChanged={e => this.onMultiExpressionEditorValueChanged(e)}
          onSyntaxChanged={e => this.onMultiExpressionEditorSyntaxChanged(e)}
        > */}

        <table class="elsa-min-w-full elsa-divide-y elsa-divide-gray-200">
          <thead class="elsa-bg-gray-50">
            <tr>
              <th
                class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-10/12">Answer
              </th>
              <th
                class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-1/12">&nbsp;</th>
            </tr>
          </thead>
          <tbody>
            {choices.map(renderChoiceEditor)}
          </tbody>
        </table>
        <button type="button" onClick={() => this.onAddChoiceClick()}
          class="elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2">
          <PlusIcon options={this.iconProvider.getOptions()} />
          Add Choice
        </button>
        {/* </elsa-multi-expression-editor> */}
      </div>
    );
  }
}
