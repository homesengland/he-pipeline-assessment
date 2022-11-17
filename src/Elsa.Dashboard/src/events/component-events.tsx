import { EventEmitter } from '@stencil/core';

import {
  CheckboxOption,
  RadioOption,
  IQuestionOption,
  IQuestionComponent,
  Question,
  MultipleChoiceQuestion,
  RadioQuestion,
  CheckboxQuestion,
  QuestionOptions,
} from '../models/custom-component-models';



abstract class BaseQuestionEventHandler {

  constructor(q: IQuestionComponent, e: EventEmitter) {
    this.question = q;
    this.emitter = e;
  }

  question: Question;
  emitter: EventEmitter;

  onTitleChanged = (e: Event) => {
    let updatedQuestion = this.question;
    updatedQuestion.title = (e.currentTarget as HTMLInputElement).value.trim();
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
    this.emitter.emit(updatedQuestion);
  };

  onDisplayCommentsBox= (e: Event) => {
    let updatedQuestion = this.question;
    const checkbox = (e.target as HTMLInputElement);
    updatedQuestion.displayComments = checkbox.checked;
    this.emitter.emit(updatedQuestion);
  };
}

abstract class ChoiceQuestionEventHandler<T extends IQuestionOption> extends BaseQuestionEventHandler {

  constructor(q: MultipleChoiceQuestion<T>, e: EventEmitter) {
    super(q, e);
    if (!this.question.options) {
      this.question.options = new QuestionOptions<T>();
    }
  }

  abstract getChoice(name: string): IQuestionOption;

  abstract assignOptions(val: QuestionOptions<T>)

  question: MultipleChoiceQuestion<T>;
  emitter: EventEmitter;

  onAddChoiceClick = () => {
    const choiceName = `Choice ${this.question.options.choices.length + 1}`;
    const newChoice = this.getChoice(choiceName);
    let updatedChoices = { ... this.question.options, choices: [... this.question.options.choices, newChoice] } as QuestionOptions<T>;
    this.question = { ...this.question, options: updatedChoices }
    this.assignOptions(updatedChoices);
    this.emitter.emit(this.question);
  };

  onChoiceNameChanged = (e: Event, record: IQuestionOption) => {
    record.answer = (e.currentTarget as HTMLInputElement).value.trim();
    this.emitter.emit(this.question);
  };

  onDeleteChoiceClick = (record: T) => {
    //The below filter method has to convert and compare a stringified version, as the options property is mapped from the actual implmeneted property
    //i.e. Radio or Checkbox.  Because of this the objects are not equal in reference, and the filter would fail.
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

export class CheckboxEventHandler extends ChoiceQuestionEventHandler<CheckboxOption> {

  constructor(q: CheckboxQuestion, e: EventEmitter) {
    super(q, e);
  }

  question: CheckboxQuestion;
  emitter: EventEmitter;

  getChoice(name: string): CheckboxOption {
    return { answer: name, isSingle: false };
  }

  assignOptions(val: QuestionOptions<CheckboxOption>) {
    this.question.checkbox = val;
  }

  onCheckChanged = (e: Event, record: CheckboxOption) => {
    const checkbox = (e.target as HTMLInputElement);
    record.isSingle = checkbox.checked;
    this.emitter.emit(this.question);
  };

}

export class RadioEventHandler extends ChoiceQuestionEventHandler<RadioOption> {


  constructor(q: RadioQuestion, e: EventEmitter) {
    super(q, e);
  };

  question: RadioQuestion;
  emitter: EventEmitter;

  getChoice(name: string): RadioOption {
    return { answer: name };
  }

  assignOptions = (val: QuestionOptions<RadioOption>) => {
    this.question.radio = val;
  }
}
