import type { Components, JSX } from "../types/components";

interface ElsaRadioListProperty extends Components.ElsaRadioListProperty, HTMLElement {}
export const ElsaRadioListProperty: {
  prototype: ElsaRadioListProperty;
  new (): ElsaRadioListProperty;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
