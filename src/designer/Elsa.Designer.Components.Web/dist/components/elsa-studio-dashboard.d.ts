import type { Components, JSX } from "../types/components";

interface ElsaStudioDashboard extends Components.ElsaStudioDashboard, HTMLElement {}
export const ElsaStudioDashboard: {
  prototype: ElsaStudioDashboard;
  new (): ElsaStudioDashboard;
};
/**
 * Used to define this component and all nested components recursively.
 */
export const defineCustomElement: () => void;
