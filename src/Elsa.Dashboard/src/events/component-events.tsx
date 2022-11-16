import { EventEmitter } from '@stencil/core';

import {
  MultiChoiceRecord,
  SingleChoiceRecord,
  OptionsRecord,
  QuestionComponent,
  Question,
  MultipleChoiceQuestion,
  RadioQuestion,
  CheckboxQuestion,
  QuestionOptions,
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

  abstract assignOptions(val: QuestionOptions<T>)

  question: MultipleChoiceQuestion<T>;
  emitter: EventEmitter;

  onAddChoiceClick = () => {
    console.log('adding choice');
    const choiceName = `Choice ${this.question.options.choices.length + 1}`;
    console.log('question', this.question);
    const newChoice = this.getChoice(choiceName);
    console.log('new choice', newChoice);
    let updatedChoices = { ... this.question.options, choices: [... this.question.options.choices, newChoice] } as QuestionOptions<T>;
    console.log('updated choices', updatedChoices);
    //this.question = { ... this.question, options: updatedChoices };
    console.log(this.question);
    this.question = { ...this.question, options: updatedChoices }
    this.assignOptions(updatedChoices);
    console.log('updated question', this.question);
    this.emitter.emit(this.question);
    console.log('emitting...');
  };

  onChoiceNameChanged = (e: Event, record: MultiChoiceRecord) => {
    record.answer = (e.currentTarget as HTMLInputElement).value.trim();
    this.emitter.emit(this.question);
  };

  onDeleteChoiceClick = (record: T) => {
    let newChoiceObj = { ... this.question.options, choices: this.question.options.choices.filter(x => JSON.stringify(x) != JSON.stringify(record)) };
    this.question = { ...this.question, options: newChoiceObj }
    this.assignOptions(newChoiceObj);
    this.emitter.emit(this.question);
  };
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

  assignOptions(val: QuestionOptions<MultiChoiceRecord>) {
    this.question.checkbox = val;
  }

  onCheckChanged = (e: Event, record: MultiChoiceRecord) => {
    console.log("Check changed...")
    const checkbox = (e.target as HTMLInputElement);
    record.isSingle = checkbox.checked;
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

  assignOptions = (val: QuestionOptions<SingleChoiceRecord>) => {
    this.question.radio = val;
  }
}
