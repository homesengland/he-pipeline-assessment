import type { Components, JSX } from "../types/components";

interface ElsaModalDialog extends Components.ElsaModalDialog, HTMLElement {}
export const ElsaModalDialog: {
  prototype: ElsaModalDialog;
  new (): ElsaModalDialog;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
