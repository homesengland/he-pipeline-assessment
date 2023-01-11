import { createStore } from "@stencil/store";

const { state } = createStore({
  activityDescriptors: [],
  workflowStorageDescriptors: [],
  monacoLibPath: ''
});

export default state;
