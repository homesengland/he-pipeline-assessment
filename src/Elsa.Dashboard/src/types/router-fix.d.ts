// Type fix for @stencil-community/router compatibility with @stencil/core 4.x
// Addresses ariaLabel property conflict between router and core HTMLElement types
// and ensures proper type exports

declare module '@stencil-community/router/dist/types/components' {
  namespace Components {
    interface StencilRouteLink {
      ariaLabel?: string | null | undefined;
    }
  }

  interface HTMLStencilRouteLinkElement {
    ariaLabel?: string | null | undefined;
  }
}

declare module '@stencil-community/router' {
  // Re-export types from the package's internal types folder
  export { default as injectHistory } from '@stencil-community/router/dist/types/global/injectHistory';
  export { ActiveRouter, Listener, LocationSegments, RouterHistory, MatchOptions, MatchResults, RouteRenderProps } from '@stencil-community/router/dist/types/global/interfaces';
  export { Components } from '@stencil-community/router/dist/types/components';
  export { matchPath } from '@stencil-community/router/dist/types/utils/match-path';
}
