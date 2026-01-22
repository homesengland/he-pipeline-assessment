import type { Components, JSX } from "../types/components";

interface ElsaWorkflowTestPanel extends Components.ElsaWorkflowTestPanel, HTMLElement {}
export const ElsaWorkflowTestPanel: {
  prototype: ElsaWorkflowTestPanel;
  new (): ElsaWorkflowTestPanel;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
