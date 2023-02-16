import { Component, h, Prop, State } from '@stencil/core';

import {
  ActivityDefinitionProperty,
  ActivityModel,
  ActivityPropertyDescriptor,
  HTMLElsaMultiExpressionEditorElement,
  IntellisenseContext,
} from '../../models/elsa-interfaces';

import {
    HeActivityPropertyDescriptor,
  QuestionActivity,
  QuestionModel,
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
import { SyntaxNames } from '../../constants/Constants';
import { filterPropertiesByType } from '../../models/utils';


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
  tag: 'question-screen-property-v2',
  shadow: false,
})

export class MultiQuestionProperty {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() propertyModel: ActivityDefinitionProperty;
  @Prop() questionProperties: Array<HeActivityPropertyDescriptor>;
  @State() questionModel: QuestionScreenProperty = new QuestionScreenProperty();
  @State() iconProvider = new IconProvider();
  @State() questionProvider = new QuestionProvider(Object.values(QuestionLibrary));

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxMultiChoiceCount: number = 0;


  async componentWillLoad() {
    console.log("Loading Question Screen:", this.questionProperties)
    const propertyModel = this.propertyModel;
    const choicesJson = propertyModel.expressions[SyntaxNames.QuestionScreen]
    this.questionModel = parseJson(choicesJson) || this.defaultActivityModel();
  }

  defaultActivityModel() {
    var activity = new QuestionActivity();
    activity.questions = [];
    return activity;
  }

  updatePropertyModel() {
    console.log("UpdatePropertyModel:", this.questionModel);
    this.propertyModel.expressions[SyntaxNames.QuestionScreen] = JSON.stringify(this.questionModel);
  }

  onDeleteQuestionClick(e: Event, question: QuestionModel) {
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

  onUpdateQuestion(e: Event) {
    e = e;
    this.updatePropertyModel();
  }

  renderQuestions(model: QuestionScreenProperty) {
    return model.questions.map(this.renderChoiceEditor)
  }

  renderChoiceEditor = (question: QuestionModel, index: number) => {
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
    let data: IQuestionData = JSON.parse((e.currentTarget as HTMLSelectElement).selectedOptions[0].dataset.type)
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
    let filteredValues = filterPropertiesByType(this.questionProperties, questionType.nameConstant);
    console.log("questionProperties", this.questionProperties);
    console.log("FilteredPRoperties", filteredValues);
    let newQuestion: QuestionModel = { value: newValue, descriptor: filterPropertiesByType(this.questionProperties, questionType.nameConstant), questionType: questionType }
    this.questionModel = { ...this.questionModel, questions: [...this.questionModel.questions, newQuestion] };
    
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
    this.questionModel.questions = parsed;
  }

  onMultiExpressionEditorSyntaxChanged(e: CustomEvent<string>) {
    e = e;
  }



  renderQuestionComponent(question: QuestionModel) {
    console.log("Render Question screen property:", question);
    return <question-property
      class="panel elsa-rounded"
      activityModel={this.activityModel}
      questionModel={question}
      onClick={(e) => e.stopPropagation()}
      onUpdateQuestionScreen={e => this.onUpdateQuestion(e)}
    ></question-property>
  }

  render() {
    const context: IntellisenseContext = {
      activityTypeName: this.activityModel.type,
      propertyName: this.propertyDescriptor.name
    };
    const json = JSON.stringify(this.questionModel, null, 2);

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
        </elsa-multi-expression-editor>
      </div>
    );
  }
}
