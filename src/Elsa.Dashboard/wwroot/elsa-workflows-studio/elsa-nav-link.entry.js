import { r as registerInstance, h } from './index-CL6j2ec2.js';

const ElsaNavLink = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
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
        return (h("a", { key: '79f4afcd0870287b7d3d4026a1d6a59d292dacac', href: this.url, class: classes, rel: this.rel, target: this.target }, h("slot", { key: '75755a9e48ed37e0e4c06e087e9c01a63fe6c82c' })));
    }
};

export { ElsaNavLink as elsa_nav_link };
//# sourceMappingURL=elsa-nav-link.entry.esm.js.map
