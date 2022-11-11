import { Component, h, Prop, State, Event, EventEmitter } from '@stencil/core';

import {
  HTMLElsaMultiExpressionEditorElement,
  //IntellisenseContext,
  SyntaxNames
} from '../../models/elsa-interfaces';

import {
    MultiChoiceQuestion,
  MultiChoiceRecord,
  QuestionComponent
} from '../../models/custom-component-models';

import {
  IconProvider,
} from '../icon-provider/icon-provider'

import PlusIcon from '../../icons/plus_icon';
import TrashCanIcon from '../../icons/trash-can';


@Component({
  tag: 'elsa-checkbox-question',
  shadow: false,
})

export class MultiQuestionCheckboxComponent {

  @Prop() question: MultiChoiceQuestion
  @State() iconProvider = new IconProvider();

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxMultiChoiceCount: number = 0;

  async componentWillLoad() {
    if (this.question && !this.question.choices) {
      this.question.choices = [];
    }
  }

  @Event({
    eventName: 'updateQuestion',
    composed: true,
    cancelable: true,
    bubbles: true,
  }) updateQuestion: EventEmitter<QuestionComponent>;

  onTitleChanged = (e: Event) => {
    let updatedQuestion = this.question;
    updatedQuestion.title = (e.currentTarget as HTMLInputElement).value.trim();
    this.updateQuestion.emit(updatedQuestion);
  }

  onIdentifierChanged(e: Event) {
    let updatedQuestion = this.question;
    updatedQuestion.id = (e.currentTarget as HTMLInputElement).value.trim();
    this.updateQuestion.emit(updatedQuestion);
  }

  onQuestionChanged(e: Event) {
    let updatedQuestion = this.question;
    updatedQuestion.questionText = (e.currentTarget as HTMLInputElement).value.trim();
    this.updateQuestion.emit(updatedQuestion);
  };

  onGuidanceChanged(e: Event) {
    let updatedQuestion = this.question;
    updatedQuestion.questionGuidance = (e.currentTarget as HTMLInputElement).value.trim();
    this.updateQuestion.emit(updatedQuestion);
  }

  onHintChanged(e: Event) {
    let updatedQuestion = this.question;
    updatedQuestion.questionHint = (e.currentTarget as HTMLInputElement).value.trim();
    this.updateQuestion.emit(updatedQuestion);
  }

  onDisplayCommentsBox(e: Event) {
    let updatedQuestion = this.question;
    const checkbox = (e.target as HTMLInputElement);
    updatedQuestion.displayComments = checkbox.checked;
    this.updateQuestion.emit(updatedQuestion);
  }

  onAddChoiceClick() {
    const choiceName = `Choice ${this.question.choices.length + 1}`;
    const newChoice = { answer: choiceName, isSingle: false };
    this.question = { ... this.question, choices: [... this.question.choices, newChoice] };
    this.updateQuestion.emit(this.question);


  }

  onChoiceNameChanged(e: Event, record: MultiChoiceRecord) {
    record.answer = (e.currentTarget as HTMLInputElement).value.trim();
    this.updateQuestion.emit(this.question);
  }

  onCheckChanged(e: Event, record: MultiChoiceRecord) {
    const checkbox = (e.target as HTMLInputElement);
    record.isSingle = checkbox.checked;
    this.updateQuestion.emit(this.question);
  }

  onDeleteChoiceClick(record: MultiChoiceRecord) {
    this.question = { ...this.question, choices: this.question.choices.filter(x => x != record) }
    this.updateQuestion.emit(this.question);
  }

  renderQuestionField(fieldId, fieldName, fieldValue, onChangedFunction) {
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
      <input type="text" id={fieldId} name={fieldId} value={fieldValue} onChange={e => {
        onChangedFunction.bind(this)(e);
      }
      }
        class="disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
    </div>;
  }

  renderCheckboxField(fieldId, fieldName, isChecked, onChangedFunction) {
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
              onChangedFunction.bind(this)(e)}
              class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
          </div>
        </div>
      </div>
    </div>;
  }

  render() {
    const renderChoiceEditor = (multiChoice: MultiChoiceRecord, index: number) => {
      const field = `choice-${index}`;
      let isChecked = multiChoice.isSingle;
      return (
        <tr key={`choice-${index}`}>
          <td class="elsa-py-2 elsa-pr-5">
            <input type="text" value={multiChoice.answer} onChange={e => this.onChoiceNameChanged.bind(this)(e, multiChoice)}
              class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
          </td>
          <td class="elsa-py-0">
            <input id={`${field}-id`} name="choice_input" type="checkbox" checked={isChecked} value={'true'}
              onChange={e => this.onCheckChanged.bind(this)(e, multiChoice)}
              class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
          </td>

          <td class="elsa-pt-1 elsa-pr-2 elsa-text-right">
            <button type="button" onClick={() => this.onDeleteChoiceClick.bind(this)(multiChoice)}
              class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
              <TrashCanIcon options={this.iconProvider.getOptions()} />
            </button>
          </td>
        </tr>
      );
    };
    const field = `question-${this.question.id}`;
    return (
          <div>

        {this.renderQuestionField(`${field}-questionid`, `Identifier`, this.question.id, this.onIdentifierChanged)}
        {this.renderQuestionField(`${field}-title`, `Title`, this.question.title, this.onTitleChanged)}
        {this.renderQuestionField(`${field}-questionText`, `Question`, this.question.questionText, this.onQuestionChanged)}
        {this.renderQuestionField(`${field}-questionHint`, `Hint`, this.question.questionHint, this.onHintChanged)}
        {this.renderQuestionField(`${field}-questionGuidance`, `Guidance`, this.question.questionGuidance, this.onGuidanceChanged)}
        {this.renderCheckboxField(`${field}-displayCommentBox`, `Display Comments`, this.question.displayComments, this.onDisplayCommentsBox)}

        <div>
          <table class="elsa-min-w-full elsa-divide-y elsa-divide-gray-200">
            <thead class="elsa-bg-gray-50">
              <tr>
                <th
                  class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-10/12">Answer
                </th>
                <th
                  class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-1/12">IsSingle
                </th>
                <th
                  class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-1/12">&nbsp;</th>
              </tr>
            </thead>
            <tbody>
              {this.question.choices.map(renderChoiceEditor)}
            </tbody>
          </table>
          <button type="button" onClick={() => this.onAddChoiceClick.bind(this)()}
            class="elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2">
            <PlusIcon options={this.iconProvider.getOptions()} />
            Add Choice
          </button>
          {/* </elsa-multi-expression-editor> */}
        </div>

          </div>
    );
  }
}
