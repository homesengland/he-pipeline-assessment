import type { Components, JSX } from "../types/components";

interface ElsaJsonProperty extends Components.ElsaJsonProperty, HTMLElement {}
export const ElsaJsonProperty: {
  prototype: ElsaJsonProperty;
  new (): ElsaJsonProperty;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
