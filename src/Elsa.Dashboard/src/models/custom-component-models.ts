export interface OptionsRecord {
  answer: string
}

export interface MultiChoiceRecord extends OptionsRecord {
    isSingle: boolean;
}

export interface SingleChoiceRecord extends OptionsRecord {
}

export class QuestionOptions<T>{
  choices: Array<T> = [];
}

//export class CheckboxChoices extends  {
//  choices: Array<MultiChoiceRecord> = []
//}

//export class RadioChoices {
//  choices: Array<SingleChoiceRecord> = []
//}



export interface QuestionComponent {
  id: string;
  title: string;
  questionGuidance: string;
  questionText: string;
  displayComments: boolean;
  questionHint: string;
  questionType: string;
}

export class Question implements QuestionComponent {
    id: string;
    title: string;
    questionGuidance: string;
    questionText: string;
    displayComments: boolean;
    questionHint: string;
    questionType: string;
}

export abstract class MultipleChoiceQuestion<T> extends Question {

  options: QuestionOptions<T> = new QuestionOptions<T>()
}

export class CheckboxQuestion extends MultipleChoiceQuestion<MultiChoiceRecord> {

  checkbox: QuestionOptions<MultiChoiceRecord> = this.options;

}

export class RadioQuestion extends MultipleChoiceQuestion<SingleChoiceRecord> {

  radio: QuestionOptions<SingleChoiceRecord> = this.options;

}




export class MultiChoiceActivity {
  choices: Array<MultiChoiceRecord> = [];
}

export class SingleChoiceActivity {
  choices: Array<SingleChoiceRecord> = [];
}

export class MultiQuestionActivity {
  questions: Array<QuestionComponent> = [];
}






