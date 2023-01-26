import { Map } from './utils'

///Question Options

export interface IQuestionOption {
  identifier: string
  answer: string
}

export interface CheckboxOption extends IQuestionOption {
  isSingle: boolean;
}

export interface RadioOption extends IQuestionOption {
}

export class QuestionOptions<T extends IQuestionOption>{
  choices: Array<T> = [];
}


///Questions

export interface ISampleQuestionProperty {
  name: string;
  syntax?: string;
  expressions: Map<string>;
  value?: any;
}

export interface ISampleQuestion {
  properties: Array<ISampleQuestionProperty>;
}

export interface IQuestionComponent {
  id: string;
  title: string;
  questionGuidance: string;
  questionText: string;
  displayComments: boolean;
  questionHint: string;
  questionType: string;
  questionTypeName: string;
}

export class Question implements IQuestionComponent {
    id: string;
    title: string;
    questionGuidance: string;
    questionText: string;
    displayComments: boolean;
    questionHint: string;
    questionType: string;
    questionTypeName: string;
}

export abstract class MultipleChoiceQuestion<T extends IQuestionOption> extends Question {
  options: QuestionOptions<T> = new QuestionOptions<T>()
}

export class CheckboxQuestion extends MultipleChoiceQuestion<CheckboxOption> {
  checkbox: QuestionOptions<CheckboxOption> = this.options;
}

export class RadioQuestion extends MultipleChoiceQuestion<RadioOption> {
  radio: QuestionOptions<RadioOption> = this.options;
}


//Activity Screens

export class MultiChoiceActivity {
  choices: Array<CheckboxOption> = [];
}

export class SingleChoiceActivity {
  choices: Array<RadioOption> = [];
}

export class QuestionActivity {
  questions: Array<IQuestionComponent> = [];
}

//Outcome Screens

export interface IAltConditionalText {
  name: string;
  expressions?: Map<string>;
  syntax?: string;
}

export interface ITextProperty {
  expressions?: Map<string>;
  syntax?: string;
}

export interface IOutcomeProperty {
  text: ITextProperty;
  condition: ITextProperty;
}
