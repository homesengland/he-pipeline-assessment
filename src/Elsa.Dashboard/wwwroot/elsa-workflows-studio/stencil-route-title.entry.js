import { r as registerInstance, j as getElement } from './index-1542df5c.js';
import { A as ActiveRouter } from './active-router-16dd3465.js';
import './index-2db7bf78.js';

const RouteTitle = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.titleSuffix = '';
    this.pageTitle = '';
  }
  updateDocumentTitle() {
    const el = this.el;
    if (el.ownerDocument) {
      el.ownerDocument.title = `${this.pageTitle}${this.titleSuffix || ''}`;
    }
  }
  componentWillLoad() {
    this.updateDocumentTitle();
  }
  get el() { return getElement(this); }
  static get watchers() { return {
    "pageTitle": ["updateDocumentTitle"]
  }; }
};
ActiveRouter.injectProps(RouteTitle, [
  'titleSuffix',
]);

export { RouteTitle as stencil_route_title };
