// Global type augmentations for customSrc build

import '@stencil-community/router';

declare module '@stencil-community/router' {
  interface StencilRouteLink {
    ariaLabel?: string | null;
  }
}
