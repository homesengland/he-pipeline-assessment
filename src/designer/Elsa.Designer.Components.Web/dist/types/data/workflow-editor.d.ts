export interface WorkflowEditorState {
  workflowDefinitionId: string;
  serverUrl: string;
  serverFeatures: Array<string>;
}
declare const _default: {
  Provider: import("@stencil/state-tunnel/dist/types/stencil.core").FunctionalComponent<{
    state: WorkflowEditorState;
  }>;
  Consumer: import("@stencil/state-tunnel/dist/types/stencil.core").FunctionalComponent<{}>;
  injectProps: (Cstr: any, fieldList: import("@stencil/state-tunnel/dist/types/declarations").PropList<WorkflowEditorState>) => void;
};
export default _default;
