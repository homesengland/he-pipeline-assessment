 //import { x } from '@elsa-workflows/elsa-workflows-studio'; //Not In Use.  Using as a placeholder to prompt developers to draw, and copy any interfaces they need from Elsa directly.


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




