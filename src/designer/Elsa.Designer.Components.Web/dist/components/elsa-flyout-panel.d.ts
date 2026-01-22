import type { Components, JSX } from "../types/components";

interface ElsaFlyoutPanel extends Components.ElsaFlyoutPanel, HTMLElement {}
export const ElsaFlyoutPanel: {
  prototype: ElsaFlyoutPanel;
  new (): ElsaFlyoutPanel;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
