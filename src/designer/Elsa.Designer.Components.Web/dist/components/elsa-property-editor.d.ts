import type { Components, JSX } from "../types/components";

interface ElsaPropertyEditor extends Components.ElsaPropertyEditor, HTMLElement {}
export const ElsaPropertyEditor: {
  prototype: ElsaPropertyEditor;
  new (): ElsaPropertyEditor;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
