import { Component, h, Prop, State, Listen } from '@stencil/core';

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
  MultiQuestionActivity,
} from '../../models/custom-component-models';

import {
  IconProvider
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

  @Listen('updateQuestion', { target: "body" })
  getQuestion(event: CustomEvent) {
    if (event.detail) {
      console.log(event);
      console.log(event.detail);
      this.updateQuestion(event.detail);
    }
  }

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

  updateQuestion(updatedQuestion: QuestionComponent) {
    let questionToUpdate = this.multiQuestionModel.questions.findIndex((obj) => obj.id === updatedQuestion.id);
    this.multiQuestionModel.questions[questionToUpdate] = updatedQuestion;
    this.updatePropertyModel();
  }

  onAddQuestionClick(questionType: string) {
    const questionName = `Question ${this.multiQuestionModel.questions.length + 1}`;
    let sampleId = `${this.multiQuestionModel.questions.length + 1}`
    const newQuestion = { id: sampleId, title: questionName, questionGuidance: "", questionText: "", displayComments: false, questionHint: "", questionType: questionType };
    this.multiQuestionModel = { ...this.multiQuestionModel, questions: [...this.multiQuestionModel.questions, newQuestion] };
    this.updatePropertyModel();
  }

  onDeleteQuestionClick(e: Event, question: QuestionComponent) {
    e.stopPropagation();
    this.multiQuestionModel = { ...this.multiQuestionModel, questions: this.multiQuestionModel.questions.filter(x => x != question) };
    this.updatePropertyModel();
  }



  onAccordionQuestionClick(e: Event) {
    let element = e.currentTarget as HTMLElement;
    element.classList.toggle("active");
    console.log(element.parentNode);
    console.log(element.parentNode.querySelector('.panel'));
    let panel = element.querySelector('.panel') as HTMLElement;
    if (panel.style.display === "block") {
      panel.style.display = "none";
    } else {
      panel.style.display = "block";
    }
  }

  renderQuestionField(fieldId, fieldName, fieldValue, multiQuestion, onChangedFunction) {
    return <div>
      <div class="elsa-mb-1 elsa-mt-2">
        <div class="elsa-flex">
          <div class="elsa-flex-1">
            <label htmlFor={fieldId} class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">
              {fieldName}
            </label>
          </div>
        </div>
      </div>
      <input type="text" id={fieldId} name={fieldId} value={fieldValue} onChange={e =>
        onChangedFunction(e, multiQuestion)}
        class="disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
    </div>;
  }

  renderCheckboxField(fieldId, fieldName, isChecked, multiQuestion, onChangedFunction) {
    return <div>
      <div class="elsa-mb-1 elsa-mt-2">
        <div class="elsa-flex">
          <div>
            <input id={fieldId} name={fieldId} type="checkbox" checked={isChecked} value={'true'} onChange={e =>
              onChangedFunction(e, multiQuestion)}
              class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
          </div>
          <div>
            <label htmlFor={fieldId} class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700 elsa-p-1">
              {fieldName}
            </label>
          </div>
        </div>
      </div>
    </div>;
  }

  render() {
    const renderChoiceEditor = (multiQuestion: QuestionComponent, index: number) => {
      const field = `question-${index}`;
      return (
        <div id={`${field}-id`} class="accordion elsa-mb-4 elsa-rounded" onClick={this.onAccordionQuestionClick}>
          <button type="button">Question {index + 1}</button>
          <button type="button" onClick={e => this.onDeleteQuestionClick(e, multiQuestion)}
            class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none trashcan-icon" style={{ float: "right" }}>
            <TrashCanIcon options={this.iconProvider.getOptions()}></TrashCanIcon>
          </button>

          <elsa-question class="panel" question={multiQuestion}></elsa-question>
        </div>
      );
    };
    return (
      <div>
        <div class="elsa-mb-1">
          <div class="elsa-flex">
            <div class="elsa-flex-1">
              <label htmlFor="Questions" class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">
                List of questions
              </label>
            </div>
          </div>
        </div>
        {this.multiQuestionModel.questions.map(renderChoiceEditor)}
        <button type="button" onClick={() => this.onAddQuestionClick("TextQuestion")}
          class="elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2">
          <PlusIcon options={this.iconProvider.getOptions()} />
          Add Question
        </button>
      </div>
    );
  }
}
