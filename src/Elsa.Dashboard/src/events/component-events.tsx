import { EventEmitter } from '@stencil/core';

import {
  MultiChoiceQuestion,
  MultiChoiceRecord,
  SingleChoiceRecord,
  Question,
  QuestionComponent,
  SingleChoiceQuestion
} from '../models/custom-component-models';



abstract class BaseQuestionEventHandler {

  constructor() {
    
  }
  abstract question: QuestionComponent;
  abstract emitter: EventEmitter;

  //@Event({
  //  eventName: 'updateQuestion',
  //  composed: true,
  //  cancelable: true,
  //  bubbles: true,
  //}) updateQuestion: EventEmitter<QuestionComponent>;

  onTitleChanged = (e: Event) => {
    let updatedQuestion = this.question;
    updatedQuestion.title = (e.currentTarget as HTMLInputElement).value.trim();
    this.emitter.emit(updatedQuestion);
  }

  onIdentifierChanged(e: Event) {
    let updatedQuestion = this.question;
    updatedQuestion.id = (e.currentTarget as HTMLInputElement).value.trim();
    this.emitter.emit(updatedQuestion);
  }

  onQuestionChanged(e: Event) {
    let updatedQuestion = this.question;
    updatedQuestion.questionText = (e.currentTarget as HTMLInputElement).value.trim();
    this.emitter.emit(updatedQuestion);
  };

  onGuidanceChanged(e: Event) {
    let updatedQuestion = this.question;
    updatedQuestion.questionGuidance = (e.currentTarget as HTMLInputElement).value.trim();
    this.emitter.emit(updatedQuestion);
  }

  onHintChanged(e: Event) {
    let updatedQuestion = this.question;
    updatedQuestion.questionHint = (e.currentTarget as HTMLInputElement).value.trim();
    console.log(this.emitter);
    this.emitter.emit(updatedQuestion);
  }

  onDisplayCommentsBox(e: Event) {
    let updatedQuestion = this.question;
    const checkbox = (e.target as HTMLInputElement);
    updatedQuestion.displayComments = checkbox.checked;
    this.emitter.emit(updatedQuestion);
  }
}

export class QuestionEventHandler extends BaseQuestionEventHandler {
  constructor(q: Question, e: EventEmitter) {
    super();
    this.question = q;
    this.emitter = e;
  }

  question: Question;
  emitter: EventEmitter<Question>;
}

export class CheckboxEventHandler extends BaseQuestionEventHandler {

  constructor(q: MultiChoiceQuestion, e: EventEmitter) {
    super();
    this.question = q;
    this.emitter = e;
  }

  question: MultiChoiceQuestion;
  emitter: EventEmitter<MultiChoiceQuestion>;

  onAddChoiceClick() {
    const choiceName = `Choice ${this.question.checkbox.choices.length + 1}`;
    const newChoice = { answer: choiceName, isSingle: false };
    let newCheckboxObj = { ... this.question.checkbox, choices: [... this.question.checkbox.choices, newChoice] };
    this.question = { ... this.question, checkbox: newCheckboxObj };
    this.emitter.emit(this.question);
  }

  onChoiceNameChanged(e: Event, record: MultiChoiceRecord) {
    record.answer = (e.currentTarget as HTMLInputElement).value.trim();
    this.emitter.emit(this.question);
  }

  onCheckChanged(e: Event, record: MultiChoiceRecord) {
    const checkbox = (e.target as HTMLInputElement);
    record.isSingle = checkbox.checked;
    this.emitter.emit(this.question);
  }

  onDeleteChoiceClick(record: MultiChoiceRecord) {
    let newCheckboxObj = { ... this.question.checkbox, choices: this.question.checkbox.choices.filter(x => x != record) };
    this.question = { ...this.question, checkbox: newCheckboxObj }
    this.emitter.emit(this.question);
  }

}

export class RadioEventHandler extends BaseQuestionEventHandler {

  constructor(q: SingleChoiceQuestion, e: EventEmitter) {
    super();
    this.question = q;
    this.emitter = e;
  }

  question: SingleChoiceQuestion;
  emitter: EventEmitter<SingleChoiceQuestion>;

  onAddChoiceClick() {
    const choiceName = `Choice ${this.question.radio.choices.length + 1}`;
    const newChoice = { answer: choiceName, isSingle: false };
    let newRadioObj = { ... this.question.radio, choices: [... this.question.radio.choices, newChoice] };
    this.question = { ... this.question, radio: newRadioObj };
    this.emitter.emit(this.question);
  }

  onChoiceNameChanged(e: Event, record: SingleChoiceRecord) {
    record.answer = (e.currentTarget as HTMLInputElement).value.trim();
    this.emitter.emit(this.question);
  }

  onDeleteChoiceClick(record: SingleChoiceRecord) {
    let newRadioObj = { ... this.question.radio, choices: this.question.radio.choices.filter(x => x != record) };
    this.question = { ...this.question, radio: newRadioObj }
    this.emitter.emit(this.question);
  }

}
