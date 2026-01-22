import type { Components, JSX } from "../types/components";

interface ElsaSingleLineProperty extends Components.ElsaSingleLineProperty, HTMLElement {}
export const ElsaSingleLineProperty: {
  prototype: ElsaSingleLineProperty;
  new (): ElsaSingleLineProperty;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
