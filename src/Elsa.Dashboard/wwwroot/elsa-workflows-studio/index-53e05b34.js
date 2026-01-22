import { A as ActiveRouter } from './active-router-16dd3465.js';
import './match-path-02f6df12.js';

function injectHistory(Component) {
    ActiveRouter.injectProps(Component, ['history', 'location']);
}

export { injectHistory as i };
