import { createProviderConsumer } from "./tunnel";
import { h } from "@stencil/core";
export default createProviderConsumer({
    workflowDefinitionId: null,
    serverUrl: null,
    serverFeatures: []
}, (subscribe, child) => (h("context-consumer", { subscribe: subscribe, renderer: child })));
