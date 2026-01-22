import type { Components, JSX } from "../types/components";

interface ElsaUserContextMenu extends Components.ElsaUserContextMenu, HTMLElement {}
export const ElsaUserContextMenu: {
  prototype: ElsaUserContextMenu;
  new (): ElsaUserContextMenu;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
