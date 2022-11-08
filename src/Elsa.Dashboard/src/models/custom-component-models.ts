 export interface MultiChoiceRecord {
    answer: string;
    isSingle: boolean;
}

export interface SingleChoiceRecord {
  answer: string;
}

export interface QuestionComponent {
  id: string;
  title: string;
  questionGuidance: string;
  questionText: string;
  displayComments: boolean;
  questionHint: string;
  questionType: string;
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






