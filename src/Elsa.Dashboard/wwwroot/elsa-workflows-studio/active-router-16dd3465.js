import { h } from './index-1542df5c.js';
import { c as createProviderConsumer } from './index-2db7bf78.js';

const ActiveRouter = createProviderConsumer({
    historyType: 'browser',
    location: {
        pathname: '',
        query: {},
        key: ''
    },
    titleSuffix: '',
    root: '/',
    routeViewsUpdated: () => { }
}, (subscribe, child) => (h("context-consumer", { subscribe: subscribe, renderer: child })));

export { ActiveRouter as A };
