import type { Components, JSX } from "../types/components";

interface ElsaCheckListProperty extends Components.ElsaCheckListProperty, HTMLElement {}
export const ElsaCheckListProperty: {
  prototype: ElsaCheckListProperty;
  new (): ElsaCheckListProperty;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
