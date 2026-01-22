import type { Components, JSX } from "../types/components";

interface ElsaDictionaryProperty extends Components.ElsaDictionaryProperty, HTMLElement {}
export const ElsaDictionaryProperty: {
  prototype: ElsaDictionaryProperty;
  new (): ElsaDictionaryProperty;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
