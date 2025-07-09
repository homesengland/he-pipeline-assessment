import { A as ActiveRouter } from './active-router-8e08ab72.js';
import './match-path-760e1797.js';

function injectHistory(Component) {
    ActiveRouter.injectProps(Component, ['history', 'location']);
}

export { injectHistory as i };
