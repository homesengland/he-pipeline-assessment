import type { Components, JSX } from "../types/components";

interface ElsaPager extends Components.ElsaPager, HTMLElement {}
export const ElsaPager: {
  prototype: ElsaPager;
  new (): ElsaPager;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
