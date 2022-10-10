 export interface MultiChoiceRecord {
    answer: string;
    isSingle: boolean;
}

export interface SingleChoiceRecord {
  answer: string;
}

export class MultiChoiceActivity {
  choices: Array<MultiChoiceRecord> = [];
}

export class SingleChoiceActivity {
  choices: Array<SingleChoiceRecord> = [];
}




