import type { Components, JSX } from "../types/components";

interface ElsaMultiTextProperty extends Components.ElsaMultiTextProperty, HTMLElement {}
export const ElsaMultiTextProperty: {
  prototype: ElsaMultiTextProperty;
  new (): ElsaMultiTextProperty;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
