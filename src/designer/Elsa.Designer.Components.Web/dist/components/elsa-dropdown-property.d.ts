import type { Components, JSX } from "../types/components";

interface ElsaDropdownProperty extends Components.ElsaDropdownProperty, HTMLElement {}
export const ElsaDropdownProperty: {
  prototype: ElsaDropdownProperty;
  new (): ElsaDropdownProperty;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
