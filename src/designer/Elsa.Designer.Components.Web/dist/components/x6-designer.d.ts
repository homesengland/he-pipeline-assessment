import type { Components, JSX } from "../types/components";

interface X6Designer extends Components.X6Designer, HTMLElement {}
export const X6Designer: {
  prototype: X6Designer;
  new (): X6Designer;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
