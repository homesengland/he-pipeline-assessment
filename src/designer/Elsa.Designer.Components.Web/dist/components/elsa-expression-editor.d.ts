import type { Components, JSX } from "../types/components";

interface ElsaExpressionEditor extends Components.ElsaExpressionEditor, HTMLElement {}
export const ElsaExpressionEditor: {
  prototype: ElsaExpressionEditor;
  new (): ElsaExpressionEditor;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
