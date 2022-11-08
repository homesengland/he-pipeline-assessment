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
  QuestionComponent,
  MultiQuestionActivity
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
  tag: 'elsa-multiquestion-property',
  shadow: false,
})

export class ElsaMultiQuestionRecordsProperty {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() propertyModel: ActivityDefinitionProperty;
  @State() multiQuestionModel: MultiQuestionActivity = new MultiQuestionActivity();
  @State() iconProvider = new IconProvider();

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxMultiChoiceCount: number = 0;

  async componentWillLoad() {
    const propertyModel = this.propertyModel;
    const choicesJson = propertyModel.expressions[SyntaxNames.Json]
    this.multiQuestionModel = parseJson(choicesJson) || this.defaultActivityModel();
  }

  defaultActivityModel() {
    var activity = new MultiQuestionActivity();
    activity.questions = [];
    return activity;
  }

  updatePropertyModel() {
    this.propertyModel.expressions[SyntaxNames.Json] = JSON.stringify(this.multiQuestionModel);
  }

  onAddQuestionClick(questionType: string) {
    const questionName = `Question ${this.multiQuestionModel.questions.length + 1}`;
    let sampleId = `${this.multiQuestionModel.questions.length + 1}`
    const newQuestion = { id: sampleId, title: questionName, questionGuidance: "", questionText: "", displayComments: false, questionHint: "", questionType: questionType };
    this.multiQuestionModel = { ...this.multiQuestionModel, questions: [...this.multiQuestionModel.questions, newQuestion] };
    this.updatePropertyModel();
  }

  onDeleteChoiceClick(question: QuestionComponent) {
    this.multiQuestionModel = { ...this.multiQuestionModel, questions: this.multiQuestionModel.questions.filter(x => x != question) };
    this.updatePropertyModel();
  }

  onChoiceTitleChanged(e: Event, question: QuestionComponent) {
    question.title = (e.currentTarget as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }

  onChoiceIdentifierChanged(e: Event, question: QuestionComponent) {
    question.id = (e.currentTarget as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }

  onChoiceQuestionChanged(e: Event, question: QuestionComponent) {
    question.questionText = (e.currentTarget as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }

  onChoiceGuidanceChanged(e: Event, question: QuestionComponent) {
    question.questionGuidance = (e.currentTarget as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }

  onChoiceHintChanged(e: Event, question: QuestionComponent) {
    question.questionHint = (e.currentTarget as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }

  onDisplayCommentsBox(e: Event, question: QuestionComponent) {
    const checkbox = (e.target as HTMLInputElement);
    question.displayComments = checkbox.checked;
    this.updatePropertyModel();
  }

  render() {
    const questions = this.multiQuestionModel.questions;

    const renderChoiceEditor = (multiChoice: QuestionComponent, index: number) => {
      const propertyDescriptor = this.propertyDescriptor;
      const propertyName = propertyDescriptor.name;
      const fieldId = propertyName;
      const fieldName = propertyName;
      return (
        <tr key={`choice-${index}`}>
          <td class="elsa-py-2 elsa-pr-5">
            <input type="text" value={multiChoice.id} onChange={e => this.onChoiceIdentifierChanged(e, multiChoice)}
              class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
          </td>
          <td class="elsa-py-2 elsa-pr-5">
            <input type="text" value={multiChoice.title} onChange={e => this.onChoiceTitleChanged(e, multiChoice)}
              class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
          </td>
          <td class="elsa-py-2 elsa-pr-5">
            <input type="text" value={multiChoice.questionText} onChange={e => this.onChoiceQuestionChanged(e, multiChoice)}
              class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
          </td>
          <td class="elsa-py-2 elsa-pr-5">
            <input type="text" value={multiChoice.questionHint} onChange={e => this.onChoiceHintChanged(e, multiChoice)}
              class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
          </td>
          <td class="elsa-py-2 elsa-pr-5">
            <input type="text" value={multiChoice.questionGuidance} onChange={e => this.onChoiceGuidanceChanged(e, multiChoice)}
              class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
          </td>
          <td class="elsa-py-0">
            <input id={fieldId} name={fieldName} type="checkbox" checked={multiChoice.displayComments} value={'true'}
              onChange={e => this.onDisplayCommentsBox(e, multiChoice)}
                     class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded"/>
                     </td> 
        
          <td class="elsa-pt-1 elsa-pr-2 elsa-text-right">
            <button type="button" onClick={() => this.onDeleteChoiceClick(multiChoice)}
              class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
              <TrashCanIcon options={this.iconProvider.getOptions()}/>
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
                class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-10/12">Identifier
              </th>
              <th
                class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-10/12">Title
              </th>
              <th
                class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-10/12">Question
              </th>
              <th
                class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-10/12">Hint
              </th>
              <th
                class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-10/12">Guidance
              </th>
              <th
                class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-1/12">Display Comments
              </th>
              <th
                class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-1/12">&nbsp;</th>
            </tr>
            </thead>
            <tbody>
            {questions.map(renderChoiceEditor)}
            </tbody>
        </table>
        <button type="button" onClick={() => this.onAddQuestionClick("TextQuestion")}
                  class="elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2">
          <PlusIcon options={this.iconProvider.getOptions()} />
            Add Choice
          </button>
        {/* </elsa-multi-expression-editor> */}
      </div>
    );
  }
}
