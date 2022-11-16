///Question Options

export interface IQuestionOption {
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

  setOptions(val: QuestionOptions<RadioOption>) {
    this.options = val;
    this.radio = val;
  }
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






