import type { Components, JSX } from "../types/components";

interface ElsaScriptProperty extends Components.ElsaScriptProperty, HTMLElement {}
export const ElsaScriptProperty: {
  prototype: ElsaScriptProperty;
  new (): ElsaScriptProperty;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
