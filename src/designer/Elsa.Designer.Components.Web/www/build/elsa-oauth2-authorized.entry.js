import { r as registerInstance, h } from './index-ea213ee1.js';

let ElsaOauth2Authorized = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
  }
  componentDidLoad() {
    window.opener.postMessage("refreshSecrets", "*");
    window.close();
  }
  render() {
    return h("div", null, "Retrieved consent successfully.");
  }
};

export { ElsaOauth2Authorized as elsa_oauth2_authorized };
