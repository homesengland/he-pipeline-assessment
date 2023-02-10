import { Component, h, Prop, State } from '@stencil/core';

import {
  ActivityDefinitionProperty,
  ActivityModel,
  ActivityPropertyDescriptor,
  HTMLElsaMultiExpressionEditorElement,
  SyntaxNames
} from '../../models/elsa-interfaces';

import {
  QuestionActivity,
  QuestionProperty,
  QuestionScreenProperty
} from '../../models/custom-component-models';

import {
  IconProvider
} from '../icon-provider/icon-provider';

import {
    IQuestionData,
  QuestionLibrary,
  QuestionProvider
} from '../question-provider/question-provider';
import TrashCanIcon from '../../icons/trash-can';


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
  @State() questionModel: QuestionScreenProperty = new QuestionScreenProperty();
  @State() iconProvider = new IconProvider();
  @State() questionProvider = new QuestionProvider(Object.values(QuestionLibrary));

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxMultiChoiceCount: number = 0;


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
    this.propertyModel.expressions[SyntaxNames.Json] = JSON.stringify(this.questionModel);
  }

  onDeleteQuestionClick(e: Event, question: QuestionProperty) {
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

  renderQuestions(model: QuestionScreenProperty) {
    return model.questions.map(this.renderChoiceEditor)
  }

  renderChoiceEditor = (question: QuestionProperty, index: number) => {
    const field = `question-${index}`;
    return (
      <div id={`${field}-id`} class="accordion elsa-mb-4 elsa-rounded" onClick={this.onAccordionQuestionClick}>
        <button type="button elsa-mt-1 elsa-text-m elsa-text-gray-900">{question.value.name} </button>
        <button type="button" onClick={e => this.onDeleteQuestionClick(e, question)}
          class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none trashcan-icon" style={{ float: "right" }}>
          <TrashCanIcon options={this.iconProvider.getOptions()}></TrashCanIcon>
        </button>
        <p class="elsa-mt-1 elsa-text-sm elsa-text-gray-900">{question.questionType.displayName}</p>
        {this.renderQuestionComponent(question)}
      </div>
    );
  };

  handleAddQuestion(e: Event) {
    let value = (e.currentTarget as HTMLSelectElement).value.trim();
    let data: IQuestionData = JSON.parse((e.currentTarget as HTMLSelectElement).selectedOptions[0].dataset.data)
    if (value != null && value != "") {
      this.onAddQuestion(data);
      let element = e.currentTarget as HTMLSelectElement;
      element.selectedIndex = 0;
    }
  }

  newQuestionValue(name: string) {
    var value = "";
    var defaultSyntax = SyntaxNames.Json;
    var expression: ActivityDefinitionProperty = { name: name, value: value, syntax: defaultSyntax, expressions: { defaultSyntax: value }, }
    return expression;
  }

  onAddQuestion(questionType: IQuestionData) {
    let id = (this.questionModel.questions.length + 1).toString();
    const questionName = `Question ${id}`;
    const newValue = this.newQuestionValue(questionName);

    let newQuestion: QuestionProperty = { value: newValue, descriptor: this.questionProperties, questionType: questionType }
    this.questionModel = { ...this.questionModel, questions: [...this.questionModel.questions, newQuestion] };
    this.updatePropertyModel();
  }



  renderQuestionComponent(question: QuestionProperty) {
    return <question-property onClick={(e) => e.stopPropagation()} class="panel elsa-rounded" question={question}></question-property>
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
