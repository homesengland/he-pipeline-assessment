import { Question } from '../models/custom-component-models';
import { QuestionEventHandler } from './component-events'

it('should construct', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let handler = new QuestionEventHandler(new Question(), emitter);
    expect(handler).toBeTruthy();
});

it('onTitleChanged should emit an event with updated title', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new Question();
    question.title = "MyTestTitle";
    let handler = new QuestionEventHandler(question, emitter);

    let input = document.createElement('input');
    input.value = "NewTitle";
    let event = new Event("");
    jest.spyOn(event, 'currentTarget', 'get').mockReturnValue(input)
    handler.onTitleChanged(event)

    expect(question.title).toBe("NewTitle");
    expect(emitter.emit).lastCalledWith(question);
  });

  it('onIdentifierChanged should emit an event with updated id', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new Question();
    question.id = "MyId";
    let handler = new QuestionEventHandler(question, emitter);

    let input = document.createElement('input');
    input.value = "NewId";
    let event = new Event("");
    jest.spyOn(event, 'currentTarget', 'get').mockReturnValue(input)
    handler.onIdentifierChanged(event)

    expect(question.id).toBe("NewId");
    expect(emitter.emit).lastCalledWith(question);
  });

  it('onQuestionChanged should emit an event with updated question text', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new Question();
    question.questionText = "MyQuestionText";
    let handler = new QuestionEventHandler(question, emitter);

    let input = document.createElement('input');
    input.value = "NewQuestionText";
    let event = new Event("");
    jest.spyOn(event, 'currentTarget', 'get').mockReturnValue(input)
    handler.onQuestionChanged(event)

    expect(question.questionText).toBe("NewQuestionText");
    expect(emitter.emit).lastCalledWith(question);
  });

  it('onGuidanceChanged should emit an event with updated guidance', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new Question();
    question.questionGuidance = "MyGuidance";
    let handler = new QuestionEventHandler(question, emitter);

    let input = document.createElement('input');
    input.value = "NewGuidance";
    let event = new Event("");
    jest.spyOn(event, 'currentTarget', 'get').mockReturnValue(input)
    handler.onGuidanceChanged(event)

    expect(question.questionGuidance).toBe("NewGuidance");
    expect(emitter.emit).lastCalledWith(question);
  });

  it('onHintChanged should emit an event with updated hint', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new Question();
    question.questionHint = "MyHint";
    let handler = new QuestionEventHandler(question, emitter);

    let input = document.createElement('input');
    input.value = "NewHint";
    let event = new Event("");
    jest.spyOn(event, 'currentTarget', 'get').mockReturnValue(input)
    handler.onHintChanged(event)

    expect(question.questionHint).toBe("NewHint");
    expect(emitter.emit).lastCalledWith(question);
  });

  it('onDisplayCommentsBox should emit an event with updated displayCommentsBox', () => {
    const EventEmitter = require("events");
    const emitter = new EventEmitter()
    emitter.emit = jest.fn();
    let question = new Question();
    question.displayComments = false;
    let handler = new QuestionEventHandler(question, emitter);

    let input = document.createElement('input');
    input.checked = true;
    let event = new Event("");
    jest.spyOn(event, 'currentTarget', 'get').mockReturnValue(input)
    handler.onDisplayCommentsBox(event)

    expect(question.displayComments).toBe(true);
    expect(emitter.emit).lastCalledWith(question);

    // input.toggleAttribute("checked");
    // expect(question.displayComments).toBe(false);
    // expect(emitter.emit).lastCalledWith(question);
  });
  