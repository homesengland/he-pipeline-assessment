import { Component, h, Prop, State, Listen } from '@stencil/core';

import TrashCanIcon from '../../icons/trash-can';

import {
  ActivityDefinitionProperty,
  ActivityModel,
  ActivityPropertyDescriptor,
  HTMLElsaMultiExpressionEditorElement,
  //IntellisenseContext,
  SyntaxNames
} from '../../models/elsa-interfaces';

import {
  IQuestionComponent,
  QuestionActivity,
  RadioQuestion,
  CheckboxQuestion,
  TextAreaQuestion
} from '../../models/custom-component-models';

import {
  IconProvider
} from '../icon-provider/icon-provider';

import {
  QuestionLibrary,
  QuestionProvider
} from '../question-provider/question-provider';


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
  tag: 'question-screen-property',
  shadow: false,
})

export class MultiQuestionProperty {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() propertyModel: ActivityDefinitionProperty;
  @Prop() questionProperties: Array<ActivityPropertyDescriptor>;
  @State() questionModel: QuestionActivity = new QuestionActivity();
  @State() iconProvider = new IconProvider();
  @State() questionProvider = new QuestionProvider(Object.values(QuestionLibrary));

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
    this.questionModel = parseJson(choicesJson) || this.defaultActivityModel();
  }

  defaultActivityModel() {
    var activity = new QuestionActivity();
    activity.questions = [];
    return activity;
  }

  updatePropertyModel() {
    this.enforceQuestionIdentifierUniqueness();
    this.propertyModel.expressions[SyntaxNames.Json] = JSON.stringify(this.questionModel);
  }

  updateQuestion(updatedQuestion: IQuestionComponent) {
    let questionToUpdate = this.questionModel.questions.findIndex((obj) => obj.id === updatedQuestion.id);
    let newModel = this.questionModel;
    newModel.questions[questionToUpdate] = updatedQuestion;
    this.questionModel.questions.map(x => x.id != updatedQuestion.id);
    this.questionModel = { ... this.questionModel, questions: newModel.questions };
    this.updatePropertyModel();
  }

  enforceQuestionIdentifierUniqueness() {
    for (let i = 0; i < this.questionModel.questions.length; i++) {
      console.log('enforcing uniqueness')
      this.questionModel.questions[i].id = (i+1).toString();
    }
  }

  hasChoices(questionType: string) {
    if (questionType == QuestionLibrary.Checkbox.nameConstant
      || questionType == QuestionLibrary.Radio.nameConstant)
      return true;
    else return false;
  }

  handleAddQuestion(e: Event) {
    let value = (e.currentTarget as HTMLSelectElement).value.trim();
    let name = (e.currentTarget as HTMLSelectElement).selectedOptions[0].dataset.typename;
    if (value != null && value != "") {
      this.onAddQuestion(value, name);
      let element = e.currentTarget as HTMLSelectElement;
      element.selectedIndex = 0;
    }
  }

  onAddQuestion(questionType: string, questionTypeName: string) {
    let id = (this.questionModel.questions.length + 1).toString();
    const questionName = `Question ${id}`;
    const newQuestion = { id: id, title: questionName, questionGuidance: "", questionText: "", displayComments: false, questionHint: "", questionType: questionType, questionTypeName: questionTypeName };  
    this.questionModel = { ...this.questionModel, questions: [...this.questionModel.questions, newQuestion] };
    this.updatePropertyModel();
  }

  onDeleteQuestionClick(e: Event, question: IQuestionComponent) {
    e.stopPropagation();
    this.questionModel = { ...this.questionModel, questions: this.questionModel.questions.filter(x => x != question) };
    this.updatePropertyModel();
  }

  onAccordionQuestionClick(e: Event) {
    let element = e.currentTarget as HTMLElement;
    element.classList.toggle("active");
    let panel = element.querySelector('.panel') as HTMLElement;
    if (panel.style.display === "block") {
      panel.style.display = "none";
    } else {
      panel.style.display = "block";
    }
  }

  renderQuestions(model: QuestionActivity) {
    return model.questions.map(this.renderChoiceEditor)
  }

  renderChoiceEditor = (question: IQuestionComponent, index: number) => {
    const field = `question-${index}`;
    return (
      <div id={`${field}-id`} class="accordion elsa-mb-4 elsa-rounded" onClick={this.onAccordionQuestionClick}>
        <button type="button elsa-mt-1 elsa-text-m elsa-text-gray-900">{question.title} </button>
        <button type="button" onClick={e => this.onDeleteQuestionClick(e, question)}
          class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none trashcan-icon" style={{ float: "right" }}>
          <TrashCanIcon options={this.iconProvider.getOptions()}></TrashCanIcon>
        </button>
        <p class="elsa-mt-1 elsa-text-sm elsa-text-gray-900">{question.questionTypeName}</p>
        {this.renderQuestionComponent(question)}
      </div>
    );
  };

  renderQuestionComponent(question: IQuestionComponent) {
    switch (question.questionType) {
      case "CheckboxQuestion":
        return <question-checkbox-property onClick={(e) => e.stopPropagation()} class="panel elsa-rounded" question={question as CheckboxQuestion}></question-checkbox-property>;
      case "RadioQuestion":
            return <question-radio-property onClick={(e) => e.stopPropagation()} class="panel elsa-rounded" question={question as RadioQuestion}></question-radio-property>;
        case "TextAreaQuestion":
            return <elsa-textarea-question onClick={(e) => e.stopPropagation()} class="panel elsa-rounded" question={question as TextAreaQuestion}></elsa-textarea-question>;
      default:
        return <question-property ActivityModel={this.activityModel} onClick={(e) => e.stopPropagation()} class="panel elsa-rounded" question={question}></question-property>;
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
        {this.renderQuestions(this.questionModel)}

        <select id="addQuestionDropdown"
          onChange={ (e) => this.handleAddQuestion.bind(this)(e) }
          name="addQuestionDropdown"
          class="elsa-mt-1 elsa-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md">
          <option value="">Add a Question...</option>
          {this.questionProvider.displayOptions()}
        </select>
      </div>
    );
  }
}
