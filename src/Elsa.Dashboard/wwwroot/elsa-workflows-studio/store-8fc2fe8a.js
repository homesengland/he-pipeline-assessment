import { c as createStore } from './index-0d4e8807.js';

const { state, onChange } = createStore({
  activityDescriptors: [],
  workflowStorageDescriptors: [],
  monacoLibPath: '',
  useX6Graphs: false
});

const store = /*#__PURE__*/Object.freeze({
  __proto__: null,
  'default': state
});

export { state as a, store as s };
