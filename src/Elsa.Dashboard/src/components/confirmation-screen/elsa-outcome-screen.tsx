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

import {
  IConditionalText,
  Outcome
} from '../../models/custom-component-models';

import {
  IconProvider
} from '../icon-provider/icon-provider';

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
  tag: 'elsa-outcome-screen',
  shadow: false,
})

export class ElsaOutcomeScreen {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() propertyModel: ActivityDefinitionProperty;
  @State() iconProvider = new IconProvider();
  @State() outcome = new Outcome();

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxMultiChoiceCount: number = 0;

  async componentWillLoad() {
    const propertyModel = this.propertyModel;
    const choicesJson = propertyModel.expressions[SyntaxNames.Json]
    this.outcome = parseJson(choicesJson) || this.defaultActivityModel();
  }

  defaultActivityModel() {
    var outcome = new Outcome();
    outcome.outcomeText = [];
    return outcome;
  }

  updatePropertyModel() {
    this.propertyModel.expressions[SyntaxNames.Json] = JSON.stringify(this.outcome);
  }

  onAddOutcome() {
    var placeholderText = "outcome 1";

    var conditionalText = { text : placeholderText, condition : "true" };

    this.outcome = { ...this.outcome, outcomeText: [...this.outcome.outcomeText, conditionalText] };

    this.updatePropertyModel();
  }

  onChangeText(e: Event, outcome: IConditionalText) {

    outcome.text = (e.currentTarget as HTMLInputElement).value.trim();

    this.updatePropertyModel();
  }

  onChangeCondition(e: Event, outcome: IConditionalText) {

    outcome.condition = (e.currentTarget as HTMLInputElement).value.trim();

    this.updatePropertyModel();
  }

  onDelete(outcome: IConditionalText) {

    this.outcome = { ...this.outcome, outcomeText: this.outcome.outcomeText.filter(x => x != outcome) };

    this.updatePropertyModel();
  }


  render() {
    const outcomes = this.outcome.outcomeText;

    const renderOutcomeEditor = (outcome: IConditionalText, index: number) => {

      return (
        <tr key={`choice-${index}`}>
          <td class="elsa-py-2 elsa-pr-5">
            <textarea value={outcome.condition} onChange={e => this.onChangeCondition(e, outcome)}
              class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-min-w-0 elsa-w-full elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
          </td>
          <td class="elsa-py-2 elsa-pr-5">
            <textarea value={outcome.text} onChange={e => this.onChangeText(e, outcome)}
              class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-min-w-0 elsa-w-full elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
          </td>
         
  
          <td class="elsa-pt-1 elsa-pr-2 elsa-text-right">
            <button type="button" onClick={() => this.onDelete(outcome)}
              class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
              <TrashCanIcon options={this.iconProvider.getOptions()} />
            </button>
          </td>
        </tr>
      );
    };

    return (
      <div>
        <table class="elsa-min-w-full elsa-divide-y elsa-divide-gray-200">
          <thead class="elsa-bg-gray-50">
            <tr>
              <th
                class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-5/12">Condition
              </th>
              <th
                class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-6/12">Text
              </th>
              <th
                class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-1/12">&nbsp;</th>
            </tr>
          </thead>
          <tbody>
            {outcomes.map(renderOutcomeEditor)}
          </tbody>
        </table>
        <button type="button" onClick={() => this.onAddOutcome()}
          class="elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2">
          <PlusIcon options={this.iconProvider.getOptions()} />
          Add Outcome
        </button>
       
      </div>
    );
  }
}


