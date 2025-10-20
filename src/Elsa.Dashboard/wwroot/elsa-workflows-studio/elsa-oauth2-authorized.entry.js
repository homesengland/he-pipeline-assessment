import { r as registerInstance, h } from './index-CL6j2ec2.js';

const ElsaOauth2Authorized = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
    }
    componentDidLoad() {
        window.opener.postMessage("refreshSecrets", "*");
        window.close();
    }
    render() {
        return h("div", { key: 'c90d1f9c457365306965b07269874122220419ac' }, "Retrieved consent successfully.");
    }
};

export { ElsaOauth2Authorized as elsa_oauth2_authorized };
//# sourceMappingURL=elsa-oauth2-authorized.entry.esm.js.map
