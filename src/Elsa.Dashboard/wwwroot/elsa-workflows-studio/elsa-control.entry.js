import { r as registerInstance, h, l as Host } from './index-1542df5c.js';

const ElsaControl = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.content = undefined;
  }
  render() {
    const content = this.content;
    if (typeof content === 'string')
      return h(Host, { innerHTML: content });
    if (!!content.tagName)
      return h(Host, { ref: el => this.el = el });
    return (h(Host, null, content));
  }
  componentDidLoad() {
    if (!this.el)
      return;
    const content = this.content;
    this.el.append(content);
  }
};

export { ElsaControl as elsa_control };
