import type { Components, JSX } from "../types/components";

interface ElsaMultiExpressionEditor extends Components.ElsaMultiExpressionEditor, HTMLElement {}
export const ElsaMultiExpressionEditor: {
  prototype: ElsaMultiExpressionEditor;
  new (): ElsaMultiExpressionEditor;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
