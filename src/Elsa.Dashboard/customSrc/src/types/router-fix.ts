// Type fix for @stencil-community/router compatibility with @stencil/core 4.x
// Addresses ariaLabel property conflict between router and core HTMLElement types

import '@stencil-community/router';

declare module '@stencil-community/router' {
  interface StencilRouteLink {
    ariaLabel?: string | null;
  }
}
