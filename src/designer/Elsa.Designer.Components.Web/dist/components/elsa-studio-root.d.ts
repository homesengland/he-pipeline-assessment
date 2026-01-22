import type { Components, JSX } from "../types/components";

interface ElsaStudioRoot extends Components.ElsaStudioRoot, HTMLElement {}
export const ElsaStudioRoot: {
  prototype: ElsaStudioRoot;
  new (): ElsaStudioRoot;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
