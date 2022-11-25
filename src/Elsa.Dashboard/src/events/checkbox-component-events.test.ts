import { CheckboxQuestion, CheckboxOption, QuestionOptions } from '../models/custom-component-models';
import { CheckboxEventHandler } from './component-events'

  it('should construct checkbox event handler', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let handler = new CheckboxEventHandler(new CheckboxQuestion(), emitter);
    expect(handler).toBeTruthy();
  });

  it('onTitleChanged should emit an event with updated title on checkbox', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new CheckboxQuestion();
    question.title = "MyTestTitle";
    let handler = new CheckboxEventHandler(question, emitter);

    let input = document.createElement('input');
    input.value = "NewTitle";
    let event = new Event("");
    jest.spyOn(event, 'currentTarget', 'get').mockReturnValue(input)
    handler.onTitleChanged(event)

    expect(question.title).toBe("NewTitle");
    expect(emitter.emit).lastCalledWith(question);
  });

  it('onIdentifierChanged should emit an event with updated id on checkbox', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new CheckboxQuestion();
    question.id = "MyId";
    let handler = new CheckboxEventHandler(question, emitter);

    let input = document.createElement('input');
    input.value = "NewId";
    let event = new Event("");
    jest.spyOn(event, 'currentTarget', 'get').mockReturnValue(input)
    handler.onIdentifierChanged(event)

    expect(question.id).toBe("NewId");
    expect(emitter.emit).lastCalledWith(question);
  });

  it('onQuestionChanged should emit an event with updated question text on checkbox', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new CheckboxQuestion();
    question.questionText = "MyQuestionText";
    let handler = new CheckboxEventHandler(question, emitter);

    let input = document.createElement('input');
    input.value = "NewQuestionText";
    let event = new Event("");
    jest.spyOn(event, 'currentTarget', 'get').mockReturnValue(input)
    handler.onQuestionChanged(event)

    expect(question.questionText).toBe("NewQuestionText");
    expect(emitter.emit).lastCalledWith(question);
  });

  it('onGuidanceChanged should emit an event with updated guidance on checkbox', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new CheckboxQuestion();
    question.questionGuidance = "MyGuidance";
    let handler = new CheckboxEventHandler(question, emitter);

    let input = document.createElement('input');
    input.value = "NewGuidance";
    let event = new Event("");
    jest.spyOn(event, 'currentTarget', 'get').mockReturnValue(input)
    handler.onGuidanceChanged(event)

    expect(question.questionGuidance).toBe("NewGuidance");
    expect(emitter.emit).lastCalledWith(question);
  });

  it('onHintChanged should emit an event with updated hint on checkbox', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new CheckboxQuestion();
    question.questionHint = "MyHint";
    let handler = new CheckboxEventHandler(question, emitter);

    let input = document.createElement('input');
    input.value = "NewHint";
    let event = new Event("");
    jest.spyOn(event, 'currentTarget', 'get').mockReturnValue(input)
    handler.onHintChanged(event)

    expect(question.questionHint).toBe("NewHint");
    expect(emitter.emit).lastCalledWith(question);
  });

  it('onDisplayCommentsBox should emit an event with updated displayCommentsBox on checkbox', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new CheckboxQuestion();
    question.displayComments = false;
    let handler = new CheckboxEventHandler(question, emitter);

    let input = document.createElement('input');
    input.checked = true;
    let event = new Event("");
    jest.spyOn(event, 'currentTarget', 'get').mockReturnValue(input)
    handler.onDisplayCommentsBox(event)

    expect(question.displayComments).toBe(true);
    expect(emitter.emit).lastCalledWith(question);
  });

  it('onAddChoiceClick should emit an event with new choice on checkbox', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new CheckboxQuestion();
    var oldOption: CheckboxOption = {answer: 'second', isSingle: true, identifier: 'testId'};
    question.options.choices = [{answer: 'first', isSingle: true, identifier: 'testId'}, oldOption];
    question.checkbox.choices = [{answer: 'first', isSingle: true, identifier: 'testId'}, oldOption];
    let handler = new CheckboxEventHandler(question, emitter);

    jest.spyOn(handler, 'enforceUniqueIdentifier');

    handler.onAddChoiceClick();
    handler.onAddChoiceClick();
    handler.onAddChoiceClick();

    expect(handler.question.checkbox.choices.length).toBe(5);
    expect(handler.question.options.choices.length).toBe(5);

    let lastChoice = handler.question.checkbox.choices[4];
    expect(lastChoice.isSingle).toBe(false);
    expect(lastChoice.answer).toBe("Choice 5");
    expect(lastChoice.identifier).toBe("E");
    expect(emitter.emit).lastCalledWith(handler.question);
    expect(handler.enforceUniqueIdentifier).toBeCalledTimes(3);
  });

  it('onDeleteChoiceClick should emit an event without deleted choice on checkbox', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new CheckboxQuestion();
    var oldOption: CheckboxOption = {answer: 'second', isSingle: true, identifier: 'testId'};
    question.options.choices = [{answer: 'first', isSingle: true, identifier: 'testId'}, oldOption];
    question.checkbox.choices = [{answer: 'first', isSingle: true, identifier: 'testId'}, oldOption];
    let handler = new CheckboxEventHandler(question, emitter);

    jest.spyOn(handler, 'enforceUniqueIdentifier');

    handler.onDeleteChoiceClick(oldOption);

    expect(handler.question.checkbox.choices.length).toBe(1);
    expect(handler.question.options.choices.length).toBe(1);

    let onlyChoice = handler.question.checkbox.choices[0];
    expect(onlyChoice.isSingle).toBe(true);
    expect(onlyChoice.answer).toBe("first");
    expect(onlyChoice.identifier).toBe("A");
    expect(emitter.emit).lastCalledWith(handler.question);
    expect(handler.enforceUniqueIdentifier).toBeCalledTimes(1);
  });

  it('onChoiceNameChanged should emit an event with trimmed updated choice on checkbox', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new CheckboxQuestion();
    var oldOption: CheckboxOption = {answer: 'second', isSingle: true, identifier: 'testId'};
    question.options.choices = [{answer: 'first', isSingle: true, identifier: 'testId'}, oldOption];
    question.checkbox.choices = [{answer: 'first', isSingle: true, identifier: 'testId'}, oldOption];
    let handler = new CheckboxEventHandler(question, emitter);

    let input = document.createElement('input');
    input.value = "NewValue";
    let event = new Event("");
    jest.spyOn(event, 'currentTarget', 'get').mockReturnValue(input)
    handler.onChoiceNameChanged(event, oldOption);

    expect(question.checkbox.choices.length).toBe(2);
    expect(question.options.choices.length).toBe(2);

    let onlyChoice = question.checkbox.choices[1];
    expect(onlyChoice.isSingle).toBe(true);
    expect(onlyChoice.answer).toBe("NewValue");
    expect(emitter.emit).lastCalledWith(question);
  });

  it('getChoice should return a choice object with default isSingle and supplied name and id on checkbox', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    let question = new CheckboxQuestion();
    let handler = new CheckboxEventHandler(question, emitter);

    let choice = handler.getChoice("SomeName", "SomeId")

    expect(choice.answer).toBe("SomeName");
    expect(choice.identifier).toBe("SomeId");
    expect(choice.isSingle).toBe(false);
  });

  it('assignOptions should set checkbox value to supplied object', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    let question = new CheckboxQuestion();
    let handler = new CheckboxEventHandler(question, emitter);
    let questionOptions: QuestionOptions<CheckboxOption> = { choices: [{answer: 'Option 1', isSingle: true, identifier: 'testId'},{answer: 'Option 2', isSingle: true, identifier: 'testId'}]}
    
    handler.assignOptions(questionOptions)

    expect(question.checkbox).toBe(questionOptions);
  });

  it('onCheckChanged should emit an event with updated isSingle value on checkbox', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new CheckboxQuestion();
    var oldOption: CheckboxOption = {answer: 'second', isSingle: true, identifier: 'testId'};
    question.options.choices = [{answer: 'first', isSingle: true, identifier: 'testId'}, oldOption];
    question.checkbox.choices = [{answer: 'first', isSingle: true, identifier: 'testId'}, oldOption];
    let handler = new CheckboxEventHandler(question, emitter);

    let input = document.createElement('input');
    input.checked = false;
    let event = new Event("");
    jest.spyOn(event, 'currentTarget', 'get').mockReturnValue(input)
    handler.onCheckChanged(event, oldOption);

    expect(question.checkbox.choices[1].isSingle).toBe(false);
    expect(question.options.choices[1].isSingle).toBe(false);
    expect(oldOption.isSingle).toBe(false);
    expect(emitter.emit).lastCalledWith(question);
  });

  it('enforceUniqueIdentifier should create unique identifiers for checkbox options', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    let question = new CheckboxQuestion();
    let handler = new CheckboxEventHandler(question, emitter);
    let questionOptions: QuestionOptions<CheckboxOption> = { choices: [{answer: 'Option 1', identifier: 'testId', isSingle: true},{answer: 'Option 2', identifier: 'testId',  isSingle: true}]}
    question.options = questionOptions;

    handler.enforceUniqueIdentifier();

    expect(question.options.choices[0].identifier).toBe('A');
    expect(question.options.choices[1].identifier).toBe('B');
  });
