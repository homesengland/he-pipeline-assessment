import { Component, Prop, h } from '@stencil/core';

/**
 * Lightweight replacement for stencil-route-link to eliminate @stencil-community/router dependency.
 * Provides anchor rendering with active class evaluation based on current window.location.
 * Performs client-side navigation using history.pushState to avoid page reloads.
 * NOTE: For now, active check is a simple startsWith; can be enhanced for exact matching.
 */
@Component({
  tag: 'elsa-nav-link',
  shadow: false,
})
export class ElsaNavLink {
  /** Target URL */
  @Prop() url: string;
  /** CSS class applied to anchor */
  @Prop() anchorClass?: string;
  /** Class appended when current location matches URL */
  @Prop() activeClass?: string;
  /** Optional rel attribute */
  @Prop() rel?: string;
  /** Optional target attribute */
  @Prop() target?: string;

  private isActive(): boolean {
    try {
      return !!this.url && window.location.pathname.startsWith(this.url);
    } catch {
      return false;
    }
  }

  private handleClick = (event: MouseEvent) => {
    // Allow normal navigation for external links, new tabs, or modified clicks
    if (this.target || this.rel === 'external' || event.ctrlKey || event.metaKey || event.shiftKey) {
      return;
    }

    // Prevent default anchor behavior and use history API for SPA navigation
    event.preventDefault();

    if (this.url && window.location.pathname !== this.url) {
      window.history.pushState({}, '', this.url);
      // Dispatch a custom event that can be listened to for route changes
      window.dispatchEvent(new PopStateEvent('popstate'));
    }
  }

  render() {
    const isActive = this.isActive();
    const classes = [this.anchorClass, isActive ? this.activeClass : null].filter(x => !!x).join(' ');
    return (
      <a href={this.url} class={classes} rel={this.rel} target={this.target} onClick={this.handleClick}>
        <slot />
      </a>
    );
  }
}
