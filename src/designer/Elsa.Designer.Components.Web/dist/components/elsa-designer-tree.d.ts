import type { Components, JSX } from "../types/components";

interface ElsaDesignerTree extends Components.ElsaDesignerTree, HTMLElement {}
export const ElsaDesignerTree: {
  prototype: ElsaDesignerTree;
  new (): ElsaDesignerTree;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
