export interface DashboardState {
  serverUrl: string;
  basePath: string;
  culture: string;
  monacoLibPath: string;
  serverFeatures: Array<string>;
  serverVersion: string;
}
declare const _default: {
  Provider: import("@stencil/state-tunnel/dist/types/stencil.core").FunctionalComponent<{
    state: DashboardState;
  }>;
  Consumer: import("@stencil/state-tunnel/dist/types/stencil.core").FunctionalComponent<{}>;
  injectProps: (Cstr: any, fieldList: import("@stencil/state-tunnel/dist/types/declarations").PropList<DashboardState>) => void;
};
export default _default;
