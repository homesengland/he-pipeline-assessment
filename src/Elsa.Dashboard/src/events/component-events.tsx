import { EventEmitter } from '@stencil/core';


import {
  MultiChoiceRecord,
  SingleChoiceRecord,
  Question,
  MultipleChoiceQuestion,
  QuestionComponent,
  RadioQuestion,
  CheckboxQuestion,
  QuestionOptions,
  OptionsRecord,
} from '../models/custom-component-models';



abstract class BaseQuestionEventHandler {

  constructor(q: QuestionComponent, e: EventEmitter) {
    this.question = q;
    this.emitter = e;
  }

  question: Question;
  emitter: EventEmitter;

  onTitleChanged = (e: Event) => {
    let updatedQuestion = this.question;
    updatedQuestion.title = (e.currentTarget as HTMLInputElement).value.trim();
    console.log('emitting title', this.emitter);
    this.emitter.emit(updatedQuestion);
  };

  onIdentifierChanged = (e: Event) => {
    let updatedQuestion = this.question;
    updatedQuestion.id = (e.currentTarget as HTMLInputElement).value.trim();
    this.emitter.emit(updatedQuestion);
  };

  onQuestionChanged = (e: Event) => {
    let updatedQuestion = this.question;
    updatedQuestion.questionText = (e.currentTarget as HTMLInputElement).value.trim();
    this.emitter.emit(updatedQuestion);
  };

  onGuidanceChanged = (e: Event) => {
    let updatedQuestion = this.question;
    updatedQuestion.questionGuidance = (e.currentTarget as HTMLInputElement).value.trim();
    this.emitter.emit(updatedQuestion);
  };

  onHintChanged = (e: Event) => {
    let updatedQuestion = this.question;
    updatedQuestion.questionHint = (e.currentTarget as HTMLInputElement).value.trim();
    console.log(this.emitter);
    this.emitter.emit(updatedQuestion);
  };

  onDisplayCommentsBox= (e: Event) => {
    let updatedQuestion = this.question;
    const checkbox = (e.target as HTMLInputElement);
    updatedQuestion.displayComments = checkbox.checked;
    this.emitter.emit(updatedQuestion);
  };
}

abstract class ChoiceQuestionEventHandler<T> extends BaseQuestionEventHandler {

  constructor(q: MultipleChoiceQuestion<T>, e: EventEmitter) {
    super(q, e);
    if (!this.question.options) {
      this.question.options = new QuestionOptions<T>();
    }
  }

  abstract getChoice(name: string): OptionsRecord;

  question: MultipleChoiceQuestion<T>;
  emitter: EventEmitter;

  onAddChoiceClick = () => {
    const choiceName = `Choice ${this.question.options.choices.length + 1}`;
    const newChoice = this.getChoice(choiceName);
    let updatedChoices = { ... this.question.options, choices: [... this.question.options.choices, newChoice] } as QuestionOptions<T>;
    this.question = { ... this.question, options: updatedChoices };
    this.emitter.emit(this.question);
  };

//  onAddChoiceClick = () => {
//    let q = this.question as MultipleChoiceQuestion;
//    console.log('logging question type', q)
//    const choiceName = `Choice ${q.getChoices.choices.length + 1}`;
//    const newChoice = { answer: choiceName, isSingle: false };
//    let newCheckboxObj = { ...q.getChoices, choices: [...q.getChoices.choices, newChoice] };
//    q = q.setChoices(newCheckboxObj.choices);
///*    q = { ... q, q.getChoices().choices : newCheckboxObj };*/
//    this.emitter.emit(q);
//  };

  //onChoiceNameChanged = (e: Event, record: MultiChoiceRecord) => {
  //  record.answer = (e.currentTarget as HTMLInputElement).value.trim();
  //  this.emitter.emit(this.question);
  //};

  //onCheckChanged = (e: Event, record: MultiChoiceRecord) => {
  //  const checkbox = (e.target as HTMLInputElement);
  //  record.isSingle = checkbox.checked;
  //  this.emitter.emit(this.question);
  //};

  //onDeleteChoiceClick = (record: MultiChoiceRecord) => {
  //  let newCheckboxObj = { ... this.question.checkbox, choices: this.question.checkbox.choices.filter(x => x != record) };
  //  this.question = { ...this.question, checkbox: newCheckboxObj }
  //  this.emitter.emit(this.question);
  //};
}

export class QuestionEventHandler extends BaseQuestionEventHandler {
  constructor(q: Question, e: EventEmitter) {
    super(q, e);
  }

  question: Question;
  emitter: EventEmitter<Question>;
}

export class CheckboxEventHandler extends ChoiceQuestionEventHandler<MultiChoiceRecord> {


  constructor(q: CheckboxQuestion, e: EventEmitter) {
    super(q, e);
  }

  question: CheckboxQuestion;
  emitter: EventEmitter;

  getChoice(name: string): MultiChoiceRecord {
    return { answer: name, isSingle: false };
  }

  onAddChoiceClick = () => {
    const choiceName = `Choice ${this.question.checkbox.choices.length + 1}`;
    const newChoice = { answer: choiceName, isSingle: false };
    let newCheckboxObj = { ... this.question.checkbox, choices: [... this.question.checkbox.choices, newChoice] };
    this.question = { ... this.question, checkbox: newCheckboxObj };
    this.emitter.emit(this.question);
  };

  onChoiceNameChanged = (e: Event, record: MultiChoiceRecord) => {
    record.answer = (e.currentTarget as HTMLInputElement).value.trim();
    this.emitter.emit(this.question);
  };

  onCheckChanged = (e: Event, record: MultiChoiceRecord) => {
    const checkbox = (e.target as HTMLInputElement);
    record.isSingle = checkbox.checked;
    this.emitter.emit(this.question);
  };

  onDeleteChoiceClick = (record: MultiChoiceRecord) => {
    record = record;
    let newCheckboxObj = { ... this.question.checkbox, choices: this.question.checkbox.choices.filter(x => x != record) };
    this.question = { ...this.question, checkbox: newCheckboxObj }
    this.emitter.emit(this.question);
  };

}

export class RadioEventHandler extends ChoiceQuestionEventHandler<SingleChoiceRecord> {


  constructor(q: RadioQuestion, e: EventEmitter) {
    super(q, e);
  };

  question: RadioQuestion;
  emitter: EventEmitter;

  getChoice(name: string): SingleChoiceRecord {
    return { answer: name };
  }

  onAddChoiceClick = () => {
    const choiceName = `Choice ${this.question.radio.choices.length + 1}`;
    const newChoice = { answer: choiceName, isSingle: false };
    let newRadioObj = { ... this.question.radio, choices: [... this.question.radio.choices, newChoice] };
    this.question = { ... this.question, radio: newRadioObj };
    this.emitter.emit(this.question);
  };

  onChoiceNameChanged = (e: Event, record: SingleChoiceRecord) => {
    record.answer = (e.currentTarget as HTMLInputElement).value.trim();
    this.emitter.emit(this.question);
  };

  onDeleteChoiceClick = (record: SingleChoiceRecord) => {
    record = record;
    let newRadioObj = { ... this.question.radio, choices: this.question.radio.choices.filter(x => x != record) };
    this.question = { ...this.question, radio: newRadioObj }
    this.emitter.emit(this.question);
  };

}
