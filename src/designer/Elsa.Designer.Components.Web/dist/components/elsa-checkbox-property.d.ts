import type { Components, JSX } from "../types/components";

interface ElsaCheckboxProperty extends Components.ElsaCheckboxProperty, HTMLElement {}
export const ElsaCheckboxProperty: {
  prototype: ElsaCheckboxProperty;
  new (): ElsaCheckboxProperty;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
