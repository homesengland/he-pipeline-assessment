///Question Options
export class QuestionOptions {
    constructor() {
        this.choices = [];
    }
}
export class Question {
}
export class MultipleChoiceQuestion extends Question {
    constructor() {
        super(...arguments);
        this.options = new QuestionOptions();
    }
}
export class CheckboxQuestion extends MultipleChoiceQuestion {
    constructor() {
        super(...arguments);
        this.checkbox = this.options;
    }
}
export class RadioQuestion extends MultipleChoiceQuestion {
    constructor() {
        super(...arguments);
        this.radio = this.options;
    }
}
//Activity Screens
export class MultiChoiceActivity {
    constructor() {
        this.choices = [];
    }
}
export class SingleChoiceActivity {
    constructor() {
        this.choices = [];
    }
}
export class QuestionActivity {
    constructor() {
        this.questions = [];
    }
}
