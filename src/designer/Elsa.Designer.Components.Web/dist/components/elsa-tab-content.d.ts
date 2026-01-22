import type { Components, JSX } from "../types/components";

interface ElsaTabContent extends Components.ElsaTabContent, HTMLElement {}
export const ElsaTabContent: {
  prototype: ElsaTabContent;
  new (): ElsaTabContent;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
