import type { Components, JSX } from "../types/components";

interface ElsaInputTags extends Components.ElsaInputTags, HTMLElement {}
export const ElsaInputTags: {
  prototype: ElsaInputTags;
  new (): ElsaInputTags;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
