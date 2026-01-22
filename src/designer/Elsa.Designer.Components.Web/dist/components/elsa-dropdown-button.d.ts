import type { Components, JSX } from "../types/components";

interface ElsaDropdownButton extends Components.ElsaDropdownButton, HTMLElement {}
export const ElsaDropdownButton: {
  prototype: ElsaDropdownButton;
  new (): ElsaDropdownButton;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
