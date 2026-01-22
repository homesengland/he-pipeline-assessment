import type { Components, JSX } from "../types/components";

interface ElsaSwitchCasesProperty extends Components.ElsaSwitchCasesProperty, HTMLElement {}
export const ElsaSwitchCasesProperty: {
  prototype: ElsaSwitchCasesProperty;
  new (): ElsaSwitchCasesProperty;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
