import type { Components, JSX } from "../types/components";

interface ElsaStudioWorkflowRegistry extends Components.ElsaStudioWorkflowRegistry, HTMLElement {}
export const ElsaStudioWorkflowRegistry: {
  prototype: ElsaStudioWorkflowRegistry;
  new (): ElsaStudioWorkflowRegistry;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
