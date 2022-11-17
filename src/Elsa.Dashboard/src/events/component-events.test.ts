import {QuestionEventHandler} from './component-events'


it('should construct', () => {
    let q = jest.fn().mock({id: '', title: '', questionGuidance: 'string', questionText: 'string', displayComments: '', questionHint: 'string', questionType: 'string', questionTypeName: 'string'});
    
    let e = jest.fn().mockImplementation({emit: () => {}})
    // let e = jest.mock('@stencil/core', () => {
    //     return {
    //         emit: () => {}
    //         EventEmitter: jest.fn().mockImplementation(() => {
    //         return {
    //           emit: () => {},
    //         };
    //       })
    //     };
    //   });
    //let question = jest.mock('../models/custom-component-models', () => { return new { Question: jest.fn()}});
    let handler = new QuestionEventHandler(q, e);
    expect(handler).toBeTruthy();
});

