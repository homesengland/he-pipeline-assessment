import type { Components, JSX } from "../types/components";

interface ElsaStudioHome extends Components.ElsaStudioHome, HTMLElement {}
export const ElsaStudioHome: {
  prototype: ElsaStudioHome;
  new (): ElsaStudioHome;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
