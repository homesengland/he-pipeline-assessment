import { Component, h, Prop, State, Event, EventEmitter } from '@stencil/core';

import {
  HTMLElsaMultiExpressionEditorElement,
  //IntellisenseContext,
  SyntaxNames
} from '../../models/elsa-interfaces';

import {
  QuestionComponent
} from '../../models/custom-component-models';

import {
  IconProvider,
} from '../icon-provider/icon-provider'


@Component({
  tag: 'elsa-question',
  shadow: false,
})

export class MultiQuestionComponent {

  @Prop() question: QuestionComponent
  @State() iconProvider = new IconProvider();

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxMultiChoiceCount: number = 0;

  async componentWillLoad() {

  }

  /*  @Event({ bubbles: true, composed: true }) questionChanged: EventEmitter<QuestionComponent>;*/
  @Event({
    eventName: 'updateQuestion',
    composed: true,
    cancelable: true,
    bubbles: true,
  }) updateQuestion: EventEmitter<QuestionComponent>;


  updatePropertyModel(question: QuestionComponent) {
    console.log('updating parent')
    console.log(question);
    this.updateQuestion.emit(question);
  }

  onTitleChanged = (e: Event, updatedQuestion: QuestionComponent, eventEmitter: EventEmitter) =>  {
    console.log('Setting title');
    updatedQuestion.title = (e.currentTarget as HTMLInputElement).value.trim();
    eventEmitter.emit(updatedQuestion);
  }


  onIdentifierChanged(e: Event, updatedQuestion: QuestionComponent, eventEmitter: EventEmitter) {
    updatedQuestion.id = (e.currentTarget as HTMLInputElement).value.trim();
    eventEmitter.emit(updatedQuestion);
  }

  onQuestionChanged(e: Event, updatedQuestion: QuestionComponent, eventEmitter: EventEmitter) {
    updatedQuestion.questionText = (e.currentTarget as HTMLInputElement).value.trim();
    eventEmitter.emit(updatedQuestion);
  };

  onGuidanceChanged(e: Event, updatedQuestion: QuestionComponent, eventEmitter: EventEmitter) {
    updatedQuestion.questionGuidance = (e.currentTarget as HTMLInputElement).value.trim();
    eventEmitter.emit(updatedQuestion);
  }

  onHintChanged(e: Event, updatedQuestion: QuestionComponent, eventEmitter: EventEmitter) {
    updatedQuestion.questionHint = (e.currentTarget as HTMLInputElement).value.trim();
    eventEmitter.emit(updatedQuestion);
  }

  onDisplayCommentsBox(e: Event, updatedQuestion: QuestionComponent, eventEmitter: EventEmitter) {
    const checkbox = (e.target as HTMLInputElement);
    updatedQuestion.displayComments = checkbox.checked;
    eventEmitter.emit(updatedQuestion);
  }

  renderQuestionField(fieldId, fieldName, fieldValue, onChangedFunction, questionToUpdate) {
    return <div>
      <div class="elsa-mb-1">
        <div class="elsa-flex">
          <div class="elsa-flex-1">
            <label htmlFor={fieldId} class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">
              {fieldName}
            </label>
          </div>
        </div>
      </div>
      <input type="text" id={fieldId} name={fieldId} value={fieldValue} onChange={e =>
        onChangedFunction(e, questionToUpdate, this.updateQuestion)}
        class="disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
    </div>;
  }

  renderCheckboxField(fieldId, fieldName, isChecked, onChangedFunction, questionToUpdate) {
    return <div>
      <div class="elsa-mb-1 elsa-mt-2">
        <div class="elsa-flex">
          <div>
            <label htmlFor={fieldId} class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700 elsa-p-1">
              {fieldName}
            </label>
          </div>
          <div>
            <input id={fieldId} name={fieldId} type="checkbox" checked={isChecked} value={'true'} onChange={e =>
              onChangedFunction(e, questionToUpdate)}
              class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
          </div>
        </div>
      </div>
    </div>;
  }

  render() {
    const field = `question-${this.question.id}`;
    console.log('rendering');
    console.log(this.question);
    return (
          <div>

        {this.renderQuestionField(`${field}-questionid`, `Identifier`, this.question.id, this.onIdentifierChanged, this.question)}
        {this.renderQuestionField(`${field}-title`, `Title`, this.question.title, this.onTitleChanged, this.question)}
        {this.renderQuestionField(`${field}-questionText`, `Question`, this.question.questionText, this.onQuestionChanged, this.question)}
        {this.renderQuestionField(`${field}-questionHint`, `Hint`, this.question.questionHint, this.onHintChanged, this.question)}
        {this.renderQuestionField(`${field}-questionGuidance`, `Guidance`, this.question.questionGuidance, this.onGuidanceChanged, this.question)}
          {this.renderCheckboxField(`${field}-displayCommentBox`, `Display Comments`, this.question.displayComments, this.onDisplayCommentsBox, this.question)}

          </div>

      //<div>
      //  <div>
      //    <input type="text" id={this.question.id + "_title"} name="title" value={this.question.title} onChange={e => this.onTitleChanged(e)}
      //         class="disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300"
      //         disabled={false}/>
      //    {/*<input type="text" value={this.question.title} onChange={e => this.onTitleChanged(e)}*/}
      //    {/*  class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />*/}
      //    <br/>
      //    <input type="text" value={this.question.questionText} onChange={e => this.onQuestionChanged(e)}
      //      class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
      //    <br />
      //    <input type="text" value={this.question.questionHint} onChange={e => this.onHintChanged(e, this.question)}
      //      class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
      //    <br />
      //    <textarea value={this.question.questionGuidance} onChange={e => this.onGuidanceChanged(e, this.question)}
      //      class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
      //    <br />
      //    <input id={"displayCommentsCheckbox_" + this.question.id} name={"displayCommentsCheckbox_" + this.question.id} type="checkbox" checked={this.question.displayComments} value={'true'}
      //      onChange={e => this.onDisplayCommentsBox(e, this.question)}
      //      class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
      //  </div>
      //</div>
    );
  }
}
