import { r as registerInstance, h } from './index-1542df5c.js';

const ElsaNavLink = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.handleClick = (event) => {
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
    };
    this.url = undefined;
    this.anchorClass = undefined;
    this.activeClass = undefined;
    this.rel = undefined;
    this.target = undefined;
  }
  isActive() {
    try {
      return !!this.url && window.location.pathname.startsWith(this.url);
    }
    catch (_a) {
      return false;
    }
  }
  render() {
    const isActive = this.isActive();
    const classes = [this.anchorClass, isActive ? this.activeClass : null].filter(x => !!x).join(' ');
    return (h("a", { href: this.url, class: classes, rel: this.rel, target: this.target, onClick: this.handleClick }, h("slot", null)));
  }
};

export { ElsaNavLink as elsa_nav_link };
