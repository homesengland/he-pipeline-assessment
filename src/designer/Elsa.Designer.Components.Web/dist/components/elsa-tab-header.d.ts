import type { Components, JSX } from "../types/components";

interface ElsaTabHeader extends Components.ElsaTabHeader, HTMLElement {}
export const ElsaTabHeader: {
  prototype: ElsaTabHeader;
  new (): ElsaTabHeader;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
