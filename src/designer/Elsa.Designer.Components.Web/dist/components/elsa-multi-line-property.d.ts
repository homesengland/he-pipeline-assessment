import type { Components, JSX } from "../types/components";

interface ElsaMultiLineProperty extends Components.ElsaMultiLineProperty, HTMLElement {}
export const ElsaMultiLineProperty: {
  prototype: ElsaMultiLineProperty;
  new (): ElsaMultiLineProperty;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
