import type { Components, JSX } from "../types/components";

interface ElsaControl extends Components.ElsaControl, HTMLElement {}
export const ElsaControl: {
  prototype: ElsaControl;
  new (): ElsaControl;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
