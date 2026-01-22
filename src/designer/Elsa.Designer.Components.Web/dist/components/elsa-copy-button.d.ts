import type { Components, JSX } from "../types/components";

interface ElsaCopyButton extends Components.ElsaCopyButton, HTMLElement {}
export const ElsaCopyButton: {
  prototype: ElsaCopyButton;
  new (): ElsaCopyButton;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
