import { r as registerInstance, h, k as Host } from './index-ea213ee1.js';

let ElsaControl = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
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
