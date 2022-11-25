import { RadioQuestion, RadioOption, QuestionOptions } from '../models/custom-component-models';
import { RadioEventHandler } from './component-events'

  it('onTitleChanged should emit an event with updated title on radio', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new RadioQuestion();
    question.title = "MyTestTitle";
    let handler = new RadioEventHandler(question, emitter);

    let input = document.createElement('input');
    input.value = "NewTitle";
    let event = new Event("");
    jest.spyOn(event, 'currentTarget', 'get').mockReturnValue(input)
    handler.onTitleChanged(event)

    expect(question.title).toBe("NewTitle");
    expect(emitter.emit).lastCalledWith(question);
  });

  it('onIdentifierChanged should emit an event with updated id on radio', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new RadioQuestion();
    question.id = "MyId";
    let handler = new RadioEventHandler(question, emitter);

    let input = document.createElement('input');
    input.value = "NewId";
    let event = new Event("");
    jest.spyOn(event, 'currentTarget', 'get').mockReturnValue(input)
    handler.onIdentifierChanged(event)

    expect(question.id).toBe("NewId");
    expect(emitter.emit).lastCalledWith(question);
  });

  it('onQuestionChanged should emit an event with updated question text on radio', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new RadioQuestion();
    question.questionText = "MyQuestionText";
    let handler = new RadioEventHandler(question, emitter);

    let input = document.createElement('input');
    input.value = "NewQuestionText";
    let event = new Event("");
    jest.spyOn(event, 'currentTarget', 'get').mockReturnValue(input)
    handler.onQuestionChanged(event)

    expect(question.questionText).toBe("NewQuestionText");
    expect(emitter.emit).lastCalledWith(question);
  });

  it('onGuidanceChanged should emit an event with updated guidance on radio', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new RadioQuestion();
    question.questionGuidance = "MyGuidance";
    let handler = new RadioEventHandler(question, emitter);

    let input = document.createElement('input');
    input.value = "NewGuidance";
    let event = new Event("");
    jest.spyOn(event, 'currentTarget', 'get').mockReturnValue(input)
    handler.onGuidanceChanged(event)

    expect(question.questionGuidance).toBe("NewGuidance");
    expect(emitter.emit).lastCalledWith(question);
  });

  it('onHintChanged should emit an event with updated hint on radio', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new RadioQuestion();
    question.questionHint = "MyHint";
    let handler = new RadioEventHandler(question, emitter);

    let input = document.createElement('input');
    input.value = "NewHint";
    let event = new Event("");
    jest.spyOn(event, 'currentTarget', 'get').mockReturnValue(input)
    handler.onHintChanged(event)

    expect(question.questionHint).toBe("NewHint");
    expect(emitter.emit).lastCalledWith(question);
  });

  it('onDisplayCommentsBox should emit an event with updated displayCommentsBox on radio', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new RadioQuestion();
    question.displayComments = false;
    let handler = new RadioEventHandler(question, emitter);

    let input = document.createElement('input');
    input.checked = true;
    let event = new Event("");
    jest.spyOn(event, 'currentTarget', 'get').mockReturnValue(input)
    handler.onDisplayCommentsBox(event)

    expect(question.displayComments).toBe(true);
    expect(emitter.emit).lastCalledWith(question);
  });

  it('should construct radio event handler', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let handler = new RadioEventHandler(new RadioQuestion(), emitter);
    expect(handler).toBeTruthy();
  });

  it('onAddChoiceClick should emit an event with new choice on radio', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new RadioQuestion();
    var oldOption: RadioOption = {answer: 'second'};
    question.options.choices = [{answer: 'first'}, oldOption];
    question.radio.choices = [{answer: 'first'}, oldOption];
    let handler = new RadioEventHandler(question, emitter);

    handler.onAddChoiceClick();
    handler.onAddChoiceClick();

    expect(handler.question.radio.choices.length).toBe(4);
    expect(handler.question.options.choices.length).toBe(4);

    let lastChoice = handler.question.radio.choices[3];
    expect(lastChoice.answer).toBe("Choice 4");
    expect(emitter.emit).lastCalledWith(handler.question);
  });

  it('onDeleteChoiceClick should emit an event without deleted choice on radio', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new RadioQuestion();
    var oldOption: RadioOption = {answer: 'second'};
    question.options.choices = [{answer: 'first'}, oldOption];
    question.radio.choices = [{answer: 'first'}, oldOption];
    let handler = new RadioEventHandler(question, emitter);

    handler.onDeleteChoiceClick(oldOption);

    expect(handler.question.radio.choices.length).toBe(1);
    expect(handler.question.options.choices.length).toBe(1);

    let onlyChoice = handler.question.radio.choices[0];
    expect(onlyChoice.answer).toBe("first");
    expect(emitter.emit).lastCalledWith(handler.question);
  });

  it('onChoiceNameChanged should emit an event with trimmed updated choice on radio', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new RadioQuestion();
    var oldOption: RadioOption = {answer: 'second'};
    question.options.choices = [{answer: 'first'}, oldOption];
    question.radio.choices = [{answer: 'first'}, oldOption];
    let handler = new RadioEventHandler(question, emitter);

    let input = document.createElement('input');
    input.value = "NewValue";
    let event = new Event("");
    jest.spyOn(event, 'currentTarget', 'get').mockReturnValue(input)
    handler.onChoiceNameChanged(event, oldOption);

    expect(question.radio.choices.length).toBe(2);
    expect(question.options.choices.length).toBe(2);

    let onlyChoice = question.radio.choices[1];
    expect(onlyChoice.answer).toBe("NewValue");
    expect(emitter.emit).lastCalledWith(question);
  });

  it('getChoice should return a choice object with supplied name on radio', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    let question = new RadioQuestion();
    let handler = new RadioEventHandler(question, emitter);

    let choice = handler.getChoice("SomeName")

    expect(choice.answer).toBe("SomeName");
  });

  it('assignOptions should set radio value to supplied object', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    let question = new RadioQuestion();
    let handler = new RadioEventHandler(question, emitter);
    let questionOptions: QuestionOptions<RadioOption> = { choices: [{answer: 'Option 1'},{answer: 'Option 2'}]}
    
    handler.assignOptions(questionOptions)

    expect(question.radio).toBe(questionOptions);
  });