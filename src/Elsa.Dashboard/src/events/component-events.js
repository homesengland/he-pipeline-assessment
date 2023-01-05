import { ToLetter } from '../models/utils';
import { QuestionOptions, } from '../models/custom-component-models';
class BaseQuestionEventHandler {
    constructor(q, e) {
        this.onTitleChanged = (e) => {
            let updatedQuestion = this.question;
            updatedQuestion.title = e.currentTarget.value.trim();
            this.emitter.emit(updatedQuestion);
        };
        this.onIdentifierChanged = (e) => {
            let updatedQuestion = this.question;
            updatedQuestion.id = e.currentTarget.value.trim();
            this.emitter.emit(updatedQuestion);
        };
        this.onQuestionChanged = (e) => {
            let updatedQuestion = this.question;
            updatedQuestion.questionText = e.currentTarget.value.trim();
            this.emitter.emit(updatedQuestion);
        };
        this.onGuidanceChanged = (e) => {
            let updatedQuestion = this.question;
            updatedQuestion.questionGuidance = e.currentTarget.value.trim();
            this.emitter.emit(updatedQuestion);
        };
        this.onHintChanged = (e) => {
            let updatedQuestion = this.question;
            updatedQuestion.questionHint = e.currentTarget.value.trim();
            this.emitter.emit(updatedQuestion);
        };
        this.onDisplayCommentsBox = (e) => {
            let updatedQuestion = this.question;
            const checkbox = e.currentTarget;
            updatedQuestion.displayComments = checkbox.checked;
            this.emitter.emit(updatedQuestion);
        };
        this.question = q;
        this.emitter = e;
    }
}
class ChoiceQuestionEventHandler extends BaseQuestionEventHandler {
    constructor(q, e) {
        super(q, e);
        this.onAddChoiceClick = () => {
            const choiceName = `Choice ${this.question.options.choices.length + 1}`;
            const identifier = ToLetter(1 + this.question.options.choices.length);
            console.log(identifier);
            const newChoice = this.getChoice(choiceName, identifier);
            let updatedChoices = Object.assign(Object.assign({}, this.question.options), { choices: [...this.question.options.choices, newChoice] });
            this.question = Object.assign(Object.assign({}, this.question), { options: updatedChoices });
            this.enforceUniqueIdentifier();
            this.assignOptions(updatedChoices);
            this.emitter.emit(this.question);
        };
        this.onChoiceNameChanged = (e, record) => {
            record.answer = e.currentTarget.value.trim();
            this.emitter.emit(this.question);
        };
        this.onChoiceIdentifierChanged = (e, record) => {
            record.identifier = e.currentTarget.value.trim();
            this.emitter.emit(this.question);
        };
        this.onDeleteChoiceClick = (record) => {
            //The below filter method has to convert and compare a stringified version, as the options property is mapped from the actual implmeneted property
            //i.e. Radio or Checkbox.  Because of this the objects are not equal in reference, and the filter would fail.
            let newChoiceObj = Object.assign(Object.assign({}, this.question.options), { choices: this.question.options.choices.filter(x => JSON.stringify(x) != JSON.stringify(record)) });
            this.question = Object.assign(Object.assign({}, this.question), { options: newChoiceObj });
            this.enforceUniqueIdentifier();
            this.assignOptions(newChoiceObj);
            this.emitter.emit(this.question);
        };
        if (!this.question.options) {
            this.question.options = new QuestionOptions();
        }
    }
    enforceUniqueIdentifier() {
        for (let i = 0; i < this.question.options.choices.length; i++) {
            this.question.options.choices[i].identifier = ToLetter(i + 1);
        }
    }
}
export class QuestionEventHandler extends BaseQuestionEventHandler {
    constructor(q, e) {
        super(q, e);
    }
}
export class CheckboxEventHandler extends ChoiceQuestionEventHandler {
    constructor(q, e) {
        super(q, e);
        this.onCheckChanged = (e, record) => {
            const checkbox = e.currentTarget;
            record.isSingle = checkbox.checked;
            this.emitter.emit(this.question);
        };
    }
    getChoice(name, id) {
        return { identifier: id, answer: name, isSingle: false };
    }
    assignOptions(val) {
        this.question.checkbox = val;
    }
}
export class RadioEventHandler extends ChoiceQuestionEventHandler {
    constructor(q, e) {
        super(q, e);
        this.assignOptions = (val) => {
            this.question.radio = val;
        };
    }
    ;
    getChoice(name, id) {
        return { identifier: id, answer: name };
    }
}
