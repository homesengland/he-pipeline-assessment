import type { Components, JSX } from "../types/components";

interface ElsaMonaco extends Components.ElsaMonaco, HTMLElement {}
export const ElsaMonaco: {
  prototype: ElsaMonaco;
  new (): ElsaMonaco;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
