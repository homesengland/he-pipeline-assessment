////Direct Import of Elsa Tunnel feature, so as to enable us to use this feature, without relying on a full import of the code and the associated negative build times.

//import { createProviderConsumer } from "@stencil/state-tunnel";
//import { h } from "@stencil/core";

//export interface WorkflowEditorState {
//  workflowDefinitionId: string;
//  serverUrl: string;
//  serverFeatures: Array<string>;
//}

//export default createProviderConsumer<WorkflowEditorState>(
//  {
//    workflowDefinitionId: null,
//    serverUrl: null,
//    serverFeatures: []
//  },
//  (subscribe, child) => (<context-consumer subscribe={subscribe} renderer={child} />)
//);
