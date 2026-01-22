import type { Components, JSX } from "../types/components";

interface ElsaToastNotification extends Components.ElsaToastNotification, HTMLElement {}
export const ElsaToastNotification: {
  prototype: ElsaToastNotification;
  new (): ElsaToastNotification;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
