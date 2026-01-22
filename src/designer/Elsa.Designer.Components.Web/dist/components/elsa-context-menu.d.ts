import type { Components, JSX } from "../types/components";

interface ElsaContextMenu extends Components.ElsaContextMenu, HTMLElement {}
export const ElsaContextMenu: {
  prototype: ElsaContextMenu;
  new (): ElsaContextMenu;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
