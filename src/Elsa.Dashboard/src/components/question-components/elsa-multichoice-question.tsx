import { Component, h, Prop, State } from '@stencil/core';

import {
  HTMLElsaMultiExpressionEditorElement,
  //IntellisenseContext,
  SyntaxNames
} from '../../models/elsa-interfaces';

import {
  Question, QuestionComponent
} from '../../models/custom-component-models';

import {
  IconProvider,
} from '../icon-provider/icon-provider'


@Component({
  tag: 'elsa-question',
  shadow: false,
})

export class MultiQuestionComponent {

  @Prop() questionType: string;
  @Prop() updateParentCallback: Function;
  @Prop() id: string;
  @Prop() questionSeed: QuestionComponent
  @State() question: Question = new Question();
  @State() iconProvider = new IconProvider();

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxMultiChoiceCount: number = 0;

  async componentWillLoad() {
    this.question = this.defaultActivityModel();
  }

  defaultActivityModel() {
    var activity = new Question();
    activity.id = this.id;
    activity.questionType = this.questionType;
    return activity;
  }

  updatePropertyModel() {
    this.updateParentCallback(this.question);
  }

  onTitleChanged(e: Event) {
    this.question.title = (e.currentTarget as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }

  onIdentifierChanged(e: Event, question: Question) {
    question.id = (e.currentTarget as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }

  onQuestionChanged(e: Event) {
    this.question.questionText = (e.currentTarget as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }

  onGuidanceChanged(e: Event, question: Question) {
    question.questionGuidance = (e.currentTarget as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }

  onHintChanged(e: Event, question: Question) {
    question.questionHint = (e.currentTarget as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }

  onDisplayCommentsBox(e: Event, question: Question) {
    const checkbox = (e.target as HTMLInputElement);
    question.displayComments = checkbox.checked;
    this.updatePropertyModel();
  }

  render() {
    return (
      <div>
        <div>
          <input type="text" id={this.question.id + "_title"} name="title" value={this.question.title} onChange={e => this.onTitleChanged(e)}
               class="disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300"
               disabled={false}/>
          {/*<input type="text" value={this.question.title} onChange={e => this.onTitleChanged(e)}*/}
          {/*  class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />*/}
          <br/>
          <input type="text" value={this.question.questionText} onChange={e => this.onQuestionChanged(e)}
            class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
          <br />
          <input type="text" value={this.question.questionHint} onChange={e => this.onHintChanged(e, this.question)}
            class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
          <br />
          <textarea value={this.question.questionGuidance} onChange={e => this.onGuidanceChanged(e, this.question)}
            class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
          <br />
          <input id={"displayCommentsCheckbox_" + this.question.id} name={"displayCommentsCheckbox_" + this.question.id} type="checkbox" checked={this.question.displayComments} value={'true'}
            onChange={e => this.onDisplayCommentsBox(e, this.question)}
            class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
        </div>
      </div>
    );
  }
}
