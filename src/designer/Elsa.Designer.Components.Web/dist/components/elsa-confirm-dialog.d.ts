import type { Components, JSX } from "../types/components";

interface ElsaConfirmDialog extends Components.ElsaConfirmDialog, HTMLElement {}
export const ElsaConfirmDialog: {
  prototype: ElsaConfirmDialog;
  new (): ElsaConfirmDialog;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
