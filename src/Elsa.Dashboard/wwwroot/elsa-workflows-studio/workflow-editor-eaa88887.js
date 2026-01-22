import { c as createProviderConsumer } from './index-2db7bf78.js';
import { h } from './index-1542df5c.js';

const Tunnel = createProviderConsumer({
  workflowDefinitionId: null,
  serverUrl: null,
  serverFeatures: []
}, (subscribe, child) => (h("context-consumer", { subscribe: subscribe, renderer: child })));

export { Tunnel as T };
