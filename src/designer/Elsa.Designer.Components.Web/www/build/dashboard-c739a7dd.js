import { c as createProviderConsumer } from './index-2db7bf78.js';
import { h } from './index-ea213ee1.js';

const Tunnel = createProviderConsumer({
  serverUrl: null,
  basePath: null,
  culture: null,
  monacoLibPath: null,
  serverFeatures: [],
  serverVersion: null
}, (subscribe, child) => (h("context-consumer", { subscribe: subscribe, renderer: child })));

const dashboard = /*#__PURE__*/Object.freeze({
  __proto__: null,
  'default': Tunnel
});

export { Tunnel as T, dashboard as d };
