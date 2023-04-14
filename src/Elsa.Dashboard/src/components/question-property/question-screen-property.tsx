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
  //QuestionModel,
  NestedPropertyModel,
  QuestionScreenProperty
} from '../../models/custom-component-models';

import {
  IconProvider
} from '../providers/icon-provider/icon-provider';

import {
  IActivityData,
  QuestionLibrary,
  QuestionProvider
} from '../providers/question-provider/question-provider';
import TrashCanIcon from '../../icons/trash-can';
import { QuestionCategories, SyntaxNames } from '../../constants/constants';
import { filterPropertiesByType, parseJson } from '../../utils/utils';

@Component({
  tag: 'question-screen-property',
  shadow: false,
})

export class QuestionScreen {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() propertyModel: ActivityDefinitionProperty;
  @Prop() questionProperties: Array<HeActivityPropertyDescriptor>;
  @State() questionModel: QuestionScreenProperty = new QuestionScreenProperty();
  @State() iconProvider = new IconProvider();
  @State() questionProvider = new QuestionProvider(Object.values(QuestionLibrary));
  @State() questionDropdownDisplay = QuestionCategories.None;

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxMultiChoiceCount: number = 0;


  async componentWillLoad() {
    const propertyModel = this.propertyModel;
    const choicesJson = propertyModel.expressions[SyntaxNames.QuestionList]
    this.questionModel = parseJson(choicesJson) || this.defaultActivityModel();
  }

  defaultActivityModel() {
    var activity = new QuestionScreenProperty();
    activity.activities = [];
    return activity;
  }

  updatePropertyModel() {
    this.propertyModel.expressions[SyntaxNames.QuestionList] = JSON.stringify(this.questionModel);
  }

  onDeleteQuestionClick(e: Event, question: NestedPropertyModel) {
    e.stopPropagation();
    this.questionModel = { ...this.questionModel, activities: this.questionModel.activities.filter(x => x != question) };
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
    return model.activities.map(this.renderChoiceEditor)
  }

  renderChoiceEditor = (question: NestedPropertyModel, index: number) => {
    const field = `question-${index}`;
    return (
      <div id={`${field}-id`} class="accordion elsa-mb-4 elsa-rounded" onClick={this.onAccordionQuestionClick}>
        <button type="button elsa-mt-1 elsa-text-m elsa-text-gray-900">{question.value.name} </button>
        <button type="button" onClick={e => this.onDeleteQuestionClick(e, question)}
          class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none trashcan-icon" style={{ float: "right" }}>
          <TrashCanIcon options={this.iconProvider.getOptions()}></TrashCanIcon>
        </button>
        <p class="elsa-mt-1 elsa-text-sm elsa-text-gray-900">{question.ActivityType.displayName}</p>
        {this.renderQuestionComponent(question)}
      </div>
    );
  };

  handleAddQuestion(e: Event) {
    console.log("Adding Question...");
    let value = (e.currentTarget as HTMLSelectElement).value.trim();
    console.log("value", value);
    let data: IActivityData = JSON.parse((e.currentTarget as HTMLSelectElement).selectedOptions[0].dataset.type)
    console.log("Data", data);
    if (value != null && value != "") {
      this.onAddQuestion(data);
      let element = e.currentTarget as HTMLSelectElement;
      element.selectedIndex = 0;
    }
  }

  handleFilterQuestions(e: Event) {
    let value = (e.currentTarget as HTMLSelectElement).value.trim();
    let data: string =(e.currentTarget as HTMLSelectElement).selectedOptions[0].dataset.type
    if (value != null && value != "") {
      this.onToggleDropdownFilter(data);
    }
  }

  onToggleDropdownFilter(toggleValue: string) {
    this.questionDropdownDisplay = toggleValue;
  }

  displayDropdowns(categoryType: string) {
    if (categoryType == this.questionDropdownDisplay) {
      return true;
    }
    return false;

  }

  newQuestionValue(name: string, id: string) {
    var value = id;
    var defaultSyntax = SyntaxNames.QuestionList;
    var expression: ActivityDefinitionProperty = { name: name, value: value, syntax: defaultSyntax, expressions: { defaultSyntax: value }, }
    return expression;
  }

  onAddQuestion(questionType: IActivityData) {
    let id = (this.questionModel.activities.length + 1).toString();
    const questionName = `Question ${id}`;
    const newValue = this.newQuestionValue(questionName, id);
    let propertyDescriptors = filterPropertiesByType(this.questionProperties, questionType.nameConstant);
    let newQuestion: NestedPropertyModel = { value: newValue, descriptor: propertyDescriptors, ActivityType: questionType }
    this.questionModel = { ...this.questionModel, activities: [...this.questionModel.activities, newQuestion] };
    
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
    this.questionModel.activities = parsed;
  }

  onMultiExpressionEditorSyntaxChanged(e: CustomEvent<string>) {
    e = e;
  }



  renderQuestionComponent(question: NestedPropertyModel) {
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
    const displayScoringQuestions = this.displayDropdowns(QuestionCategories.Scoring) ? "inline-block" : "none";
    const displayStandardQuestions = this.displayDropdowns(QuestionCategories.Question) ? "inline-block" : "none";

    return (
      <div>
        {this.renderQuestions(this.questionModel)}

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

          <select id="addQuestionDropdown"
            onChange={(e) => this.handleFilterQuestions.bind(this)(e)}
            name="toggleCategoryDropdown"
            class="elsa-mt-1 elsa-inline-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md">
            {this.questionProvider.displayDropdownToggleOptions()}
          </select>


        <select id="addQuestionDropdown"
          onChange={ (e) => this.handleAddQuestion.bind(this)(e) }
            name="addQuestionDropdown"
            style={{ display: displayStandardQuestions }}
          class="elsa-mt-1 elsa-inline-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md">
          <option value="">Add a Question...</option>
          {this.questionProvider.displayOptions()}
          </select>

          <select id="addQuestionDropdown"
            onChange={(e) => this.handleAddQuestion.bind(this)(e)}
            name="addScoringQuestionDropdown"
            style={{ display: displayScoringQuestions }}
            class="elsa-mt-1 elsa-inline-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md">
            <option value="">Add a Scoring Question...</option>
            {this.questionProvider.displayScoringOptions()}
          </select>
        </elsa-multi-expression-editor>
      </div>
    );
  }
}
