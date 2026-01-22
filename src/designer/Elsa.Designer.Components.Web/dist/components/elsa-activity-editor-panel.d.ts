import type { Components, JSX } from "../types/components";

interface ElsaActivityEditorPanel extends Components.ElsaActivityEditorPanel, HTMLElement {}
export const ElsaActivityEditorPanel: {
  prototype: ElsaActivityEditorPanel;
  new (): ElsaActivityEditorPanel;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
