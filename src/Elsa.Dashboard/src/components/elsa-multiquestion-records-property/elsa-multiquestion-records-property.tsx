import { Component, h, Prop, State, Listen } from '@stencil/core';

import TrashCanIcon from '../../icons/trash-can';
//import PlusIcon from '../../icons/plus_icon';
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
  RadioQuestion,
  CheckboxQuestion,
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
    console.log(this.multiQuestionModel);
  }

  updateQuestion(updatedQuestion: QuestionComponent) {
    let questionToUpdate = this.multiQuestionModel.questions.findIndex((obj) => obj.id === updatedQuestion.id);
    let newModel = this.multiQuestionModel;
    newModel.questions[questionToUpdate] = updatedQuestion;
    this.multiQuestionModel.questions.map(x => x.id != updatedQuestion.id);
    this.multiQuestionModel = { ... this.multiQuestionModel, questions: newModel.questions };
    this.updatePropertyModel();
  }

  handleAddQuestion(e: Event) {
    let value = (e.currentTarget as HTMLInputElement).value.trim();
    if (value != null && value != "") {
      this.onAddQuestion(value);
      let element = e.currentTarget as HTMLSelectElement;
      element.selectedIndex = 0;
    }
  }

  onAddQuestion(questionType: string) {
    console.log('Adding Question');
    const questionName = `Question ${this.multiQuestionModel.questions.length + 1}`;
    let sampleId = `${this.multiQuestionModel.questions.length + 1}`
    const newQuestion = { id: sampleId, title: questionName, questionGuidance: "", questionText: "", displayComments: false, questionHint: "", questionType: questionType };
    this.multiQuestionModel = { ...this.multiQuestionModel, questions: [...this.multiQuestionModel.questions, newQuestion] };
    console.log(newQuestion)
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

  renderQuestions(model: MultiQuestionActivity) {
    return model.questions.map(this.renderChoiceEditor)
  }

  renderChoiceEditor = (multiQuestion: QuestionComponent, index: number) => {
    const field = `question-${index}`;
    return (
      <div id={`${field}-id`} class="accordion elsa-mb-4 elsa-rounded" onClick={this.onAccordionQuestionClick}>
        <button type="button">Question {index + 1} - {multiQuestion.questionType} </button>
        <button type="button" onClick={e => this.onDeleteQuestionClick(e, multiQuestion)}
          class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none trashcan-icon" style={{ float: "right" }}>
          <TrashCanIcon options={this.iconProvider.getOptions()}></TrashCanIcon>
        </button>
        {this.renderQuestionComponent(multiQuestion)}
      </div>
    );
  };

  renderQuestionComponent(question: QuestionComponent) {
    switch (question.questionType) {
      case "MultipleChoiceQuestion":
        return <elsa-checkbox-question onClick={(e) => e.stopPropagation()} class="panel elsa-rounded" question={question as CheckboxQuestion}></elsa-checkbox-question>;
      case "SingleChoiceQuestion":
        return <elsa-radio-question onClick={(e) => e.stopPropagation()} class="panel elsa-rounded" question={question as RadioQuestion}></elsa-radio-question>;
      default:
        return <elsa-question onClick={(e) => e.stopPropagation()} class="panel elsa-rounded" question={question}></elsa-question>;
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
        {this.renderQuestions(this.multiQuestionModel)}

        <select id="addQuestionDropdown"
          onChange={ (e) => this.handleAddQuestion.bind(this)(e) }
          name="addQuestionDropdown"
          class="elsa-mt-1 elsa-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md">
          <option value="">Add a Question...</option>
          <option value="MultipleChoiceQuestion">Checkbox Question</option>
          <option value="CurrencyQuestion">Currency Question</option>
          <option value="DateQuestion">Date Question</option>
          <option value="SingleChoiceQuestion">Radio Question</option>
          <option value="TextQuestion">Text Question</option> 
        </select>
      </div>
    );
  }
}
