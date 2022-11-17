import { Question } from '../models/custom-component-models';
import { QuestionEventHandler } from './component-events'



it('should construct', () => {
  const EventEmitter = require("events");
  const emitter = new EventEmitter()
  emitter.emit = jest.fn();
  let handler = new QuestionEventHandler(new Question, emitter)

    expect(handler).toBeTruthy();
});

