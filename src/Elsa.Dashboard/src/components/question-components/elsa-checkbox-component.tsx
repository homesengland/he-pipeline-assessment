import { Component, h, Prop, State, Event, EventEmitter } from '@stencil/core';

import {
  HTMLElsaMultiExpressionEditorElement,
  //IntellisenseContext,
  SyntaxNames
} from '../../models/elsa-interfaces';

import {
  CheckboxQuestion,
  CheckboxOption,
  IQuestionComponent,
  QuestionOptions
} from '../../models/custom-component-models';

import {
  IconProvider,
} from '../icon-provider/icon-provider'

import PlusIcon from '../../icons/plus_icon';
import TrashCanIcon from '../../icons/trash-can';
import { CheckboxEventHandler } from '../../events/component-events';


@Component({
  tag: 'elsa-checkbox-question',
  shadow: false,
})

export class ElsaCheckboxComponent {

  @Prop() question: CheckboxQuestion
  @State() iconProvider = new IconProvider();

  handler: CheckboxEventHandler;

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxMultiChoiceCount: number = 0;

  @Event({
    eventName: 'updateQuestion',
    composed: true,
    cancelable: true,
    bubbles: true,
  }) updateQuestion: EventEmitter<IQuestionComponent>;


  async componentWillLoad() {
    if (this.question && !this.question.checkbox) {
      this.question.checkbox = new QuestionOptions<CheckboxOption>();
    }
    this.handler = new CheckboxEventHandler(this.question, this.updateQuestion);
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
    const renderChoiceEditor = (multiChoice: CheckboxOption, index: number) => {
      const field = `choice-${index}`;
      let isChecked = multiChoice.isSingle;
      return (
        <tr key={`choice-${index}`}>
          <td class="elsa-py-2 elsa-pr-5">
            <input type="text" value={multiChoice.identifier} disabled onChange={e => this.handler.onChoiceIdentifierChanged.bind(this)(e, multiChoice)}
              class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
          </td>
          <td class="elsa-py-2 elsa-pr-5">
            <input type="text" value={multiChoice.answer} onChange={e => this.handler.onChoiceNameChanged.bind(this)(e, multiChoice)}
              class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
          </td>
          <td class="elsa-py-0">
            <input id={`${field}-id`} name="choice_input" type="checkbox" checked={isChecked} value={'true'}
              onChange={e => this.handler.onCheckChanged.bind(this)(e, multiChoice)}
              class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
          </td>

          <td class="elsa-pt-1 elsa-pr-2 elsa-text-right">
            <button type="button" onClick={() => this.handler.onDeleteChoiceClick.bind(this)(multiChoice)}
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

        {this.renderQuestionField(`${field}-questionid`, `Identifier`, this.question.id, this.handler.onIdentifierChanged)}
        {this.renderQuestionField(`${field}-title`, `Question Name`, this.question.title, this.handler.onTitleChanged)}
        {this.renderQuestionField(`${field}-questionText`, `Question`, this.question.questionText, this.handler.onQuestionChanged)}
        {this.renderQuestionField(`${field}-questionHint`, `Hint`, this.question.questionHint, this.handler.onHintChanged)}
        {this.renderQuestionField(`${field}-questionGuidance`, `Guidance`, this.question.questionGuidance, this.handler.onGuidanceChanged)}
        {this.renderCheckboxField(`${field}-displayCommentBox`, `Display Comments`, this.question.displayComments, this.handler.onDisplayCommentsBox)}

        <div>
          <table class="elsa-min-w-full elsa-divide-y elsa-divide-gray-200">
            <thead class="elsa-bg-gray-50">
              <tr>
                <th
                  class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Identifier
                </th>
                <th
                  class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-8/12">Answer
                </th>
                <th
                  class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-1/12">IsSingle
                </th>
                <th
                  class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-1/12">&nbsp;</th>
              </tr>
            </thead>
            <tbody>
              {this.question.checkbox.choices.map(renderChoiceEditor)}
            </tbody>
          </table>
          <button type="button" onClick={() => this.handler.onAddChoiceClick.bind(this)()}
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