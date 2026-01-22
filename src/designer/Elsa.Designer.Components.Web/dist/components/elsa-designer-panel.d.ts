import type { Components, JSX } from "../types/components";

interface ElsaDesignerPanel extends Components.ElsaDesignerPanel, HTMLElement {}
export const ElsaDesignerPanel: {
  prototype: ElsaDesignerPanel;
  new (): ElsaDesignerPanel;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
